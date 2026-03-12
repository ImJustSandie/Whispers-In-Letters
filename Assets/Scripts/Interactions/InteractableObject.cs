using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public InteractableData data;

    public void Interact()
    {
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
    }
}