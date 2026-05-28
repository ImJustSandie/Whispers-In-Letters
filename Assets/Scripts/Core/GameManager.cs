using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Núcleo del sistema. Singleton persistente que orquesta todos los subsistemas.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global State References")]
    [SerializeField] private GameStateSO gameState;

    [Header("UI References")]
    public UIAjustes uiAjustes;
    [Tooltip("Objetos de la UI (como el HUD o el botón de ajustes) que deben ocultarse en el Menú.")]
    public GameObject hudObjects;

    [Header("Debugging")]
    [Tooltip("Si está activado, borrará el save en disco y el estado en memoria al dar Play.")]
    [SerializeField] private bool clearStateOnStart = false;

    [Header("Auto-Reset Settings")]
    [Tooltip("Si este flag está presente en el guardado al iniciar la app, la partida se borrará automáticamente.")]
    [SerializeField] private string completionFlag = "Final_Del_Dia";

    public string CompletionFlag => completionFlag;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFromDisk();
            
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            // Verificación inicial de la UI al arrancar
            UpdateUIVisibility(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUIVisibility(scene.name);
    }

    /// <summary>
    /// Activa o desactiva los elementos de HUD según la escena.
    /// </summary>
    private void UpdateUIVisibility(string sceneName)
    {
        if (hudObjects != null)
        {
            // Ocultar si estamos en el menú, mostrar en cualquier otra escena
            bool isMenu = sceneName == "Menu";
            hudObjects.SetActive(!isMenu);

        }
    }

    private void Update()
    {
        // El usuario configurará el Input Action, pero por ahora detectamos la tecla Q directamente
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (uiAjustes != null && SceneManager.GetActiveScene().name != "Menu")
            {
                uiAjustes.ToggleAjustes();
            }
        }
    }

    private void InitializeFromDisk()
    {
        if (clearStateOnStart)
        {
            ResetGameState();
            return;
        }

        GameSaveData savedData = SaveSystem.Load();
        if (savedData != null)
        {
            gameState.LoadFrom(savedData);

            if (!string.IsNullOrEmpty(completionFlag) && gameState.HasFlag(completionFlag))
            {
                ResetGameState();
                return;
            }


        }
        else
        {
            gameState.ClearState();
        }
    }

    private void ResetGameState()
    {

        gameState.ClearState();
        SaveSystem.DeleteSave();
    }

    public void RequestLoadLevel(string baseSceneName)
    {
        if (LevelManager.Instance == null)
        {

            return;
        }

        if (!string.IsNullOrEmpty(completionFlag) && gameState.HasFlag(completionFlag))
        {
            ResetGameState();
        }

        if (SaveSystem.HasSave() && 
            !string.IsNullOrEmpty(gameState.currentSceneName) && 
            gameState.currentSceneName != "Menu")
        {
            string targetScene = gameState.currentSceneName;

            LevelManager.Instance.ChangeScene(targetScene);
        }
        else
        {
            ResetGameState();

            LevelManager.Instance.ChangeScene(baseSceneName);
        }
    }

    public void SaveGame()
    {
        if (gameState == null) return;

        GameSaveData data = new GameSaveData
        {
            lastSceneName     = gameState.currentSceneName,
            previousSceneName = gameState.previousSceneName,
            unlockedFlags     = gameState.GetFlags(),
        };

        List<GameStateSO.StoryVariable> vars = gameState.GetVariables();
        foreach (GameStateSO.StoryVariable sv in vars)
        {
            data.storyVariables.Add(new StoryVariableData { key = sv.key, value = sv.value });
        }

        SaveSystem.Save(data);
    }

    private void OnApplicationQuit()
    {
        if (gameState != null && !string.IsNullOrEmpty(gameState.currentSceneName))
        {
            SaveGame();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && gameState != null && !string.IsNullOrEmpty(gameState.currentSceneName))
        {
            SaveGame();
        }
    }

    public GameStateSO GetGameState() => gameState;

    public void SetStoryFlag(string flagName, bool value)
    {
        if (gameState != null) gameState.SetFlag(flagName, value);
    }

    public bool GetStoryFlag(string flagName)
    {
        if (gameState != null) return gameState.HasFlag(flagName);
        return false;
    }

    public void SetStoryVariable(string key, string value)
    {
        if (gameState != null) gameState.SetVariable(key, value);
    }

    public string GetStoryVariable(string key)
    {
        if (gameState != null) return gameState.GetVariable(key);
        return string.Empty;
    }

    public int IncrementStoryVariable(string key, int amount = 1)
    {
        string current = GetStoryVariable(key);
        int value = 0;
        if (!string.IsNullOrEmpty(current)) int.TryParse(current, out value);
        value += amount;
        SetStoryVariable(key, value.ToString());
        return value;
    }

    public int GetStoryVariableAsInt(string key)
    {
        string val = GetStoryVariable(key);
        int result = 0;
        if (!string.IsNullOrEmpty(val)) int.TryParse(val, out result);
        return result;
    }
}
