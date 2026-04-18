using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIAjustes : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panelAjustes;
    public Slider sliderMusica;
    public Slider sliderSFX;
    public Slider sliderUI;

    void Start()
    {
        // Inicializar sliders con los valores actuales del AudioManager
        if (AudioManager.Instance != null)
        {
            if (sliderMusica != null) sliderMusica.value = AudioManager.Instance.musicVolume;
            if (sliderSFX != null) sliderSFX.value = AudioManager.Instance.sfxVolume;
            if (sliderUI != null) sliderUI.value = AudioManager.Instance.uiVolume;

            // Añadir listeners para actualizar el AudioManager cuando cambien los sliders
            if (sliderMusica != null) sliderMusica.onValueChanged.AddListener(SetMusica);
            if (sliderSFX != null) sliderSFX.onValueChanged.AddListener(SetSFX);
            if (sliderUI != null) sliderUI.onValueChanged.AddListener(SetUI);
        }
    }

    void Update()
    {
        // El usuario configurará la tecla 'Q' en el Input Actions.
        // Mientras tanto, mantenemos el cierre con el botón del Gamepad si se desea.
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
        
        // Pausamos el juego cuando el panel está abierto
        Time.timeScale = estado ? 0f : 1f;

        // Si se abre el panel, sincronizamos los sliders de nuevo por si cambiaron externamente
        if (estado && AudioManager.Instance != null)
        {
            if (sliderMusica != null) sliderMusica.value = AudioManager.Instance.musicVolume;
            if (sliderSFX != null) sliderSFX.value = AudioManager.Instance.sfxVolume;
            if (sliderUI != null) sliderUI.value = AudioManager.Instance.uiVolume;
        }
    }

    public void CerrarAjustes()
    {
        panelAjustes.SetActive(false);
        Time.timeScale = 1f;
    }

    // Métodos para los sliders
    public void SetMusica(float val)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(val);
    }

    public void SetSFX(float val)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetSFXVolume(val);
    }

    public void SetUI(float val)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetUIVolume(val);
    }
}
