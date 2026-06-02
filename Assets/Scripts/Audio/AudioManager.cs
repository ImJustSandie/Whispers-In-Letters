using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource MusicAudioSource;
    public AudioSource UIAudioSource;
    public AudioSource SFXAudioSource;
    public AudioSource AmbienceAudioSource;

    [Header("Volume Configuration (Defaults)")]
    [Range(0f, 1f)] [SerializeField] private float musicVolumeConfig = 1f;
    [Range(0f, 1f)] [SerializeField] private float sfxVolumeConfig = 1f;
    [Range(0f, 1f)] [SerializeField] private float uiVolumeConfig = 1f;
    [Range(0f, 1f)] [SerializeField] private float ambienceVolumeConfig = 1f;
    [Range(0f, 1f)] [SerializeField] private float masterVolumeConfig = 1f;

    // Runtime volume state (loaded from/saved to PlayerPrefs)
    private float _currentMusicVolume;
    private float _currentSFXVolume;
    private float _currentUIVolume;
    private float _currentAmbienceVolume;
    private float _currentMasterVolume;

    // Base AudioSource volumes from Inspector (read once at Awake)
    private float _musicSourceBaseVolume;
    private float _sfxSourceBaseVolume;
    private float _uiSourceBaseVolume;
    private float _ambienceSourceBaseVolume;

    // Currently playing events (for continuous playback recalculation)
    private AudioEvent _currentMusicEvent;
    private AudioEvent _currentAmbienceEvent;

    // Public read-only access for UI
    public float MusicVolume => _currentMusicVolume;
    public float SFXVolume => _currentSFXVolume;
    public float UIVolume => _currentUIVolume;
    public float AmbienceVolume => _currentAmbienceVolume;
    public float MasterVolume => _currentMasterVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        // Load runtime values from PlayerPrefs (use config values as defaults)
        _currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolumeConfig);
        _currentSFXVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolumeConfig);
        _currentUIVolume = PlayerPrefs.GetFloat("UIVolume", uiVolumeConfig);
        _currentAmbienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", ambienceVolumeConfig);
        _currentMasterVolume = PlayerPrefs.GetFloat("MasterVolume", masterVolumeConfig);

        // Store base Source volumes from Inspector
        _musicSourceBaseVolume = MusicAudioSource != null ? MusicAudioSource.volume : 1f;
        _sfxSourceBaseVolume = SFXAudioSource != null ? SFXAudioSource.volume : 1f;
        _uiSourceBaseVolume = UIAudioSource != null ? UIAudioSource.volume : 1f;
        _ambienceSourceBaseVolume = AmbienceAudioSource != null ? AmbienceAudioSource.volume : 1f;

        // Apply initial volumes to AudioSources
        UpdateOneShotSourceVolumes();

        if (MusicAudioSource != null)
            MusicAudioSource.volume = _musicSourceBaseVolume * _currentMusicVolume * _currentMasterVolume;

        if (AmbienceAudioSource != null)
            AmbienceAudioSource.volume = _ambienceSourceBaseVolume * _currentAmbienceVolume * _currentMasterVolume;
    }

    /// <summary>
    /// Calculates final volume as: EventVolume x SourceVolume x CategoryVolume x MasterVolume
    /// </summary>
    private float CalculateVolume(float eventVolume, float sourceVolume, float categoryVolume, float masterVolume)
    {
        return Mathf.Clamp01(eventVolume * sourceVolume * categoryVolume * masterVolume);
    }

    /// <summary>
    /// Updates SFX and UI AudioSource volumes (affects currently-playing one-shots dynamically).
    /// </summary>
    private void UpdateOneShotSourceVolumes()
    {
        if (SFXAudioSource != null)
            SFXAudioSource.volume = _sfxSourceBaseVolume * _currentSFXVolume * _currentMasterVolume;

        if (UIAudioSource != null)
            UIAudioSource.volume = _uiSourceBaseVolume * _currentUIVolume * _currentMasterVolume;
    }

    /// <summary>
    /// Updates all AudioSource volumes after category or master changes.
    /// </summary>
    private void UpdateAllVolumes()
    {
        UpdateOneShotSourceVolumes();

        if (_currentMusicEvent != null)
        {
            MusicAudioSource.volume = CalculateVolume(
                _currentMusicEvent.volume, _musicSourceBaseVolume, _currentMusicVolume, _currentMasterVolume);
        }

        if (_currentAmbienceEvent != null)
        {
            AmbienceAudioSource.volume = CalculateVolume(
                _currentAmbienceEvent.volume, _ambienceSourceBaseVolume, _currentAmbienceVolume, _currentMasterVolume);
        }
    }

    public void PlaySFX(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null)
        {

            return;
        }

        SFXAudioSource.pitch = audioEvent.pitch;
        SFXAudioSource.PlayOneShot(audioEvent.clip, audioEvent.volume);
    }

    public void PlayUI(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;

        UIAudioSource.pitch = audioEvent.pitch;
        UIAudioSource.PlayOneShot(audioEvent.clip, audioEvent.volume);
    }

    public void PlayMusic(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;

        if (MusicAudioSource.clip == audioEvent.clip && MusicAudioSource.isPlaying)
            return;

        _currentMusicEvent = audioEvent;
        MusicAudioSource.clip = audioEvent.clip;
        MusicAudioSource.volume = CalculateVolume(audioEvent.volume, _musicSourceBaseVolume, _currentMusicVolume, _currentMasterVolume);
        MusicAudioSource.pitch = audioEvent.pitch;
        MusicAudioSource.loop = audioEvent.loop;
        MusicAudioSource.Play();
    }

    public void StopMusic()
    {
        _currentMusicEvent = null;
        MusicAudioSource.Stop();
    }

    public void PlayAmbience(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null) return;

        if (AmbienceAudioSource.clip == audioEvent.clip && AmbienceAudioSource.isPlaying)
            return;

        _currentAmbienceEvent = audioEvent;
        AmbienceAudioSource.clip = audioEvent.clip;
        AmbienceAudioSource.volume = CalculateVolume(audioEvent.volume, _ambienceSourceBaseVolume, _currentAmbienceVolume, _currentMasterVolume);
        AmbienceAudioSource.pitch = audioEvent.pitch;
        AmbienceAudioSource.loop = true;
        AmbienceAudioSource.Play();
    }

    public void StopAmbience()
    {
        _currentAmbienceEvent = null;
        AmbienceAudioSource.Stop();
    }

    // ==== VOLUME SETTERS (Runtime + Persistence) ====

    public void SetMusicVolume(float volume)
    {
        _currentMusicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MusicVolume", _currentMusicVolume);

        if (_currentMusicEvent != null)
        {
            MusicAudioSource.volume = CalculateVolume(
                _currentMusicEvent.volume, _musicSourceBaseVolume, _currentMusicVolume, _currentMasterVolume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        _currentSFXVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("SFXVolume", _currentSFXVolume);
        UpdateOneShotSourceVolumes();
    }

    public void SetUIVolume(float volume)
    {
        _currentUIVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("UIVolume", _currentUIVolume);
        UpdateOneShotSourceVolumes();
    }

    public void SetAmbienceVolume(float volume)
    {
        _currentAmbienceVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("AmbienceVolume", _currentAmbienceVolume);

        if (_currentAmbienceEvent != null)
        {
            AmbienceAudioSource.volume = CalculateVolume(
                _currentAmbienceEvent.volume, _ambienceSourceBaseVolume, _currentAmbienceVolume, _currentMasterVolume);
        }
    }

    public void SetMasterVolume(float volume)
    {
        _currentMasterVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("MasterVolume", _currentMasterVolume);
        UpdateAllVolumes();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (_currentMusicEvent != null)
            {
                MusicAudioSource.volume = CalculateVolume(
                    _currentMusicEvent.volume, _musicSourceBaseVolume, _currentMusicVolume, _currentMasterVolume);
            }

            if (_currentAmbienceEvent != null)
            {
                AmbienceAudioSource.volume = CalculateVolume(
                    _currentAmbienceEvent.volume, _ambienceSourceBaseVolume, _currentAmbienceVolume, _currentMasterVolume);
            }
        }
    }
#endif
}
