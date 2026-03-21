using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global State References")]
    [SerializeField] private GameStateSO gameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Garantiza que este administrador nunca se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameStateSO GetGameState()
    {
        return gameState;
    }

    /// <summary>
    /// Guarda una decision o evento narrativo (path).
    /// </summary>
    public void SetStoryFlag(string flagName, bool value)
    {
        if (gameState != null)
        {
            gameState.SetFlag(flagName, value);
        }
        else
        {
            Debug.LogWarning("[GameManager] Intentando guardar un flag pero no hay GameStateSO asignado.");
        }
    }

    /// <summary>
    /// Consulta si una decision o evento narrativo ya ocurrio.
    /// </summary>
    public bool GetStoryFlag(string flagName)
    {
        if (gameState != null)
        {
            return gameState.HasFlag(flagName);
        }
        return false;
    }
}
