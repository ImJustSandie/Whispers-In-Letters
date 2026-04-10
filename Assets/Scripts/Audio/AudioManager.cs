using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource MusicAudioSource;
    public AudioSource UIAudioSource;
    public AudioSource SFXAudioSource;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float uiVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Evita que el AudioManager se destruya al cambiar de escenas
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Reproduce un efecto de sonido en general.
    /// </summary>
    public void PlaySFX(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;
        
        SFXAudioSource.pitch = audioEvent.pitch;
        SFXAudioSource.PlayOneShot(audioEvent.clip, audioEvent.volume * sfxVolume);
    }
    
    /// <summary>
    /// Reproduce un sonido de interfaz de usuario.
    /// </summary>
    public void PlayUI(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;

        UIAudioSource.pitch = audioEvent.pitch;
        UIAudioSource.PlayOneShot(audioEvent.clip, audioEvent.volume * uiVolume);
    }
    
    /// <summary>
    /// Reproduce la música de fondo.
    /// </summary>
    public void PlayMusic(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;

        // Evitar reiniciar si ya esta sonando la misma musica
        if (MusicAudioSource.clip == audioEvent.clip && MusicAudioSource.isPlaying)
            return; 

        MusicAudioSource.clip = audioEvent.clip;
        MusicAudioSource.volume = audioEvent.volume * musicVolume;
        MusicAudioSource.pitch = audioEvent.pitch;
        MusicAudioSource.loop = audioEvent.loop;
        MusicAudioSource.Play();
    }

    /// <summary>
    /// Detiene la música actual.
    /// </summary>
    public void StopMusic()
    {
        MusicAudioSource.Stop();
    }
}
