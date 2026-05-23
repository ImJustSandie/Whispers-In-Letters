using System.Collections.Generic;
using UnityEngine;

public class GameSummaryManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PhilosopherCardDatabase philosopherDatabase;
    [SerializeField] private CollectableDuckDatabase duckDatabase;
    [SerializeField] private GameSummaryUI summaryUI;

    private void Awake()
    {
        // Asegurarse de que el panel de resumen inicie desactivado
        if (summaryUI != null)
        {
            summaryUI.HideSummary();
        }
    }

    private void Start()
    {
        if (StoryManager.Instance != null)
        {
            StoryManager.Instance.OnDialogueStateChanged += HandleDialogueStateChanged;
        }
        else
        {
            Debug.LogWarning("[GameSummaryManager] StoryManager no encontrado en Start. Escuchando en el futuro...");
        }
    }

    private void OnDestroy()
    {
        if (StoryManager.Instance != null)
        {
            StoryManager.Instance.OnDialogueStateChanged -= HandleDialogueStateChanged;
        }
    }

    private void HandleDialogueStateChanged(bool isDialogueActive)
    {
        // Solo nos interesa cuando el diálogo se cierra
        if (isDialogueActive) return;

        if (GameManager.Instance != null && GameManager.Instance.GetStoryFlag("Final_Del_Dia"))
        {
            Debug.Log("[GameSummaryManager] Detectado Final_Del_Dia. Mostrando resumen.");
            ShowGameSummary();
            
            // Removemos el flag para que no salte el panel repetidamente al cambiar de escena 
            // (aunque el ChangeScene destruirá la escena actual, es buena práctica)
            GameManager.Instance.SetStoryFlag("Final_Del_Dia", false);
        }
    }

    private void ShowGameSummary()
    {
        if (summaryUI == null)
        {
            Debug.LogError("[GameSummaryManager] GameSummaryUI no asignado.");
            return;
        }

        string endingName = "Desconocido";
        string chosenPath = "Ninguna";

        // 1. Determinar el Filósofo (Final)
        if (GameManager.Instance != null)
        {
            string acceptKnot = GameManager.Instance.GetStoryVariable("carta_aceptacion_ruta");
            if (!string.IsNullOrEmpty(acceptKnot) && philosopherDatabase != null)
            {
                var entry = philosopherDatabase.GetByRuta(acceptKnot);
                if (entry != null)
                {
                    endingName = entry.displayName;
                    // Registrar permanentemente
                    PlayerPrefs.SetInt(PlayerPrefsKeys.EndingKey(entry.philosopherKey), 1);
                    PlayerPrefs.Save();
                    Debug.Log($"[GameSummaryManager] Final guardado en PlayerPrefs: {entry.philosopherKey}");
                }
            }
            
            // 2. Determinar la Ruta (Arcade o Biblioteca)
            if (GameManager.Instance.GetStoryFlag(PrologueManager.FLAG_ARCADE_VISITED))
            {
                chosenPath = "Arcade";
            }
            else if (GameManager.Instance.GetStoryFlag(PrologueManager.FLAG_LIBRARY_VISITED))
            {
                chosenPath = "Biblioteca";
            }
        }

        // 3. Procesar patos
        int totalDucks = 0;
        int collectedDucks = 0;
        List<GameSummaryUI.DuckDisplayData> duckDataList = new List<GameSummaryUI.DuckDisplayData>();

        if (duckDatabase != null)
        {
            totalDucks = duckDatabase.ducks.Count;
            foreach (var duck in duckDatabase.ducks)
            {
                bool isCollected = false;
                if (GameManager.Instance != null)
                {
                    isCollected = GameManager.Instance.GetStoryFlag(duck.flagName);
                }

                if (isCollected) collectedDucks++;

                duckDataList.Add(new GameSummaryUI.DuckDisplayData
                {
                    duckName = duck.duckName,
                    isCollected = isCollected,
                    collectedSprite = duck.collectedSprite,
                    lockedSprite = duck.lockedSprite
                });
            }
        }

        // 4. Mostrar UI
        summaryUI.ShowSummary(endingName, chosenPath, collectedDucks, totalDucks, duckDataList);
    }
}
