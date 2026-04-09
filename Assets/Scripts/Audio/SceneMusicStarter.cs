using UnityEngine;

public class SceneMusicStarter : MonoBehaviour
{
    [Tooltip("El evento de audio que contiene la música para esta escena")]
    public AudioEvent sceneMusic;
    
    [Tooltip("¿Reproducir automáticamente al iniciar la escena?")]
    public bool playOnStart = true;

    private void Start()
    {
        if (playOnStart && sceneMusic != null)
        {
            sceneMusic.PlayMusic();
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
}
