
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>Se dispara cuando el interactuable cercano cambia (incluye null al salir).</summary>
    public event Action<IInteractable> OnInteractableChanged;

    private PlayerControls controls;
    private IInteractable currentInteractable;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryInteract();

        // Modo skip de dialogo (DEBUG)
        controls.Player.SaltarTexto.started += ctx => SetSkip(true);
        controls.Player.SaltarTexto.canceled += ctx => SetSkip(false);
    }

    private void SetSkip(bool skip)
    {
        if (StoryManager.Instance != null && StoryManager.Instance.IsDialogueActive)
        {
            StoryManager.Instance.SetSkipMode(skip);
        }
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
        Debug.Log("[PlayerInteraction] Intentando interactuar. DialogueActive: " + (StoryManager.Instance != null && StoryManager.Instance.IsDialogueActive));

        if (StoryManager.Instance != null && StoryManager.Instance.IsDialogueActive)
        {
            StoryManager.Instance.AdvanceStory();
            return;
        }

        if (currentInteractable != null)
        {
            Debug.Log("[PlayerInteraction] Ejecutando Interact() en: " + currentInteractable.GetInteractionName());

            // Rotar suavemente hacia el objeto con el que vamos a interactuar
            Component targetComponent = currentInteractable as Component;
            if (targetComponent != null)
            {
                Vector3 directionToTarget = targetComponent.transform.position - transform.position;
                directionToTarget.y = 0;

                if (directionToTarget.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                }
            }

            currentInteractable.Interact();
        }
        else
        {
            Debug.LogWarning("[PlayerInteraction] currentInteractable es NULL.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[PlayerInteraction] OnTriggerEnter con: " + other.gameObject.name);
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("Objeto cercano: " + interactable.GetInteractionName());
            Debug.Log("[PlayerInteraction] Invocando OnInteractableChanged. Suscriptores: " + (OnInteractableChanged?.GetInvocationList().Length ?? 0));
            OnInteractableChanged?.Invoke(currentInteractable);
        }
        else
        {
            Debug.Log("[PlayerInteraction] OnTriggerEnter: el objeto NO implementa IInteractable.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("[PlayerInteraction] OnTriggerExit con: " + other.gameObject.name);
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            Debug.Log("[PlayerInteraction] Invocando OnInteractableChanged(null). Suscriptores: " + (OnInteractableChanged?.GetInvocationList().Length ?? 0));
            OnInteractableChanged?.Invoke(null);
        }
    }
}