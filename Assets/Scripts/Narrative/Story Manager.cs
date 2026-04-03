using Ink.Runtime;
using UnityEngine;
using System;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public TextAsset inkJSON;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;

    private DialogueUIController uiController;
    private Story story;
    private bool dialogueActive;
    private bool isSkippingMode;
    private float skipTimer;
    [SerializeField] private float skipDelay = 0.2f;

    public bool IsDialogueActive => dialogueActive;
    public bool IsSkippingMode => isSkippingMode;
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

        // Vincular funciones externas para que Ink lea directamente del GameManager
        story.BindExternalFunction("GetFlag", (string flagName) => {
            return GameManager.Instance != null && GameManager.Instance.GetStoryFlag(flagName);
        });

        story.BindExternalFunction("GetVar", (string varName) => {
            if (GameManager.Instance != null) return GameManager.Instance.GetStoryVariable(varName);
            return "";
        });

        if (dialoguePanel != null)
        {
            // Intentamos obtener el DialogueUIController en el panel o en sus hijos
            uiController = dialoguePanel.GetComponentInChildren<DialogueUIController>(true);
            
            if (uiController == null)
            {
                Debug.LogError("[StoryManager] No se encontro DialogueUIController en el dialoguePanel ni en sus hijos.");
            }
            else
            {
                // Suscribirse al evento de finalizacion del dialogo
                uiController.OnDialogueEnded += EndStory;
            }

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

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        if (uiController != null)
        {
            uiController.ResetUI();
            uiController.SetStory(story);
            uiController.DisplayNextLine();
        }

        OnDialogueStateChanged?.Invoke(true);
    }

    public void AdvanceStory()
    {
        if (!dialogueActive)
        {
            Debug.LogWarning("[StoryManager] AdvanceStory llamado sin dialogo activo.");
            return;
        }

        // Delegamos el avance del dialogo al UI Controller (para saltar el Typewriter o avanzar de linea)
        if (uiController != null)
        {
            uiController.OnAdvanceInput();
        }
    }

    public void SetSkipMode(bool skip)
    {
        isSkippingMode = skip;
    }

    private void Update()
    {
        if (isSkippingMode && dialogueActive)
        {
            skipTimer += Time.deltaTime;
            if (skipTimer >= skipDelay)
            {
                skipTimer = 0f;
                AdvanceStory();
            }
        }
        else
        {
            skipTimer = 0f;
        }
    }

    private void EndStory()
    {
        dialogueActive = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        OnDialogueStateChanged?.Invoke(false);
    }
}