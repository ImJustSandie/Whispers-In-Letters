using UnityEngine;

/// <summary>
/// Conecta el evento <see cref="PlayerInteraction.OnInteractableChanged"/> con
/// <see cref="InteractionIndicator.SetInteractable"/>.
///
/// Uso:
///   1. Añade este componente en CUALQUIER GameObject de la escena.
///   2. Asigna playerInteraction e indicator desde el Inspector.
/// </summary>
public class InteractionIndicatorConnector : MonoBehaviour
{
    [Tooltip("Referencia a PlayerInteraction (puede estar en otro GameObject).")]
    [SerializeField] private PlayerInteraction playerInteraction;

    [Tooltip("Referencia al InteractionIndicator de la escena.")]
    [SerializeField] private InteractionIndicator indicator;

    private void Awake()
    {

    }

    private void Start()
    {
        if (playerInteraction == null)
        {
            playerInteraction = PlayerInteraction.Instance;
            if (playerInteraction == null)
            {

            }
            else
            {

            }
        }

        if (indicator == null)
        {

            return;
        }

        if (playerInteraction != null)
        {
            playerInteraction.OnInteractableChanged += OnInteractableChanged;

        }
    }

    private void OnInteractableChanged(IInteractable interactable)
    {

        indicator.SetInteractable(interactable);
    }

    private void OnDestroy()
    {
        if (playerInteraction != null)
            playerInteraction.OnInteractableChanged -= OnInteractableChanged;
    }
}
