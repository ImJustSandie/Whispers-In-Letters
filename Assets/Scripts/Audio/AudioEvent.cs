using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Event", menuName = "Audio/Audio Event")]
public class AudioEvent : ScriptableObject
{
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 1f;
    
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    
    [Tooltip("¿Se debe repetir en bucle? (Usualmente para música de fondo)")]
    public bool loop = false;

    /// <summary>
    /// Reproduce este evento a través del canal de Efectos de Sonido (SFX).
    /// Ideal para reproducir desde eventos de Unity (UnityEvents) en la UI o interacciones.
    /// </summary>
    public void PlaySFX()
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlaySFX(this);
        }
    }

    /// <summary>
    /// Reproduce este evento a través del canal de UI.
    /// </summary>
    public void PlayUI()
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlayUI(this);
        }
    }

    /// <summary>
    /// Reproduce este evento a través del canal de Música (BGM).
    /// </summary>
    public void PlayMusic()
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlayMusic(this);
        }
    }

    /// <summary>
    /// Reproduce este evento a través del canal de Sonido Ambiente.
    /// Se reproduce en loop automáticamente.
    /// </summary>
    public void PlayAmbience()
    {
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlayAmbience(this);
        }
    }
}
