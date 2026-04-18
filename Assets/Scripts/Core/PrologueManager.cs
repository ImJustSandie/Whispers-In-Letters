using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Orquesta el flujo del prólogo jugable de "Whispers in Letters".
///
/// RESPONSABILIDADES:
///   - Detectar el cambio de cada escena y aplicar la lógica contextual del prólogo
///   - Disparar los diálogos de entrada (StoryManager) según el estado actual
///   - Activar/desactivar los objetos del prólogo en cada escena según GameState
///   - Marcar el prólogo como completado y guardar el estado
///
/// LO QUE NO HACE:
///   - No maneja UI directamente
///   - No escribe a disco (eso es SaveSystem vía GameManager)
///   - No modifica objetos de escena fuera de los registrados
///   - No contiene lógica narrativa (eso es Ink)
///
/// CUÁNDO ESTÁ ACTIVO:
///   - Solo cuando el flag "prologue_completed" NO está activo en GameState
///   - Una vez completado, es completamente silencioso
///
/// INTEGRACIÓN EN UNITY:
///   - Añadir como componente en el mismo GameObject del GameManager (prefab raíz)
///   - Se inicializa solo en Awake y escucha eventos de escena automáticamente
/// </summary>
public class PrologueManager : MonoBehaviour
{
    public static PrologueManager Instance;

    // ─── Nombres de escena ────────────────────────────────────────────────────
    private const string SCENE_PARQUE     = "Parque";
    private const string SCENE_ARCADE     = "Arcade";
    private const string SCENE_BIBLIOTECA = "Biblioteca";

    // ─── Flags del GameState (solo PrologueManager los escribe) ──────────────
    public const string FLAG_ARCADE_VISITED         = "prologue_arcade_visited";
    public const string FLAG_ARCADE_ITEM_COLLECTED  = "prologue_arcade_item_collected";
    public const string FLAG_LIBRARY_VISITED        = "prologue_library_visited";
    public const string FLAG_LIBRARY_ITEM_COLLECTED = "prologue_library_item_collected";
    public const string FLAG_COMPLETED              = "prologue_completed"; // Seteado por ti en Ink para hacer aparecer a Joseph
    public const string FLAG_FINAL_SEEN             = "prologue_final_seen"; // Usado para cerrar definitivamente el manager

    // ─── Knots de Ink (deben existir en Prologo.ink) ─────────────────────────
    private const string KNOT_PARQUE_INICIO          = "prologo_parque_inicio";
    private const string KNOT_ARCADE_LLEGADA         = "prologo_arcade_llegada";
    private const string KNOT_BIBLIOTECA_LLEGADA     = "prologo_biblioteca_llegada";
    private const string KNOT_PARQUE_FINAL           = "prologo_parque_final";

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>True si el diálogo final aún no ha sido visto.</summary>
    public bool IsPrologueActive =>
        GameManager.Instance != null &&
        !GameManager.Instance.GetStoryFlag(GameManager.Instance.CompletionFlag);

    // ─────────────────────────────────────────────────────────────────────────
    // Ciclo de vida
    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
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

    // ─────────────────────────────────────────────────────────────────────────
    // Lógica principal — se ejecuta en cada cambio de escena
    // ─────────────────────────────────────────────────────────────────────────

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool active = IsPrologueActive;
        Debug.Log($"[PrologueManager] Escena cargada: {scene.name}. Activo: {active}");
        
        if (!active) return;

        // Esperamos un frame para que todos los objetos de escena se inicialicen
        // (incluyendo StoryManager y PrologueItemInteractable)
        StartCoroutine(HandleSceneRoutine(scene.name));
    }

    private IEnumerator HandleSceneRoutine(string sceneName)
    {
        float timeout = 3.0f;
        float elapsed = 0f;

        // 1. Esperar activamente a que las dependencias estén listas
        // (StoryManager se inicializa en Awake de la nueva escena, puede haber delay de carga)
        while (StoryManager.Instance == null || StoryManager.Instance.Story == null)
        {
            elapsed += Time.deltaTime;
            if (elapsed > timeout)
            {
                Debug.LogError($"[PrologueManager] TIMEOUT: StoryManager no disponible tras {timeout}s en {sceneName}.");
                yield break;
            }
            yield return null;
        }

        // 2. Margen de seguridad para estabilidad de la UI y otros scripts de la escena
        yield return new WaitForSeconds(0.2f);

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[PrologueManager] GameManager no disponible. Prólogo pausado.");
            yield break;
        }

        // 3. Generar reporte de estado para depuración en consola
        LogStatusReport(sceneName);

        switch (sceneName)
        {
            case SCENE_PARQUE:
                HandleParqueLoaded();
                break;
            case SCENE_ARCADE:
                HandleArcadeLoaded();
                break;
            case SCENE_BIBLIOTECA:
                HandleBibliotecaLoaded();
                break;
        }
    }

    /// <summary>Imprime el estado de todos los flags relevantes para el prólogo.</summary>
    private void LogStatusReport(string sceneName)
    {
        var gm = GameManager.Instance;
        bool isFinished = gm.GetStoryFlag(gm.CompletionFlag);
        bool completed = gm.GetStoryFlag(FLAG_COMPLETED);
        bool arcadeVisited = gm.GetStoryFlag(FLAG_ARCADE_VISITED);
        bool libVisited = gm.GetStoryFlag(FLAG_LIBRARY_VISITED);

        Debug.Log($"[PrologueManager] --- REPORTE DE ESTADO ({sceneName}) ---");
        Debug.Log($"- Escena: {sceneName}");
        Debug.Log($"- Prólogo Activo (Basado en {gm.CompletionFlag}): {!isFinished}");
        Debug.Log($"- Flags Internos -> Completed: {completed}, ArcadeVisited: {arcadeVisited}, LibVisited: {libVisited}");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Handlers por escena
    // ─────────────────────────────────────────────────────────────────────────

    private void HandleParqueLoaded()
    {
        bool completed = GameManager.Instance.GetStoryFlag(FLAG_COMPLETED);
        bool arcadeVisited = GameManager.Instance.GetStoryFlag(FLAG_ARCADE_VISITED);

        Debug.Log($"[PrologueManager] Parque: completed={completed}, arcadeVisited={arcadeVisited}");

        if (completed)
        {
            if (!GameManager.Instance.GetStoryFlag(GameManager.Instance.CompletionFlag)) 
            {
                TriggerDialogue(KNOT_PARQUE_FINAL);
                CompletePrologue();
            }
        }
        else if (!arcadeVisited)
        {
            TriggerDialogue(KNOT_PARQUE_INICIO);
        }
    }

    private void HandleArcadeLoaded()
    {
        if (GameManager.Instance.GetStoryFlag(FLAG_ARCADE_ITEM_COLLECTED))
        {
            // Ya recogió el objeto: nada que hacer aquí
            return;
        }

        if (!GameManager.Instance.GetStoryFlag(FLAG_ARCADE_VISITED))
        {
            GameManager.Instance.SetStoryFlag(FLAG_ARCADE_VISITED, true);
            TriggerDialogue(KNOT_ARCADE_LLEGADA);
        }
    }

    private void HandleBibliotecaLoaded()
    {
        if (GameManager.Instance.GetStoryFlag(FLAG_LIBRARY_ITEM_COLLECTED))
        {
            // Ya recogió el objeto: nada que hacer aquí
            return;
        }

        if (!GameManager.Instance.GetStoryFlag(FLAG_LIBRARY_VISITED))
        {
            GameManager.Instance.SetStoryFlag(FLAG_LIBRARY_VISITED, true);
            TriggerDialogue(KNOT_BIBLIOTECA_LLEGADA);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Completar prólogo
    // ─────────────────────────────────────────────────────────────────────────

    private void CompletePrologue()
    {
        GameManager.Instance.SetStoryFlag(GameManager.Instance.CompletionFlag, true);
        GameManager.Instance.SaveGame();
        Debug.Log($"[PrologueManager] Prólogo completado. Marcando flag: {GameManager.Instance.CompletionFlag}");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    private void TriggerDialogue(string knot)
    {
        if (StoryManager.Instance == null)
        {
            Debug.LogError($"[PrologueManager] ERROR: StoryManager.Instance es NULO al intentar disparar '{knot}'.");
            return;
        }

        if (StoryManager.Instance.IsDialogueActive)
        {
            Debug.LogWarning($"[PrologueManager] BLOQUEADO: Ya hay un diálogo activo. Ignorando KNOT: '{knot}'.");
            return;
        }

        Debug.Log($"[PrologueManager] >>> DISPARANDO DIÁLOGO: '{knot}'");
        StoryManager.Instance.StartStory(knot);
    }
}
