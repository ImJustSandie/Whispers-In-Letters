using UnityEngine;

/// <summary>
/// Script independiente para gestionar el regreso al menú principal.
/// Se puede colocar en cualquier botón de la UI.
/// </summary>
public class BotonSalirMenu : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("El nombre exacto de la escena del menú principal.")]
    [SerializeField] private string nombreEscenaMenu = "Menu";

    /// <summary>
    /// Método público para ser llamado desde un evento OnClick de un Button.
    /// </summary>
    public void IrAlMenuPrincipal()
    {
        // 1. Guardamos la partida antes de salir para asegurar que no se pierda nada.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            
            // 2. Cerramos el panel de ajustes para limpiar la UI.
            if (GameManager.Instance.uiAjustes != null)
            {
                GameManager.Instance.uiAjustes.CerrarAjustes();
            }
        }

        // 2. Reanudamos el tiempo (por si CerrarAjustes no lo hizo o no se encontró).
        Time.timeScale = 1f;

        // 2. Usamos el LevelManager para una transición fluida con fade.
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ChangeScene(nombreEscenaMenu);
        }
        else
        {
            // Fallback por si el LevelManager no está en la escena (aunque debería ser Singleton persistente)
            UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscenaMenu);
        }
    }
}
