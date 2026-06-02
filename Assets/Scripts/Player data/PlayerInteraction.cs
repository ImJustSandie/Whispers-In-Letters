
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>Se dispara cuando el interactuable cercano cambia (incluye null al salir).</summary>
    public event Action<IInteractable> OnInteractableChanged;

    private PlayerControls controls;
    private IInteractable currentInteractable;

    public static PlayerInteraction Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                }
            }

            currentInteractable.Interact();
        }
        else
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;


            OnInteractableChanged?.Invoke(currentInteractable);
        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {

        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;

            OnInteractableChanged?.Invoke(null);
        }
    }
}