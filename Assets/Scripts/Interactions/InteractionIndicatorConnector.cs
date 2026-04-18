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
        Debug.Log("[Connector] Awake ejecutado en: " + gameObject.name);
    }

    private void Start()
    {
        if (playerInteraction == null)
        {
            Debug.LogError("[Connector] 'playerInteraction' no está asignado en el Inspector. Arrástralo desde la jerarquía.");
            return;
        }

        if (indicator == null)
        {
            Debug.LogError("[Connector] 'indicator' no está asignado en el Inspector. Arrástralo desde la jerarquía.");
            return;
        }

        playerInteraction.OnInteractableChanged += OnInteractableChanged;
        Debug.Log("[Connector] Suscripción registrada correctamente en: " + playerInteraction.gameObject.name);
    }

    private void OnInteractableChanged(IInteractable interactable)
    {
        Debug.Log($"[Connector] Evento recibido → {(interactable != null ? interactable.GetInteractionName() : "null")}");
        indicator.SetInteractable(interactable);
    }

    private void OnDestroy()
    {
        if (playerInteraction != null)
            playerInteraction.OnInteractableChanged -= OnInteractableChanged;
    }
}
