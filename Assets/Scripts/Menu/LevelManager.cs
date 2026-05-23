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
        // Limpieza de seguridad: Al entrar en cualquier escena, la pantalla debe estar clara
        // y las transiciones detenidas.
        isTransitioning = false;

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.gameObject.SetActive(false);
        }
        
        Debug.Log($"[LevelManager] Reset de fade y transición completado en {scene.name}");
    }

    private bool isTransitioning = false;

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
        float d = duration < 0 ? fadeDuration : duration;
        StartCoroutine(FadeRoutine(0f, 1f, d, true));
    }

    /// <summary>
    /// Inicia un aclaramiento gradual de la pantalla.
    /// </summary>
    public void FadeToClear(float duration = -1f)
    {
        float d = duration < 0 ? fadeDuration : duration;
        StartCoroutine(FadeRoutine(1f, 0f, d, false));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration, bool blockRaycasts)
    {
        if (fadeCanvasGroup == null) yield break;

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
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true); // Prenderlo automáticamente
            fadeCanvasGroup.blocksRaycasts = true;
            float timer = 0f;
            while (timer < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        if (asyncLoad == null)
        {
            Debug.LogError($"[LevelManager] No se pudo cargar la escena '{sceneName}'. ¿Aseguraste agregarla en File -> Build Settings?");
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

        // 4. Efecto Fade In (Aclarar pantalla)
        if (fadeCanvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.gameObject.SetActive(false); // Apagarlo para que no estorbe en el Editor ni en el juego
        }

        isTransitioning = false;
    }

    /// <summary>
    /// Busca al jugador por Tag y lo reubica en el SpawnPoint correspondiente a la escena anterior.
    /// </summary>
    private void HandlePlayerSpawn()
    {
        if (GameManager.Instance == null || GameManager.Instance.GetGameState() == null)
        {
            Debug.Log("[LevelManager] No hay GameManager/GameState. Spawn omitido.");
            return;
        }

        string previousScene = GameManager.Instance.GetGameState().previousSceneName;
        Debug.Log($"[LevelManager] Buscando SpawnPoint para escena anterior: '{previousScene}'");

        if (string.IsNullOrEmpty(previousScene)) return;

        // Buscar al jugador por Tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[LevelManager] No se encontro jugador con Tag 'Player' en la escena.");
            return;
        }

        // Buscar SpawnPoints
        SpawnPoint[] spawnPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        Debug.Log($"[LevelManager] SpawnPoints encontrados: {spawnPoints.Length}");

        foreach (SpawnPoint sp in spawnPoints)
        {
            Debug.Log($"[LevelManager] Comparando SpawnPoint '{sp.fromSceneName}' con '{previousScene}'");

            if (sp.fromSceneName.Trim().Equals(previousScene.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                // Desactivar CharacterController temporalmente para poder teletransportar
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                player.transform.position = sp.transform.position;
                player.transform.rotation = sp.transform.rotation;

                if (cc != null) cc.enabled = true;

                Debug.Log($"[LevelManager] Jugador reubicado en SpawnPoint de '{previousScene}'");
                return;
            }
        }

        Debug.LogWarning($"[LevelManager] No se encontro SpawnPoint para '{previousScene}'.");
    }
}
