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
    public Slider sliderAmbiente;
    public Slider sliderMaster;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (sliderMusica != null) sliderMusica.value = AudioManager.Instance.MusicVolume;
            if (sliderSFX != null) sliderSFX.value = AudioManager.Instance.SFXVolume;
            if (sliderUI != null) sliderUI.value = AudioManager.Instance.UIVolume;
            if (sliderAmbiente != null) sliderAmbiente.value = AudioManager.Instance.AmbienceVolume;
            if (sliderMaster != null) sliderMaster.value = AudioManager.Instance.MasterVolume;

            if (sliderMusica != null) sliderMusica.onValueChanged.AddListener(SetMusica);
            if (sliderSFX != null) sliderSFX.onValueChanged.AddListener(SetSFX);
            if (sliderUI != null) sliderUI.onValueChanged.AddListener(SetUI);
            if (sliderAmbiente != null) sliderAmbiente.onValueChanged.AddListener(SetAmbiente);
            if (sliderMaster != null) sliderMaster.onValueChanged.AddListener(SetMaster);
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

        if (estado && AudioManager.Instance != null)
        {
            if (sliderMusica != null) sliderMusica.value = AudioManager.Instance.MusicVolume;
            if (sliderSFX != null) sliderSFX.value = AudioManager.Instance.SFXVolume;
            if (sliderUI != null) sliderUI.value = AudioManager.Instance.UIVolume;
            if (sliderAmbiente != null) sliderAmbiente.value = AudioManager.Instance.AmbienceVolume;
            if (sliderMaster != null) sliderMaster.value = AudioManager.Instance.MasterVolume;
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

    public void SetAmbiente(float val)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetAmbienceVolume(val);
    }

    public void SetMaster(float val)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetMasterVolume(val);
    }
}
