using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("UI Transitions")]
    [Tooltip("El CanvasGroup que posee el cuadro negro de fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Audio")]
    [Tooltip("Sonido a reproducir cuando se transiciona a otra escena")]
    [SerializeField] private AudioEvent sceneTransitionSound;

    /// <summary>Duración configurada del fade, para que otros sistemas puedan sincronizar sus waits.</summary>
    public float FadeDuration => fadeDuration;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Si hay un canvas de transicion, lo apagamos por defecto
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                fadeCanvasGroup.blocksRaycasts = false;
                fadeCanvasGroup.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadStopwatch.IsRunning)
        {
            loadStopwatch.Stop();

        }
        else
        {

        }

        if (!isTransitioning)
        {
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                fadeCanvasGroup.blocksRaycasts = false;
                fadeCanvasGroup.gameObject.SetActive(false);
            }
        }
    }

    private bool isTransitioning = false;
    private Coroutine activeFadeRoutine;
    private readonly System.Diagnostics.Stopwatch loadStopwatch = new System.Diagnostics.Stopwatch();

    /// <summary>
    /// Cambia a la escena indicada aplicando transiciones visuales.
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionToScene(sceneName.Trim()));
    }

    /// <summary>
    /// Inicia un oscurecimiento gradual de la pantalla.
    /// </summary>
    public void FadeToBlack(float duration = -1f)
    {
        if (isTransitioning) return;
        float d = duration < 0 ? fadeDuration : duration;
        if (activeFadeRoutine != null) StopCoroutine(activeFadeRoutine);
        activeFadeRoutine = StartCoroutine(FadeRoutine(0f, 1f, d, true, () => activeFadeRoutine = null));
    }

    public void FadeToClear(float duration = -1f)
    {
        if (isTransitioning) return;
        float d = duration < 0 ? fadeDuration : duration;
        if (activeFadeRoutine != null) StopCoroutine(activeFadeRoutine);
        activeFadeRoutine = StartCoroutine(FadeRoutine(1f, 0f, d, false, () => activeFadeRoutine = null));
    }

    public IEnumerator FadeToBlackRoutine(float duration = -1f)
    {
        float d = duration < 0 ? fadeDuration : duration;
        yield return FadeRoutine(0f, 1f, d, true);
    }

    public IEnumerator FadeToClearRoutine(float duration = -1f)
    {
        float d = duration < 0 ? fadeDuration : duration;
        yield return FadeRoutine(1f, 0f, d, false);
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration, bool blockRaycasts, Action onComplete = null)
    {
        if (fadeCanvasGroup == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.blocksRaycasts = blockRaycasts;

        float timer = 0f;
        while (timer < duration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
        
        if (endAlpha <= 0)
        {
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.gameObject.SetActive(false);
        }

        onComplete?.Invoke();
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        isTransitioning = true;

        // Cerrar el panel de configuración antes de cualquier transición
        if (sceneName != "Menu" && GameManager.Instance != null && GameManager.Instance.uiAjustes != null)
        {
            GameManager.Instance.uiAjustes.CerrarAjustes();
        }

        if (sceneTransitionSound != null)
        {
            sceneTransitionSound.PlaySFX();
        }
        
        // 1. Efecto Fade Out (Oscurecer pantalla)
        yield return StartCoroutine(FadeRoutine(0f, 1f, fadeDuration, true));

        // 2. Registrar el historial en la persistencia a través del GameManager
        if (GameManager.Instance != null && GameManager.Instance.GetGameState() != null)
        {
            var state = GameManager.Instance.GetGameState();
            state.previousSceneName = SceneManager.GetActiveScene().name;
            
            // Solo actualizamos la escena actual si NO vamos al menú.
            // Esto permite que el botón de "Continuar" sepa a qué nivel volver.
            if (sceneName != "Menu")
            {
                state.currentSceneName = sceneName;
            }

            // Auto-save: persistir en disco antes de cambiar de escena.
            // Así, si el juego se cierra durante la carga, el progreso no se pierde.
            GameManager.Instance.SaveGame();
        }

        // 3. Carga asíncrona para no congelar el juego
        loadStopwatch.Reset();
        loadStopwatch.Start();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        if (asyncLoad == null)
        {
            loadStopwatch.Stop();

            isTransitioning = false;
            yield break; // Detener la corrutina para evitar errores
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 3.5 Reubicar al jugador en el SpawnPoint correcto
        yield return null; // Esperar un frame para que todos los objetos de la nueva escena se inicialicen
        HandlePlayerSpawn();

        // 3.6 Liberar assets huérfanos y forzar recolección de basura
        yield return Resources.UnloadUnusedAssets();
        GC.Collect();
        GC.WaitForPendingFinalizers();

        // 4. Efecto Fade In (Aclarar pantalla)
        yield return StartCoroutine(FadeRoutine(1f, 0f, fadeDuration, false));

        isTransitioning = false;
    }

    /// <summary>
    /// Busca al jugador por Tag y lo reubica en el SpawnPoint correspondiente a la escena anterior.
    /// </summary>
    private void HandlePlayerSpawn()
    {
        if (GameManager.Instance == null || GameManager.Instance.GetGameState() == null)
        {

            return;
        }

        string previousScene = GameManager.Instance.GetGameState().previousSceneName;


        if (string.IsNullOrEmpty(previousScene)) return;

        // Buscar al jugador por Tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {

            return;
        }

        // Buscar SpawnPoints
        SpawnPoint[] spawnPoints = UnityEngine.Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);


        foreach (SpawnPoint sp in spawnPoints)
        {


            if (sp.fromSceneName.Trim().Equals(previousScene.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                // Desactivar CharacterController temporalmente para poder teletransportar
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                player.transform.position = sp.transform.position;
                player.transform.rotation = sp.transform.rotation;

                if (cc != null) cc.enabled = true;


                return;
            }
        }


    }
}
