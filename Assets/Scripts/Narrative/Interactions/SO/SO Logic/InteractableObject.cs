using UnityEngine;

/// <summary>
/// Objeto interactuable genérico con soporte narrativo y coleccionable.
///
/// MODO NARRATIVO (isCollectable = false):
///   - Si tiene requiredFlag y NO se cumple → fallbackKnot
///   - Si se cumple (o no tiene) → inkKnot
///   - El objeto permanece en la escena
///
/// MODO COLECCIONABLE (isCollectable = true):
///   - En Start(): si ya fue recogido (flagToSetOnCollect activo) → se desactiva
///   - PulseAnimation sibling: se activa SOLO si el requiredFlag se cumple (o no existe)
///   - Interact con requiredFlag cumplido → inkKnot + SetFlag + Save + desaparece
///   - Interact sin requiredFlag → fallbackKnot, sin pulse, objeto permanece
///
/// CONFIGURACIÓN EN INSPECTOR:
///   data              → InteractableData SO (nombre, knot, isCollectable, flagToSetOnCollect)
///   requiredFlag      → Flag necesario para interactuar/recoger
///   fallbackKnot      → Knot de Ink si el requiredFlag NO se cumple
///   interactionSound  → AudioEvent opcional
///   autoTriggerOnEnter→ Interacción automática al entrar al trigger
/// </summary>
public class InteractableObject : MonoBehaviour, IInteractable
{
    public InteractableData data;

    [Header("Audio")]
    [Tooltip("Audio que se reproduce al interactuar exitosamente")]
    public AudioEvent interactionSound;

    [Tooltip("Opcional: Si escribes un flag aquí (ej: 'Ruta Terminada'), el objeto o NPC no se podrá interactuar hasta conseguirlo.")]
    public string requiredFlag = "";

    [Tooltip("Opcional: Nudo de Ink que se reproducirá si el jugador NO tiene el flag requerido.")]
    public string fallbackKnot = "";

    [Tooltip("Activa esta casilla para que la interacción empiece automáticamente en cuanto el jugador toque el objeto (Trigger), sin tener que presionar el botón.")]
    public bool autoTriggerOnEnter = false;

    /// <summary>Referencia cacheada al PulseAnimation sibling (solo relevante si isCollectable).</summary>
    private PulseAnimation pulseAnimation;

    // ─────────────────────────────────────────────────────────────────────────
    // Ciclo de vida
    // ─────────────────────────────────────────────────────────────────────────

    private void Start()
    {
        if (data != null && data.isCollectable)
        {
            pulseAnimation = GetComponentInChildren<PulseAnimation>();
            EvaluateVisibility();
            EvaluatePulse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (autoTriggerOnEnter && other.CompareTag("Player"))
        {
            // Evitar que el diálogo inicie si ya hay otro corriendo
            if (StoryManager.Instance != null && !StoryManager.Instance.IsDialogueActive)
            {
                Interact();
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Collectable: Visibilidad y Pulse
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Si el objeto ya fue recogido en una sesión anterior, se desactiva.
    /// Solo aplica cuando data.isCollectable = true.
    /// </summary>
    private void EvaluateVisibility()
    {
        if (GameManager.Instance == null) return;

        if (!string.IsNullOrEmpty(data.flagToSetOnCollect) &&
            GameManager.Instance.GetStoryFlag(data.flagToSetOnCollect))
        {
            gameObject.SetActive(false);
            Debug.Log($"[InteractableObject] '{gameObject.name}' ya fue recogido. Desactivado.");
        }
    }

    /// <summary>
    /// Activa o desactiva el PulseAnimation según si el jugador puede recoger el objeto.
    /// - Sin requiredFlag → siempre pulsa.
    /// - Con requiredFlag y el jugador lo tiene → pulsa.
    /// - Con requiredFlag y el jugador NO lo tiene → no pulsa.
    /// </summary>
    private void EvaluatePulse()
    {
        if (pulseAnimation == null) return;

        bool shouldPulse = CanCollect();
        pulseAnimation.SetPulseEnabled(shouldPulse);

        Debug.Log($"[InteractableObject] '{gameObject.name}' pulse: {shouldPulse}");
    }

    /// <summary>
    /// Determina si el jugador actualmente puede recoger este objeto coleccionable.
    /// </summary>
    private bool CanCollect()
    {
        // Sin requiredFlag → siempre colectable
        if (string.IsNullOrEmpty(requiredFlag)) return true;

        // Con requiredFlag → verificar estado
        return GameManager.Instance != null &&
               GameManager.Instance.GetStoryFlag(requiredFlag);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // IInteractable
    // ─────────────────────────────────────────────────────────────────────────

    public string GetInteractionName()
    {
        return data != null ? data.interactionName : gameObject.name;
    }

    public void Interact()
    {
        // Re-evaluar pulse por si el flag cambió durante gameplay
        if (data != null && data.isCollectable)
        {
            EvaluatePulse();
        }

        // ── Verificación de requiredFlag (aplica tanto a narrativo como coleccionable) ──
        if (!string.IsNullOrEmpty(requiredFlag))
        {
            if (GameManager.Instance == null || !GameManager.Instance.GetStoryFlag(requiredFlag))
            {
                Debug.Log($"[InteractableObject] Interacción bloqueada. Falta el flag: {requiredFlag}");

                if (!string.IsNullOrEmpty(fallbackKnot) && StoryManager.Instance != null)
                {
                    StoryManager.Instance.StartStory(fallbackKnot);
                }

                return;
            }
        }

        if (data == null)
        {
            Debug.LogWarning("[InteractableObject] Interactable sin data asignada en: " + gameObject.name);
            return;
        }

        // ── Modo coleccionable: recoger ──
        if (data.isCollectable)
        {
            Collect();
            return;
        }

        // ── Modo narrativo normal ──
        Debug.Log("[InteractableObject] Interact() ejecutado. Knot: '" + data.inkKnot + "'");

        if (StoryManager.Instance == null)
        {
            Debug.LogError("[InteractableObject] StoryManager.Instance es NULL. ¿Está en la escena?");
            return;
        }

        StoryManager.Instance.StartStory(data.inkKnot);

        // Reproducir sonido de interacción
        if (interactionSound != null)
        {
            interactionSound.PlaySFX();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Recolección
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Ejecuta la secuencia completa de recolección:
    ///   1. Disparar diálogo Ink (inkKnot)
    ///   2. Registrar flag en GameState (flagToSetOnCollect)
    ///   3. Checkpoint save
    ///   4. Sonido opcional
    ///   5. Desactivar el objeto
    /// </summary>
    private void Collect()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("[InteractableObject] GameManager no disponible para recolección.");
            return;
        }

        // Guard: si ya fue recogido (condición de carrera), ignorar
        if (!string.IsNullOrEmpty(data.flagToSetOnCollect) &&
            GameManager.Instance.GetStoryFlag(data.flagToSetOnCollect))
        {
            Debug.Log($"[InteractableObject] '{data.interactionName}' ya fue recogido. Interacción ignorada.");
            gameObject.SetActive(false);
            return;
        }

        // 1. Disparar diálogo Ink
        if (!string.IsNullOrEmpty(data.inkKnot))
        {
            if (StoryManager.Instance != null && !StoryManager.Instance.IsDialogueActive)
            {
                StoryManager.Instance.StartStory(data.inkKnot);
            }
            else if (StoryManager.Instance == null)
            {
                Debug.LogError("[InteractableObject] StoryManager.Instance es NULL.");
            }
        }

        // 2. Registrar en GameState
        if (!string.IsNullOrEmpty(data.flagToSetOnCollect))
        {
            GameManager.Instance.SetStoryFlag(data.flagToSetOnCollect, true);
        }
        
        // 2.5 Incrementar variable si existe
        if (!string.IsNullOrEmpty(data.variableToIncrementOnCollect))
        {
            int newVal = GameManager.Instance.IncrementStoryVariable(data.variableToIncrementOnCollect, data.incrementAmount);
            Debug.Log($"[InteractableObject] Variable '{data.variableToIncrementOnCollect}' incrementada a {newVal}.");
        }

        // 3. Checkpoint save — persistir inmediatamente
        GameManager.Instance.SaveGame();

        // 4. Sonido opcional
        if (interactionSound != null)
        {
            interactionSound.PlaySFX();
        }

        // 5. El objeto desaparece
        gameObject.SetActive(false);

        Debug.Log($"[InteractableObject] '{data.interactionName}' recogido. Flag: '{data.flagToSetOnCollect}'. Guardado.");
    }
}