using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerControls controls;
    private InteractableObject currentInteractable;

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
        Debug.Log("[PlayerInteraction] TryInteract llamado. currentInteractable: " + (currentInteractable != null ? currentInteractable.name : "NULL"));

        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Objeto cercano: " + interactable.data.interactionName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}