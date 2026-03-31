using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerControls controls;
    private IInteractable currentInteractable;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryInteract();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void TryInteract()
    {
        if (StoryManager.Instance != null && StoryManager.Instance.IsDialogueActive)
        {
            StoryManager.Instance.AdvanceStory();
            return;
        }

        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Usamos GetComponentInParent por si el collider (hitbox) es un hijo del GameObject que tiene el script.
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Objeto cercano detectado: " + interactable.GetInteractionName());
        }
        else
        {
            Debug.Log("Entró a la hitbox de: " + other.gameObject.name + " , pero no se encontró la interfaz IInteractable.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}