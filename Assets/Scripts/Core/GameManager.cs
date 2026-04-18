using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Núcleo del sistema. Singleton persistente que orquesta todos los subsistemas.
///
/// RESPONSABILIDADES:
///   - Mantener la referencia al GameStateSO (estado en runtime)
///   - Centralizar el guardado y la carga (delegando E/S a SaveSystem)
///   - Decidir el flujo al iniciar un nivel (¿hay save? → carga, si no → nueva partida)
///   - Exponer API de narrativa para que StoryManager y otros sistemas la usen
///
/// LO QUE NO HACE:
///   - No implementa lógica de escenas (eso es LevelManager)
///   - No implementa lógica narrativa (eso es StoryManager)
///   - No escribe directamente a disco (eso es SaveSystem)
///   - No accede a PlayerPrefs
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global State References")]
    [SerializeField] private GameStateSO gameState;

    [Header("Debugging")]
    [Tooltip("Si está activado, borrará el save en disco y el estado en memoria al dar Play.")]
    [SerializeField] private bool clearStateOnStart = false;

    [Header("Auto-Reset Settings")]
    [Tooltip("Si este flag está presente en el guardado al iniciar la app, la partida se borrará automáticamente (ideal para obligar a un New Game tras el final).")]
    [SerializeField] private string completionFlag = "prologue_final_seen";

    public string CompletionFlag => completionFlag;

    // ─────────────────────────────────────────────────────────────────────────
    // Inicialización
    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFromDisk();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Al arrancar, aplica el modo debug si corresponde, y precarGary el save de disco
    /// en el GameStateSO para que esté disponible en runtime antes de que cualquier
    /// escena de juego lo necesite.
    /// </summary>
    private void InitializeFromDisk()
    {
        if (clearStateOnStart)
        {
            ResetGameState();
            return;
        }

        // Pre-cargar el save en memoria para que esté disponible de inmediato.
        // El flujo de escena se decide en RequestLoadLevel(), no aquí.
        GameSaveData savedData = SaveSystem.Load();
        if (savedData != null)
        {
            gameState.LoadFrom(savedData);

            // Verificación de partida finalizada:
            if (!string.IsNullOrEmpty(completionFlag) && gameState.HasFlag(completionFlag))
            {
                ResetGameState();
                return;
            }

            Debug.Log($"[GameManager] Save cargado en memoria. Última escena: '{gameState.currentSceneName}'");
        }
        else
        {
            gameState.ClearState();
        }
    }

    /// <summary>
    /// Limpia el estado en memoria y elimina el archivo físico.
    /// </summary>
    private void ResetGameState()
    {
        Debug.Log($"[GameManager] Reiniciando estado y borrando save de disco.");
        gameState.ClearState();
        SaveSystem.DeleteSave();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Flujo de niveles (punto de entrada desde CarruselNiveles)
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Punto de entrada unificado desde el menú de selección de niveles.
    ///
    /// Lógica:
    ///   - Si existe un save válido → restaura el GameState y navega a la última escena jugada.
    ///   - Si NO existe save → limpia el estado y carga la escena base indicada (partida nueva).
    ///
    /// Esto reemplaza cualquier lógica de "Continuar / Nueva Partida" en la UI.
    /// La decisión es automática y transparente para el jugador.
    /// </summary>
    /// <param name="baseSceneName">La escena de inicio del nivel (ej. "Parque").</param>
    public void RequestLoadLevel(string baseSceneName)
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogError("[GameManager] RequestLoadLevel: LevelManager no encontrado.");
            return;
        }

        // 1. COMPROBACIÓN DINÁMICA: Si entramos aquí y el flag de final ya está activo, limpiamos.
        // Esto cubre el caso donde el jugador termina la partida y vuelve al menú sin cerrar la app.
        if (!string.IsNullOrEmpty(completionFlag) && gameState.HasFlag(completionFlag))
        {
            Debug.Log($"[GameManager] Detectada partida finalizada con flag '{completionFlag}'. Forzando reset.");
            ResetGameState();
        }

        // 2. COMPROBACIÓN DE ESCENA: Si no hay save o la última escena fue el "Menu", iniciamos limpia.
        if (SaveSystem.HasSave() && 
            !string.IsNullOrEmpty(gameState.currentSceneName) && 
            gameState.currentSceneName != "Menu")
        {
            string targetScene = gameState.currentSceneName;
            Debug.Log($"[GameManager] Reanudando partida en: '{targetScene}'");
            LevelManager.Instance.ChangeScene(targetScene);
        }
        else
        {
            // Partida nueva o reanudación inválida
            ResetGameState();
            Debug.Log($"[GameManager] Iniciando nueva partida en: '{baseSceneName}'");
            LevelManager.Instance.ChangeScene(baseSceneName);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Guardado (llamado por LevelManager en cada transición y al cerrar el juego)
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Toma un snapshot del GameStateSO actual y lo persiste en disco.
    /// Solo debe invocarse desde LevelManager (auto-save) o al cerrar la aplicación.
    /// </summary>
    public void SaveGame()
    {
        if (gameState == null)
        {
            Debug.LogWarning("[GameManager] SaveGame: no hay GameStateSO asignado.");
            return;
        }

        GameSaveData data = new GameSaveData
        {
            lastSceneName     = gameState.currentSceneName,
            previousSceneName = gameState.previousSceneName,
            unlockedFlags     = gameState.GetFlags(),
        };

        // Convertir variables del SO al formato serializable
        List<GameStateSO.StoryVariable> vars = gameState.GetVariables();
        foreach (GameStateSO.StoryVariable sv in vars)
        {
            data.storyVariables.Add(new StoryVariableData { key = sv.key, value = sv.value });
        }

        SaveSystem.Save(data);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Ciclo de vida de la aplicación
    // ─────────────────────────────────────────────────────────────────────────

    private void OnApplicationQuit()
    {
        // Guardado de seguridad al cerrar el juego.
        // Garantiza que si el jugador cierra la app sin cambiar de escena, no pierde progreso.
        if (gameState != null && !string.IsNullOrEmpty(gameState.currentSceneName))
        {
            SaveGame();
            Debug.Log("[GameManager] Guardado de seguridad al cerrar la aplicación.");
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        // En móvil/consola, la app puede suspenderse sin llamar OnApplicationQuit.
        if (pauseStatus && gameState != null && !string.IsNullOrEmpty(gameState.currentSceneName))
        {
            SaveGame();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // API Pública — Estado y narrativa
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Devuelve la referencia al GameStateSO de runtime.</summary>
    public GameStateSO GetGameState()
    {
        return gameState;
    }

    /// <summary>
    /// Guarda una decisión o evento narrativo (flag booleano).
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
    /// Consulta si una decisión o evento narrativo ya ocurrió.
    /// </summary>
    public bool GetStoryFlag(string flagName)
    {
        if (gameState != null) return gameState.HasFlag(flagName);
        return false;
    }

    /// <summary>
    /// Registra una variable exclusiva con un valor específico.
    /// (Ejemplo: key="trato_joseph", value="motivado").
    /// </summary>
    public void SetStoryVariable(string key, string value)
    {
        if (gameState != null) gameState.SetVariable(key, value);
    }

    /// <summary>
    /// Consulta el valor de una variable narrativa importante.
    /// </summary>
    public string GetStoryVariable(string key)
    {
        if (gameState != null) return gameState.GetVariable(key);
        return string.Empty;
    }
}
