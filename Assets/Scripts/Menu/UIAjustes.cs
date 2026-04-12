using UnityEngine;
using UnityEngine.InputSystem;

public class UIAjustes : MonoBehaviour
{
    public GameObject panelAjustes;

    void Update()
    {
        // Si el panel está activo y se presiona B (botón Este)
        if (panelAjustes.activeSelf && Gamepad.current != null)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                CerrarAjustes();
            }
        }
    }

    public void ToggleAjustes()
    {
        bool estado = !panelAjustes.activeSelf;
        panelAjustes.SetActive(estado);
        Time.timeScale = estado ? 0f : 1f;
    }

    public void CerrarAjustes()
    {
        panelAjustes.SetActive(false);
        Time.timeScale = 1f;
    }
}