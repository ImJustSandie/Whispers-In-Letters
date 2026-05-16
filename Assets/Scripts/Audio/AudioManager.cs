using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource MusicAudioSource;
    public AudioSource UIAudioSource;
    public AudioSource SFXAudioSource;
    public AudioSource AmbienceAudioSource;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float uiVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambienceVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Evita que el AudioManager se destruya al cambiar de escenas
            DontDestroyOnLoad(gameObject);
            LoadVolumes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadVolumes()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        uiVolume = PlayerPrefs.GetFloat("UIVolume", 1f);
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 1f);

        // Aplicar volumen inicial a la fuente de música
        if (MusicAudioSource != null)
        {
            MusicAudioSource.volume = musicVolume;
        }

        // Aplicar volumen inicial a la fuente de sonido ambiente
        if (AmbienceAudioSource != null)
        {
            AmbienceAudioSource.volume = ambienceVolume;
        }
    }

    /// <summary>
    /// Reproduce un efecto de sonido en general.
    /// </summary>
    public void PlaySFX(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null)
        {
            Debug.LogWarning("[AudioManager] Intento de reproducir un SFX con un AudioEvent o Clip nulo.");
            return;
        }
        
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

    /// <summary>
    /// Reproduce el sonido ambiente. Funciona igual que la música: loop y evita reiniciar si ya suena el mismo clip.
    /// </summary>
    public void PlayAmbience(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;

        // Evitar reiniciar si ya esta sonando el mismo sonido ambiente
        if (AmbienceAudioSource.clip == audioEvent.clip && AmbienceAudioSource.isPlaying)
            return;

        AmbienceAudioSource.clip = audioEvent.clip;
        AmbienceAudioSource.volume = audioEvent.volume * ambienceVolume;
        AmbienceAudioSource.pitch = audioEvent.pitch;
        AmbienceAudioSource.loop = true; // Siempre en loop
        AmbienceAudioSource.Play();
    }

    /// <summary>
    /// Detiene el sonido ambiente actual.
    /// </summary>
    public void StopAmbience()
    {
        AmbienceAudioSource.Stop();
    }

    // ==== MÉTODOS DE ACTUALIZACIÓN EN TIEMPO REAL ====

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        MusicAudioSource.volume = musicVolume; 
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void SetUIVolume(float volume)
    {
        uiVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("UIVolume", uiVolume);
    }

    public void SetAmbienceVolume(float volume)
    {
        ambienceVolume = Mathf.Clamp01(volume);
        AmbienceAudioSource.volume = ambienceVolume;
        PlayerPrefs.SetFloat("AmbienceVolume", ambienceVolume);
    }

    // OnValidate se ejecuta automáticamente cada vez que cambias un valor en el Inspector de Unity
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (MusicAudioSource != null && MusicAudioSource.isPlaying)
            {
                // Actualiza en vivo. Nota: Ignora el localVolume del AudioEvent en este refresh rápido
                MusicAudioSource.volume = musicVolume; 
            }

            if (AmbienceAudioSource != null && AmbienceAudioSource.isPlaying)
            {
                AmbienceAudioSource.volume = ambienceVolume;
            }
        }
    }
#endif
}
