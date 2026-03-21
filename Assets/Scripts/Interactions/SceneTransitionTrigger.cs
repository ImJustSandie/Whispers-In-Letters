using UnityEngine;

[RequireComponent(typeof(Collider))] // O Collider2D si estas trabajando en 2D
public class SceneTransitionTrigger : MonoBehaviour
{
    [Tooltip("El nombre exacto de la escena a la que quieres viajar al tocar este objeto.")]
    public string destinationSceneName;

    [Tooltip("Asegurate de que Sophia tenga el 'Tag' correcto configurado en el Inspector.")]
    public string playerTag = "Player";

    private bool isTransitioning = false;

    // Si tu juego es 3D usa OnTriggerEnter, si es 2D usa OnTriggerEnter2D
    private void OnTriggerEnter(Collider other)
    {
        // Revisamos si quien toco la puerta fue realmente el jugador y si no estamos ya cargando
        if (other.CompareTag(playerTag) && !isTransitioning)
        {
            if (LevelManager.Instance != null && !string.IsNullOrEmpty(destinationSceneName))
            {
                isTransitioning = true;
                Debug.Log($"[SceneTransition] Sophia toco la zona. Viajando hacia: {destinationSceneName}");
                LevelManager.Instance.ChangeScene(destinationSceneName);
            }
            else
            {
                Debug.LogWarning("[SceneTransition] No hay un LevelManager instanciado o la escena de destino esta en blanco.");
            }
        }
    }
    
    // Descomenta esto y borra la funcion de arriba si tu juego es estrictamente 2D
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !isTransitioning)
        {
            if (LevelManager.Instance != null && !string.IsNullOrEmpty(destinationSceneName))
            {
                isTransitioning = true;
                LevelManager.Instance.ChangeScene(destinationSceneName);
            }
        }
    }
    */
}
