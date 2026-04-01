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
            // Rotar suavemente hacia el objeto con el que vamos a interactuar
            Component targetComponent = currentInteractable as Component;
            if (targetComponent != null)
            {
                Vector3 directionToTarget = targetComponent.transform.position - transform.position;
                directionToTarget.y = 0;

                if (directionToTarget.sqrMagnitude > 0.001f)
                {
                    // Nota: Aquí podrías usar una Corrutina para un Slerp gradual, 
                    // pero un LookRotation directo es preferible a que el jugador le dé la espalda.
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                }
            }

            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Objeto cercano: " + interactable.GetInteractionName());
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