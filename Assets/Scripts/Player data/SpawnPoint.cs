using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("El nombre de la escena desde la que el jugador debe venir para spawnear aquí.")]
    public string fromSceneName;

    // Se dibuja un indicador en verde en el editor de Unity (Gizmo)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        // Dibuja hacia donce apuntaría la vista/cuerpo
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1f);
    }
}
