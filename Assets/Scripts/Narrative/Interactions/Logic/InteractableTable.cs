using UnityEngine;
using System.Collections.Generic;

public class InteractableTable : MonoBehaviour, IInteractable
{
    public enum TableMode { Acceptance, Reproach }
    [SerializeField] private TableMode mode;
    [SerializeField] private CardPanelController panelController;
    [SerializeField] private string interactionName = "Mesa de Epílogos";

    private PhilosopherCardDatabase.PhilosopherCardEntry acceptanceEntry;
    private List<PhilosopherCardDatabase.PhilosopherCardEntry> reproachEntries;

    public void SetupAcceptance(PhilosopherCardDatabase.PhilosopherCardEntry entry)
    {
        acceptanceEntry = entry;
    }

    public void SetupReproach(List<PhilosopherCardDatabase.PhilosopherCardEntry> entries)
    {
        reproachEntries = entries;
    }

    public void Interact()
    {
        Debug.LogWarning("[InteractableTable] MÉTODO INTERACT LLAMADO CORRECTAMENTE.");

        if (panelController == null)
        {
            Debug.LogError("[InteractableTable] No se ha asignado CardPanelController en el Inspector.");
            return;
        }

        if (mode == TableMode.Acceptance)
        {
            if (acceptanceEntry != null)
            {
                Debug.LogWarning("[InteractableTable] Abriendo Acceptance Panel para: " + acceptanceEntry.displayName);
                panelController.OpenSingle(StoryManager.Instance.Story, acceptanceEntry);
            }
            else
            {
                Debug.LogWarning("[InteractableTable] No hay datos de aceptación configurados en esta mesa.");
            }
        }
        else if (mode == TableMode.Reproach)
        {
            if (reproachEntries != null && reproachEntries.Count > 0)
            {
                Debug.LogWarning("[InteractableTable] Abriendo Reproach Panel con " + reproachEntries.Count + " cartas.");
                panelController.OpenMulti(StoryManager.Instance.Story, reproachEntries);
            }
            else
            {
                Debug.LogWarning("[InteractableTable] No hay datos de reproche configurados en esta mesa.");
            }
        }
    }

    public string GetInteractionName()
    {
        return interactionName;
    }
}
