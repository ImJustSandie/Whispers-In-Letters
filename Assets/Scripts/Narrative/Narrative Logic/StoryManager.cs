using Ink.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public Story Story => story;
    public bool IsDialogueActive => dialogueActive;
    public bool IsSkippingMode => isSkippingMode;
    public event Action<bool> OnDialogueStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeStory();
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
        // Muy importante: Al cambiar de escena, la referencia al dialoguePanel se vuelve null/missing 
        // porque ese objeto pertenecía a la escena anterior. Debemos re-vincularlo.
        RefreshUIReferences();

        // Al volver al menú, reiniciamos el estado del Story de Ink
        // para evitar que el call stack interno acumule estado innecesariamente.
        if (scene.name == "Menu")
        {
            ResetStoryState();
        }
    }

    private void InitializeStory()
    {
        if (inkJSON == null)
        {

            return;
        }

        story = new Story(inkJSON.text);


        // Vincular funciones externas
        story.BindExternalFunction("GetFlag", (string flagName) => {
            return GameManager.Instance != null && GameManager.Instance.GetStoryFlag(flagName);
        });

        story.BindExternalFunction("GetVar", (string varName) => {
            if (GameManager.Instance != null) return GameManager.Instance.GetStoryVariable(varName);
            return "";
        });

        RefreshUIReferences();
    }

    /// <summary>
    /// Busca el panel de diálogo en la escena actual y re-establece las conexiones.
    /// Útil tras un cambio de escena o si se perdió la referencia.
    /// </summary>
    public void RefreshUIReferences()
    {
        // Al cargar una nueva escena, siempre asumimos que el diálogo debe empezar limpio
        dialogueActive = false;

        // 1. Intentar encontrar el panel si se perdió
        if (dialoguePanel == null)
        {
            // Buscamos un objeto llamado "DialoguePanel" o similar. 
            // Si el usuario usa un nombre distinto, lo ideal es usar tags.
            dialoguePanel = GameObject.Find("DialoguePanel");
            
            if (dialoguePanel == null)
            {
                // Intento desesperado: buscar cualquier objeto con DialogueUIController
                var foundUI = UnityEngine.Object.FindAnyObjectByType<DialogueUIController>(FindObjectsInactive.Include);
                if (foundUI != null) dialoguePanel = foundUI.transform.parent.gameObject;
            }
        }

        if (dialoguePanel != null)
        {
            uiController = dialoguePanel.GetComponentInChildren<DialogueUIController>(true);
            
            if (uiController != null)
            {
                // Limpiar suscripciones previas para evitar llamadas dobles si el objeto persiste
                uiController.OnDialogueEnded -= EndStory;
                uiController.OnDialogueEnded += EndStory;

                // Limpiar la interfaz y flags pendientes (como el Fade Out)
                uiController.ResetUI();
                

            }
            
            dialoguePanel.SetActive(false);
        }
        else
        {

        }
    }

    public void ResetStoryState()
    {
        if (story != null)
        {
            story.UnbindExternalFunction("GetFlag");
            story.UnbindExternalFunction("GetVar");
        }

        InitializeStory();

    }

    public void StartStory(string knot)
    {


        if (string.IsNullOrEmpty(knot))
        {

            return;
        }

        if (dialogueActive)
        {

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