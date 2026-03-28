using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global State References")]
    [SerializeField] private GameStateSO gameState;

    [Header("Debugging")]
    [Tooltip("Si esta activado, borrara todas las decisiones y la posicion guardada cada vez que des Play")]
    [SerializeField] private bool clearStateOnStart = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Garantiza que este administrador nunca se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);

            if (clearStateOnStart && gameState != null)
            {
                gameState.ClearState();
                Debug.Log("[GameManager] El progreso ha sido reseteado por configuracion de Debug.");
            }
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

    /// <summary>
    /// Registra una "decisión exclusiva" asignándole un valor especifico a una clave. 
    /// (Ejemplo: key="trato_joseph", value="motivado").
    /// </summary>
    public void SetStoryVariable(string key, string value)
    {
        if (gameState != null)
        {
            gameState.SetVariable(key, value);
        }
    }

    /// <summary>
    /// Intenta consultar qué variable fue escogida para cierta decisión importante.
    /// </summary>
    public string GetStoryVariable(string key)
    {
        if (gameState != null)
        {
            return gameState.GetVariable(key);
        }
        return string.Empty;
    }
}
