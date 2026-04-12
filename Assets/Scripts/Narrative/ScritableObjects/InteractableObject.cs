using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public InteractableData data;

    [Header("Audio")]
    [Tooltip("Audio que se reproduce al interactuar exitosamente")]
    public AudioEvent interactionSound;

    public string GetInteractionName()
    {
        return data != null ? data.interactionName : gameObject.name;
    }

    [Tooltip("Opcional: Si escribes un flag aquí (ej: 'Ruta Terminada'), el objeto o NPC no se podrá interactuar hasta conseguirlo.")]
    public string requiredFlag = "";

    [Tooltip("Opcional: Nudo de Ink que se reproducirá si el jugador NO tiene el flag requerido.")]
    public string fallbackKnot = "";

    [Tooltip("Activa esta casilla para que la interacción empiece automáticamente en cuanto el jugador toque el objeto (Trigger), sin tener que presionar el botón.")]
    public bool autoTriggerOnEnter = false;

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

    public void Interact()
    {
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
}