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

            return;
        }

        Story story = StoryManager.Instance.Story;

        // Leer la variable de ruta desde el GameManager (que persiste entre escenas)
        string currentRuta = "";
        
        if (GameManager.Instance != null)
        {
            currentRuta = GameManager.Instance.GetStoryVariable("ruta");

            
            // Sincronizar la variable interna de Ink SOLO si está declarada en el archivo .ink
            if (!string.IsNullOrEmpty(currentRuta))
            {
                try {
                    story.variablesState["ruta"] = currentRuta;
                } catch {

                }
            }
        }

        if (string.IsNullOrEmpty(currentRuta))
        {

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

        }
        else
        {

        }

        // Obtener los otros 3 filósofos (reproche)
        var reproachEntries = cardDatabase.GetAllExcept(currentRuta);
        if (reproachEntries.Count > 0)
        {
            table2.SetupReproach(reproachEntries);

        }
        else
        {

        }
    }
}
