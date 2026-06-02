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
        if (panelController == null)
        {
            return;
        }

        if (mode == TableMode.Acceptance)
        {
            if (acceptanceEntry != null)
            {
                panelController.OpenSingle(StoryManager.Instance.Story, acceptanceEntry);
            }
        }
        else if (mode == TableMode.Reproach)
        {
            if (reproachEntries != null && reproachEntries.Count > 0)
            {
                panelController.OpenMulti(StoryManager.Instance.Story, reproachEntries);
            }
        }
    }

    public string GetInteractionName()
    {
        return interactionName;
    }
}
