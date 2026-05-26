using UnityEngine;

/// <summary>
/// Objeto físico interactuable exclusivo del prólogo.
/// Funciona igual que InteractableObject pero con comportamiento adicional:
///   - Se desactiva automáticamente si ya fue recogido (GameState)
///   - Se desactiva si el prólogo ya fue completado
///   - Al interactuar: dispara diálogo Ink, registra flag, hace checkpoint save y desaparece
///
/// CONFIGURACIÓN EN INSPECTOR:
///   flagToSetOnCollect  → Flag que se activa al recoger (ej: "prologue_arcade_item_collected")
///   inkKnotOnInteract   → Knot de Ink a disparar al interactuar (ej: "prologo_arcade_recoger_objeto")
///   interactionName     → Nombre que ve el jugador en la UI de interacción
///   interactionSound    → AudioEvent opcional al recoger
///
/// REGLA: Solo editar flagToSetOnCollect e inkKnotOnInteract por Inspector.
///        No hardcodear lógica de prólogo aquí — eso es responsabilidad de PrologueManager.
/// </summary>
public class PrologueItemInteractable : MonoBehaviour, IInteractable
{
    [Header("Prologue Item Config")]
    [Tooltip("Flag de GameState que se activará al recoger este objeto. " +
             "También se usa para detectar si ya fue recogido al recargar la escena.")]
    public string flagToSetOnCollect;

    [Tooltip("Knot de Ink que se dispara al interactuar con este objeto.")]
    public string inkKnotOnInteract;

    [Tooltip("Nombre visible para el jugador al acercarse (usado por el sistema de interacción).")]
    [SerializeField] private string interactionName = "Objeto";

    [Header("Audio")]
    [Tooltip("Sonido al recoger el objeto (opcional).")]
    public AudioEvent interactionSound;

    [Header("Visual Feedback (Emission Pulse)")]
    [Tooltip("¿Debe el objeto pulsar su emisión para llamar la atención?")]
    [SerializeField] private bool useEmissionPulse = true;

    [Tooltip("Color de la emisión (Soporta HDR).")]
    [ColorUsage(true, true)]
    [SerializeField] private Color emissionColor = Color.white;

    [Tooltip("Velocidad de la pulsación.")]
    [SerializeField] private float pulseSpeed = 2f;

    [Tooltip("Intensidad máxima de la emisión.")]
    [SerializeField] private float maxIntensity = 1.5f;

    private Renderer _renderer;
    private Material _material;
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");

    // ─────────────────────────────────────────────────────────────────────────
    // Inicialización — auto-desactivación según GameState
    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        // Si el GameManager aún no está listo, diferimos la comprobación a Start
    }

    private void Start()
    {
        EvaluateVisibility();
        SetupEmission();
    }

    /// <summary>
    /// Verifica si este objeto debe estar visible:
    ///   - Si el prólogo ya está completado → desactivar
    ///   - Si ya fue recogido (flag activo) → desactivar
    ///   - Si el prólogo no ha llegado a esta escena aún → el objeto espera (visible pero sin interactuar)
    /// </summary>
    private void EvaluateVisibility()
    {
        if (GameManager.Instance == null) return;

        // Si el prólogo ya terminó: nunca debe estar visible
        if (GameManager.Instance.GetStoryFlag(PrologueManager.FLAG_COMPLETED))
        {
            gameObject.SetActive(false);
            return;
        }

        // Si este objeto ya fue recogido en una sesión anterior: desactivar
        if (!string.IsNullOrEmpty(flagToSetOnCollect) &&
            GameManager.Instance.GetStoryFlag(flagToSetOnCollect))
        {
            gameObject.SetActive(false);
        }
    }

    private void SetupEmission()
    {
        if (useEmissionPulse)
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer != null)
            {
                // Acceder a .material crea una instancia única para este objeto
                _material = _renderer.material;
                _material.EnableKeyword("_EMISSION");
            }
        }
    }

    private void Update()
    {
        if (useEmissionPulse && _material != null)
        {
            // Efecto de pulsación suave usando Seno basado en el tiempo
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // Rango 0..1
            float currentIntensity = pulse * maxIntensity;
            
            _material.SetColor(EmissionColorProperty, emissionColor * currentIntensity);
        }
    }

    private void OnDestroy()
    {
        // Limpiar la instancia de material para evitar fugas de memoria
        if (_material != null)
        {
            Destroy(_material);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // IInteractable
    // ─────────────────────────────────────────────────────────────────────────

    public string GetInteractionName()
    {
        return interactionName;
    }

    public void Interact()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[PrologueItemInteractable] GameManager no disponible.");
            return;
        }

        // Guard: si ya fue recogido (puede ocurrir en condiciones de carrera), ignorar
        if (!string.IsNullOrEmpty(flagToSetOnCollect) &&
            GameManager.Instance.GetStoryFlag(flagToSetOnCollect))
        {
            Debug.Log($"[PrologueItemInteractable] '{interactionName}' ya fue recogido. Interacción ignorada.");
            gameObject.SetActive(false);
            return;
        }

        // 1. Disparar diálogo Ink
        if (!string.IsNullOrEmpty(inkKnotOnInteract))
        {
            if (StoryManager.Instance != null && !StoryManager.Instance.IsDialogueActive)
            {
                StoryManager.Instance.StartStory(inkKnotOnInteract);
            }
            else if (StoryManager.Instance == null)
            {
                Debug.LogError("[PrologueItemInteractable] StoryManager.Instance es NULL.");
            }
        }

        // 2. Registrar en GameState
        if (!string.IsNullOrEmpty(flagToSetOnCollect))
        {
            GameManager.Instance.SetStoryFlag(flagToSetOnCollect, true);
        }

        // 3. Checkpoint save — persistir inmediatamente, sin esperar cambio de escena
        GameManager.Instance.SaveGame();

        // 4. Sonido opcional
        if (interactionSound != null)
        {
            interactionSound.PlaySFX();
        }

        // 5. El objeto desaparece
        gameObject.SetActive(false);

        Debug.Log($"[PrologueItemInteractable] '{interactionName}' recogido. Flag: '{flagToSetOnCollect}'. Guardado.");
    }
}
