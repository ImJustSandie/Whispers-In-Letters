using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objeto interactuable avanzado con condiciones por ScriptableObject (AdvancedInteractableData).
///
/// NUEVAS CAPACIDADES:
///   1. Condiciones de visibilidad: el objeto solo es visible si se cumplen
///      todas las condiciones de visibilityConditions en Start().
///   2. Desaparición con fade: si disappearAfterDialogue = true, al terminar
///      el diálogo activo el objeto ejecuta FadeToBlack → desaparece → FadeToClear,
///      pero solo si disappearCondition se cumple en ese momento.
/// </summary>
public class AdvancedInteractableObject : MonoBehaviour, IInteractable
{
    public AdvancedInteractableData data;

    [Header("Audio")]
    [Tooltip("Audio que se reproduce al interactuar exitosamente")]
    public AudioEvent interactionSound;

    // ─────────────────────────────────────────────────────────────────────────
    // Visibilidad condicional
    // ─────────────────────────────────────────────────────────────────────────

    [Header("Auto Trigger (Hitbox)")]
    [Tooltip("Activa esta casilla para que la interacción empiece automáticamente al entrar al Trigger, sin esperar a que se presione el botón.")]
    public bool autoTriggerOnEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (autoTriggerOnEnter && other.CompareTag("Player"))
        {
            if (StoryManager.Instance != null && !StoryManager.Instance.IsDialogueActive)
            {
                Interact();
            }
        }
    }

    [Header("Visibility Conditions")]
    [Tooltip("Si la lista está vacía, el objeto siempre es visible al cargar la escena. " +
             "Si contiene condiciones, TODAS deben cumplirse o el objeto se desactiva.")]
    public List<InteractionCondition> visibilityConditions = new List<InteractionCondition>();

    // ─────────────────────────────────────────────────────────────────────────
    // Desaparición con fade al terminar diálogo
    // ─────────────────────────────────────────────────────────────────────────

    [Header("Disappear After Dialogue")]
    [Tooltip("Si está activo, este objeto desaparecerá con un fade al terminar el diálogo, " +
             "siempre que disappearCondition se cumpla.")]
    public bool disappearAfterDialogue = false;

    [Tooltip("Condición que debe cumplirse DESPUÉS del diálogo para disparar la desaparición. " +
             "Usa RequerirFlag o RequerirVariable. ConditionType.Ninguna = siempre desaparece.")]
    public InteractionCondition disappearCondition;

    [Tooltip("Duración del fade en segundos al desaparecer. -1 usa el valor del LevelManager.")]
    public float disappearFadeDuration = -1f;

    // ─────────────────────────────────────────────────────────────────────────
    // Ciclo de vida
    // ─────────────────────────────────────────────────────────────────────────

    private void Start()
    {
        EvaluateVisibility();

        if (disappearAfterDialogue && StoryManager.Instance != null)
        {
            StoryManager.Instance.OnDialogueStateChanged += OnDialogueStateChanged;
        }
    }

    private void OnDestroy()
    {
        // Limpiar suscripción para evitar memory leaks
        if (StoryManager.Instance != null)
        {
            StoryManager.Instance.OnDialogueStateChanged -= OnDialogueStateChanged;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Visibilidad
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Evalúa las condiciones de visibilidad al cargar la escena.
    /// Si alguna no se cumple, el objeto se desactiva completamente.
    /// </summary>
    private void EvaluateVisibility()
    {
        // 1. Evaluar condiciones normales de visibilidad
        if (visibilityConditions != null && visibilityConditions.Count > 0)
        {
            foreach (var condition in visibilityConditions)
            {
                if (!condition.IsMet())
                {
                    gameObject.SetActive(false);
                    Debug.Log($"[AdvancedInteractableObject] '{gameObject.name}' desactivado por condición de visibilidad inicial.");
                    return;
                }
            }
        }

        // 2. Si el objeto tiene configurado desaparecer después de un diálogo
        // y esa condición DE DESAPARICIÓN ya es verdadera al cargar la escena,
        // significa que ya había desaparecido en una sesión/visita anterior.
        if (disappearAfterDialogue && disappearCondition != null && disappearCondition.IsMet())
        {
            gameObject.SetActive(false);
            Debug.Log($"[AdvancedInteractableObject] '{gameObject.name}' desactivado permanentemente porque su condición de desaparición ya se cumplió en el pasado.");
            return;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Desaparición con fade
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Callback al evento OnDialogueStateChanged del StoryManager.
    /// Solo reacciona cuando el diálogo TERMINA (isActive = false).
    /// </summary>
    private void OnDialogueStateChanged(bool isActive)
    {
        // Solo actuamos cuando el diálogo termina
        if (isActive) return;

        // Si el objeto ya está desactivado, no debe procesar desapariciones ni fundidos
        if (!gameObject.activeInHierarchy) return;

        // Verificar la condición de desaparición
        if (!disappearCondition.IsMet()) return;

        // Desuscribirse para no repetir la lógica en futuros diálogos
        if (StoryManager.Instance != null)
        {
            StoryManager.Instance.OnDialogueStateChanged -= OnDialogueStateChanged;
        }

        // Ejecutar la corrutina en el LevelManager (o GameManager)
        // porque si el objeto se desactiva a sí mismo, mata sus propias corrutinas.
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.StartCoroutine(DisappearWithFade());
        }
        else
        {
            StartCoroutine(DisappearWithFade());
        }
    }

    /// <summary>
    /// Fade a negro → desactiva el objeto → fade de vuelta a claro.
    /// Usa el LevelManager para el fade si está disponible.
    /// </summary>
    private IEnumerator DisappearWithFade()
    {
        if (LevelManager.Instance != null)
        {
            // Usar la duración real del LevelManager, o la override si fue configurada
            float duration = disappearFadeDuration > 0
                ? disappearFadeDuration
                : LevelManager.Instance.FadeDuration;

            // 1. Fade a negro
            LevelManager.Instance.FadeToBlack(duration);

            // 2. Esperar a que el fade a negro termine completamente (+buffer mínimo)
            yield return new WaitForSeconds(duration + 0.05f);

            // 3. Desactivar el objeto mientras la pantalla está en negro
            gameObject.SetActive(false);
            Debug.Log($"[AdvancedInteractableObject] '{gameObject.name}' desapareció con fade.");

            // 4. Pequeña pausa para que el estado sea estable
            yield return new WaitForSeconds(0.1f);

            // 5. Volver a claro
            LevelManager.Instance.FadeToClear(duration);
        }
        else
        {
            // Sin LevelManager: desaparecer directamente
            gameObject.SetActive(false);
            Debug.LogWarning($"[AdvancedInteractableObject] '{gameObject.name}' desapareció sin fade (LevelManager no encontrado).");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // IInteractable — sin cambios respecto a versión anterior
    // ─────────────────────────────────────────────────────────────────────────

    public string GetInteractionName()
    {
        return data != null ? data.interactionName : gameObject.name;
    }

    public void Interact()
    {
        if (data == null)
        {
            Debug.LogWarning("[AdvancedInteractableObject] Interactable sin data asignada en: " + gameObject.name);
            return;
        }

        string knot = data.GetValidKnot();

        if (string.IsNullOrEmpty(knot))
        {
            Debug.Log($"[AdvancedInteractableObject] Ninguna interacción válida en {data.interactionName}");
            return;
        }

        Debug.Log("[AdvancedInteractableObject] Interact() ejecutado. Knot: '" + knot + "'");

        // Rotar hacia el jugador para que se sienta como una conversación real
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0;
            if (directionToPlayer.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }

        if (StoryManager.Instance == null)
        {
            Debug.LogError("[AdvancedInteractableObject] StoryManager.Instance es NULL. ¿Está en la escena?");
            return;
        }

        StoryManager.Instance.StartStory(knot);

        // Reproducir sonido de interacción
        if (interactionSound != null)
        {
            interactionSound.PlaySFX();
        }
    }
}
