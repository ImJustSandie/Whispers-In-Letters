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
        !GameManager.Instance.GetStoryFlag(FLAG_FINAL_SEEN);

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
        if (!IsPrologueActive) return;

        // Esperamos un frame para que todos los objetos de escena se inicialicen
        // (incluyendo StoryManager y PrologueItemInteractable)
        StartCoroutine(HandleSceneRoutine(scene.name));
    }

    private IEnumerator HandleSceneRoutine(string sceneName)
    {
        yield return null; // Un frame de gracia para inicialización

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[PrologueManager] GameManager no disponible. Prólogo pausado.");
            yield break;
        }

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

    // ─────────────────────────────────────────────────────────────────────────
    // Handlers por escena
    // ─────────────────────────────────────────────────────────────────────────

    private void HandleParqueLoaded()
    {
        // Si pusiste 'prologue_completed' desde Ink, entonces Joseph ya está en la escena.
        // Ahora disparamos su diálogo final y cerramos definitivamente el Manager.
        if (GameManager.Instance.GetStoryFlag(FLAG_COMPLETED))
        {
            if (!GameManager.Instance.GetStoryFlag(FLAG_FINAL_SEEN)) 
            {
                TriggerDialogue(KNOT_PARQUE_FINAL);
                CompletePrologue();
            }
        }
        else if (!GameManager.Instance.GetStoryFlag(FLAG_ARCADE_VISITED))
        {
            // Primera vez en el Parque: diálogo de inicio
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
        GameManager.Instance.SetStoryFlag(FLAG_FINAL_SEEN, true);
        GameManager.Instance.SaveGame();
        Debug.Log("[PrologueManager] Prólogo completado. Guardando estado.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    private void TriggerDialogue(string knot)
    {
        if (StoryManager.Instance == null)
        {
            Debug.LogWarning($"[PrologueManager] StoryManager no disponible para knot '{knot}'.");
            return;
        }

        if (StoryManager.Instance.IsDialogueActive)
        {
            Debug.LogWarning($"[PrologueManager] Ya hay un diálogo activo. Knot '{knot}' ignorado.");
            return;
        }

        Debug.Log($"[PrologueManager] Disparando diálogo: '{knot}'");
        StoryManager.Instance.StartStory(knot);
    }
}
