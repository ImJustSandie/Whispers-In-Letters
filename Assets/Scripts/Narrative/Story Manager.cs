using Ink.Runtime;
using UnityEngine;
using TMPro;
using System;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public TextAsset inkJSON;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story story;
    private bool dialogueActive;
    private bool pendingEnd;

    public bool IsDialogueActive => dialogueActive;
    public event Action<bool> OnDialogueStateChanged;

    void Awake()
    {
        Instance = this;

        if (inkJSON == null)
        {
            Debug.LogError("[StoryManager] inkJSON no esta asignado en el Inspector.");
            return;
        }

        story = new Story(inkJSON.text);
        Debug.Log("[StoryManager] Story inicializado correctamente.");

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void StartStory(string knot)
    {
        Debug.Log("[StoryManager] StartStory llamado con knot: '" + knot + "'");

        if (string.IsNullOrEmpty(knot))
        {
            Debug.LogWarning("[StoryManager] El knot esta vacio o nulo.");
            return;
        }

        if (dialogueActive)
        {
            Debug.LogWarning("[StoryManager] Ya hay un dialogo activo. Ignorando nueva llamada.");
            return;
        }

        story.ChoosePathString(knot);
        dialogueActive = true;
        pendingEnd = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        OnDialogueStateChanged?.Invoke(true);

        AdvanceStory();
    }

    public void AdvanceStory()
    {
        if (!dialogueActive)
        {
            Debug.LogWarning("[StoryManager] AdvanceStory llamado sin dialogo activo.");
            return;
        }

        if (pendingEnd)
        {
            EndStory();
            return;
        }

        if (story.canContinue)
        {
            string line = story.Continue().Trim();
            Debug.Log(line);

            if (dialogueText != null)
            {
                dialogueText.text = line;
            }

            pendingEnd = !story.canContinue && story.currentChoices.Count == 0;
            return;
        }

        if (story.currentChoices.Count > 0)
        {
            Debug.LogWarning("[StoryManager] Hay opciones pero no hay UI para elegir; se tomara la primera por defecto.");
            story.ChooseChoiceIndex(0);
            AdvanceStory();
            return;
        }

        EndStory();
    }

    private void EndStory()
    {
        dialogueActive = false;
        pendingEnd = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        OnDialogueStateChanged?.Invoke(false);
    }
}