using UnityEngine;
using System.Collections.Generic;
using Ink.Runtime;

public class FinalRoomManager : MonoBehaviour
{
    [Header("Databases")]
    [SerializeField] private PhilosopherCardDatabase cardDatabase;

    [Header("Tables")]
    [SerializeField] private InteractableTable table1; // Acceptance
    [SerializeField] private InteractableTable table2; // Reproach

    void Start()
    {
        StartCoroutine(InitializeWithDelay());
    }

    private System.Collections.IEnumerator InitializeWithDelay()
    {
        // Esperar un frame para asegurar que StoryManager y UI estén listos
        yield return null;
        InitializeFinalRoom();
    }

    private void InitializeFinalRoom()
    {
        if (StoryManager.Instance == null || cardDatabase == null)
        {
            Debug.LogError("[FinalRoomManager] StoryManager o CardDatabase no encontrados.");
            return;
        }

        Story story = StoryManager.Instance.Story;

        // Leer la variable de ruta desde el GameManager (que persiste entre escenas)
        string currentRuta = "";
        
        if (GameManager.Instance != null)
        {
            currentRuta = GameManager.Instance.GetStoryVariable("ruta");
            Debug.Log("[FinalRoomManager] Ruta recuperada del GameManager: '" + currentRuta + "'");
            
            // Sincronizar la variable interna de Ink SOLO si está declarada en el archivo .ink
            if (!string.IsNullOrEmpty(currentRuta))
            {
                try {
                    story.variablesState["ruta"] = currentRuta;
                } catch {
                    Debug.LogWarning("[FinalRoomManager] La variable 'ruta' no está declarada en el archivo Ink. Ignorando sincronización interna.");
                }
            }
        }

        if (string.IsNullOrEmpty(currentRuta))
        {
            Debug.LogWarning("[FinalRoomManager] No se encontró la variable 'ruta' en el GameManager. Usando fallback.");
            // Intentar leer de Ink como último recurso
            try {
                if (story.variablesState["ruta"] != null)
                    currentRuta = story.variablesState["ruta"].ToString();
            } catch { /* Ignorar si no existe */ }
        }

        // Obtener la entrada del filósofo seguido
        var acceptanceEntry = cardDatabase.GetByRuta(currentRuta);
        if (acceptanceEntry != null)
        {
            table1.SetupAcceptance(acceptanceEntry);
            Debug.Log("[FinalRoomManager] Mesa 1 configurada para: " + acceptanceEntry.displayName);
        }
        else
        {
            Debug.LogError("[FinalRoomManager] Mesa 1 NO pudo configurarse. 'acceptanceEntry' es null para ruta: " + currentRuta);
        }

        // Obtener los otros 3 filósofos (reproche)
        var reproachEntries = cardDatabase.GetAllExcept(currentRuta);
        if (reproachEntries.Count > 0)
        {
            table2.SetupReproach(reproachEntries);
            Debug.Log("[FinalRoomManager] Mesa 2 configurada con " + reproachEntries.Count + " reproches.");
        }
        else
        {
            Debug.LogError("[FinalRoomManager] Mesa 2 NO pudo configurarse. 'reproachEntries' está vacío.");
        }
    }
}
