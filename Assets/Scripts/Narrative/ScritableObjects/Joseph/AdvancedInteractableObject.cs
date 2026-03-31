using UnityEngine;

public class AdvancedInteractableObject : MonoBehaviour, IInteractable
{
    public AdvancedInteractableData data;

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

        if (StoryManager.Instance == null)
        {
            Debug.LogError("[AdvancedInteractableObject] StoryManager.Instance es NULL. ¿Está en la escena?");
            return;
        }

        StoryManager.Instance.StartStory(knot);
    }
}
