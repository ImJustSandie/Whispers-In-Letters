using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Indicador visual de interacción con posición absoluta en world space.
///
/// En Awake desparentea este objeto Y el Canvas del icon para que ninguno
/// herede transformaciones del player.
///
/// En LateUpdate posiciona el icon directamente sobre el interactuable
/// en world space, y rota para mirar siempre a la cámara (billboard).
/// </summary>
public class InteractionIndicator : MonoBehaviour
{
    [Tooltip("Panel/Image que se activa o desactiva. Debe ser hijo de un Canvas World Space.")]
    public GameObject icon;

    [Tooltip("Desplazamiento en world space sobre el interactuable (ej. Y+2).")]
    public Vector3 worldOffset = new Vector3(0f, 2f, 0f);

    private Transform targetTransform;

    // El Canvas raíz del icon (el que hay que mover para reubicar el panel)
    private Transform iconRoot;

    // -------------------------------------------------------------------------
    // Awake: desparentear este GameObject Y el Canvas del icon
    // -------------------------------------------------------------------------

    private void Awake()
    {
        // Desparentar este script de cualquier objeto que lo mueva
        if (transform.parent != null)
        {
            Debug.LogWarning("[InteractionIndicator] Desparentando de: " + transform.parent.name);
            transform.SetParent(null);
        }

        // Encontrar la raíz del icon (Canvas padre más alto o el propio icon)
        if (icon != null)
        {
            Canvas canvas = icon.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                iconRoot = canvas.transform;

                // Si el Canvas tiene padre, desparentarlo para que esté en la raíz del mundo
                if (iconRoot.parent != null)
                {
                    Debug.LogWarning("[InteractionIndicator] Desparentando Canvas del icon de: " + iconRoot.parent.name);
                    iconRoot.SetParent(null);
                }

                // Forzar World Space para que la posición world tenga efecto
                if (canvas.renderMode != RenderMode.WorldSpace)
                {
                    Debug.LogWarning("[InteractionIndicator] Canvas del icon no es World Space. Cambiando a World Space.");
                    canvas.renderMode = RenderMode.WorldSpace;
                }
            }
            else
            {
                // Sin Canvas: usar el transform directo del icon
                iconRoot = icon.transform;
                if (iconRoot.parent != null)
                {
                    Debug.LogWarning("[InteractionIndicator] Desparentando icon de: " + iconRoot.parent.name);
                    iconRoot.SetParent(null);
                }
            }

            icon.SetActive(false);
        }
    }

    // -------------------------------------------------------------------------
    // API pública
    // -------------------------------------------------------------------------

    public void SetInteractable(IInteractable interactable)
    {
        if (icon == null)
        {
            Debug.LogError("[InteractionIndicator] 'icon' es null, asígnalo en el Inspector.");
            return;
        }

        if (interactable != null)
        {
            Component comp = interactable as Component;
            targetTransform = comp != null ? comp.transform : null;

            bool activar = targetTransform != null;
            icon.SetActive(activar);
            Debug.Log($"[InteractionIndicator] SetInteractable → {interactable.GetInteractionName()} | icon.SetActive({activar})");
        }
        else
        {
            targetTransform = null;
            icon.SetActive(false);
            Debug.Log("[InteractionIndicator] SetInteractable → null | icon.SetActive(false)");
        }
    }

    // -------------------------------------------------------------------------
    // LateUpdate: posición world absoluta + billboard
    // -------------------------------------------------------------------------

    private void LateUpdate()
    {
        if (targetTransform == null || icon == null || !icon.activeSelf || iconRoot == null)
            return;

        // Safety: el objeto interactuable se desactivó (ej. PrologueItemInteractable
        // llama a gameObject.SetActive(false) al recogerlo). Unity NO dispara
        // OnTriggerExit en ese caso, así que lo detectamos aquí y ocultamos el icono.
        if (!targetTransform.gameObject.activeInHierarchy)
        {
            targetTransform = null;
            icon.SetActive(false);
            Debug.Log("[InteractionIndicator] Interactuable desactivado → icon ocultado automáticamente.");
            return;
        }

        // 1. Mover directamente la raíz del icon al punto world del interactuable
        iconRoot.position = targetTransform.position + worldOffset;

        // 2. Billboard: el iconRoot siempre mira hacia la cámara
        if (Camera.main != null)
        {
            iconRoot.forward = Camera.main.transform.forward;
        }
    }
}
