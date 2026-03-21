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

    /// <summary>
    /// Cambia a la escena indicada aplicando transiciones visuales.
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName.Trim()));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
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

        // 2. Registrar el historial en la persistencia atraves del GameManager
        if (GameManager.Instance != null && GameManager.Instance.GetGameState() != null)
        {
            var state = GameManager.Instance.GetGameState();
            state.previousSceneName = SceneManager.GetActiveScene().name;
            state.currentSceneName = sceneName;
        }

        // 3. Carga asincrona para no congelar el juego
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        if (asyncLoad == null)
        {
            Debug.LogError($"[LevelManager] No se pudo cargar la escena '{sceneName}'. ¿Aseguraste agregarla en File -> Build Settings?");
            yield break; // Detener la corrutina para evitar errores
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

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
    }
}
