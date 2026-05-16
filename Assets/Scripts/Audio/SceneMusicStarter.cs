using UnityEngine;

public class SceneMusicStarter : MonoBehaviour
{
    [Tooltip("El evento de audio que contiene la música para esta escena")]
    public AudioEvent sceneMusic;

    [Tooltip("El evento de audio que contiene el sonido ambiente para esta escena")]
    public AudioEvent sceneAmbience;
    
    [Tooltip("¿Reproducir automáticamente al iniciar la escena?")]
    public bool playOnStart = true;

    private void Start()
    {
        if (playOnStart)
        {
            if (sceneMusic != null)
            {
                sceneMusic.PlayMusic();
            }

            if (sceneAmbience != null)
            {
                sceneAmbience.PlayAmbience();
            }
        }
    }
    
    /// <summary>
    /// Metodo publico útil si se quiere reproducir la musica despues desde un UnityEvent.
    /// </summary>
    public void Play()
    {
        if (sceneMusic != null)
        {
            sceneMusic.PlayMusic();
        }
    }

    /// <summary>
    /// Reproduce el sonido ambiente de la escena desde un UnityEvent.
    /// </summary>
    public void PlayAmbience()
    {
        if (sceneAmbience != null)
        {
            sceneAmbience.PlayAmbience();
        }
    }

    /// <summary>
    /// Detiene tanto la música como el sonido ambiente.
    /// </summary>
    public void StopAll()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.StopAmbience();
        }
    }
}
