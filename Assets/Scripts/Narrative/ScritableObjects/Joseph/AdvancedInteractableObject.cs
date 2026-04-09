using UnityEngine;

public class AdvancedInteractableObject : MonoBehaviour, IInteractable
{
    public AdvancedInteractableData data;

    [Header("Audio")]
    [Tooltip("Audio que se reproduce al interactuar exitosamente")]
    public AudioEvent interactionSound;

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

        // Bonus: Rotar hacia el jugador para que se sienta como una conversación real
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
