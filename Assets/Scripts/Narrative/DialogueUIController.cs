using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private Button choiceButtonPrefab;

    [Header("Portrait")]
    [SerializeField] private Image portraitImage; // Componente de imagen para el retrato
    [SerializeField] private DialogueTagProcessor tagProcessor; // Referencia al procesador

    [Header("Settings")]
    [SerializeField] private float typewriterSpeed = 0.05f;
    [SerializeField] private float choiceButtonMinHeight = 60f;

    [Header("Audio")]
    [Tooltip("El sonido que se reproduce al saltar a la siguiente línea del diálogo.")]
    [SerializeField] private AudioEvent advanceDialogueSound;

    [Tooltip("El bip constante que suena mientras se escribe el texto automáticamente.")]
    [SerializeField] private AudioEvent typewriterBeepSound;

    [Tooltip("Frecuencia de bips (1 = cada letra, 3 = cada 3 letras, etc.). Ajusta para que suene natural sin aturdir.")]
    [SerializeField] private int lettersPerBeep = 2;

    [System.Serializable]
    public struct CharacterVoice
    {
        public string characterId; // ej: "sophia"
        public AudioEvent voiceBeep;
    }

    [Header("Voces por Personaje")]
    [Tooltip("Define los bips para cada personaje (usa el mismo nombre que el prefijo del sprite, ej: 'sophia' para 'sophia_happy'). Si no hay coincidencia, usa el por defecto.")]
    [SerializeField] private List<CharacterVoice> characterVoices = new List<CharacterVoice>();

    [System.Serializable]
    public struct DialogueSound
    {
        public string soundId;
        public AudioEvent audioEvent;
    }

    [Header("Efectos de Sonido (Tags)")]
    [Tooltip("Define los sonidos llamados por el tag #sonido: nombre_sonido. Ej: id='joseph_suspira'")]
    [SerializeField] private List<DialogueSound> dialogueSounds = new List<DialogueSound>();

    // Evento que se dispara al finalizar el dialogo
    public event Action OnDialogueEnded;

    private Story story;
    private Coroutine typewriterCoroutine;
    private bool isTyping;
    private string currentLineText;
    private AudioEvent defaultTypewriterBeepSound;
    private float currentDelayBeforeTyping = 0f;
    private bool isFirstLineInDialogueSequence = true;

    private void Awake()
    {
        // Guardamos el sonido por defecto para restaurarlo si el personaje no tiene voz configurada
        defaultTypewriterBeepSound = typewriterBeepSound;

        if (tagProcessor != null)
        {
            // Suscribir a eventos visuales y auditivos que vienen de Ink
            tagProcessor.OnPortraitSpriteChanged += UpdatePortraitImage;
            tagProcessor.OnCharacterSpeaking += UpdateTypewriterVoice;
            tagProcessor.OnSoundRequested += HandleSoundRequested;
        }

        // Asegurarse de ocultar inicialmente la imagen de retrato
        if (portraitImage != null && portraitImage.sprite == null)
        {
            portraitImage.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (tagProcessor != null)
        {
            tagProcessor.OnPortraitSpriteChanged -= UpdatePortraitImage;
            tagProcessor.OnCharacterSpeaking -= UpdateTypewriterVoice;
            tagProcessor.OnSoundRequested -= HandleSoundRequested;
        }
    }

    private void HandleSoundRequested(string soundId)
    {
        // Encontrar el sonido en la lista
        int index = dialogueSounds.FindIndex(s => string.Equals(s.soundId, soundId, StringComparison.OrdinalIgnoreCase));
        
        if (index >= 0 && dialogueSounds[index].audioEvent != null)
        {
            dialogueSounds[index].audioEvent.PlaySFX();
            // Asignamos el delay basado en la duracion del clip para que Typewriter espere
            if (dialogueSounds[index].audioEvent.clip != null)
            {
                currentDelayBeforeTyping = dialogueSounds[index].audioEvent.clip.length;
            }
        }
    }

    private void UpdateTypewriterVoice(string characterId)
    {
        if (string.IsNullOrEmpty(characterId)) return;

        // Usamos string.Equals que es seguro contra strings nulos (por si dejaron campos vacíos en el Inspector)
        int index = characterVoices.FindIndex(v => string.Equals(v.characterId, characterId, StringComparison.OrdinalIgnoreCase));
        
        if (index >= 0 && characterVoices[index].voiceBeep != null)
        {
            typewriterBeepSound = characterVoices[index].voiceBeep;
        }
        else
        {
            // Fallback al sonido maestro
            typewriterBeepSound = defaultTypewriterBeepSound;
        }
    }

    private void UpdatePortraitImage(Sprite newSprite)
    {
        if (portraitImage == null) return;

        if (newSprite != null)
        {
            portraitImage.sprite = newSprite;
            portraitImage.gameObject.SetActive(true);
        }
        else
        {
            // Si mandan un sprite nulo o vacio, quizas queramos ocultar el retrato
            portraitImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Limpia los elementos visuales heredados de diálogos pasados.
    /// </summary>
    public void ResetUI()
    {
        if (dialogueText != null) dialogueText.text = "";
        ClearChoices();
        UpdatePortraitImage(null);
        isFirstLineInDialogueSequence = true;
    }

    /// <summary>
    /// Asigna la historia de Ink actual a este controlador.
    /// </summary>
    public void SetStory(Story currentStory)
    {
        story = currentStory;
        isFirstLineInDialogueSequence = true;
    }

    /// <summary>
    /// Obtiene la siguiente linea de la historia de Ink y la asigna al TextMeshProUGUI mediante un efecto Typewriter.
    /// </summary>
    public void DisplayNextLine()
    {
        if (story == null) return;

        if (story.canContinue)
        {
            currentLineText = story.Continue().Trim(); // Limpiar espacios en blanco al inicio o final
            currentDelayBeforeTyping = 0f; // Reiniciar delay
            
            // Procesamos los tags de la linea actual para cualquier efecto visual, de audio o sprite
            if (tagProcessor != null)
            {
                tagProcessor.ProcessTags(story.currentTags);
            }

            // Reproducir sonido de avance solo si NO es la primera linea de la secuencia
            if (!isFirstLineInDialogueSequence && advanceDialogueSound != null)
            {
                advanceDialogueSound.PlaySFX();
            }
            
            isFirstLineInDialogueSequence = false;

            // Detenemos cualquier efecto Typewriter que este en ejecucion
            if (typewriterCoroutine != null)
            {
                StopCoroutine(typewriterCoroutine);
            }
            
            // Iniciamos el efecto Typewriter para la nueva linea
            typewriterCoroutine = StartCoroutine(TypewriterEffect(currentLineText, currentDelayBeforeTyping));
        }
        else if (story.currentChoices.Count == 0)
        {
            // Fin del dialogo
            ClearChoices();
            dialogueText.text = "";

            // Verificar si hay un fade out pendiente del último tag procesado
            if (tagProcessor != null && tagProcessor.PendingFadeOut)
            {
                LevelManager.Instance?.FadeToBlack();
                tagProcessor.PendingFadeOut = false; // Resetear
            }

            OnDialogueEnded?.Invoke();
        }
        else if (story.currentChoices.Count > 0 && !isTyping)
        {
            // Asegurarnos de que las opciones se refresquen si ya se dejo de escribir
            RefreshChoices();
        }
    }

    /// <summary>
    /// Avanza el dialogo si el jugador presiona un boton de interaccion,
    /// o completa el texto si aun se esta escribiendo.
    /// </summary>
    public void OnAdvanceInput()
    {
        if (isTyping)
        {
            // Completar inmediatamente el texto
            if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);
            dialogueText.text = currentLineText;
            isTyping = false;
            RefreshChoices();
        }
        else if (story != null && story.currentChoices.Count == 0)
        {
            // Solo avanza a la siguiente linea si no hay opciones en pantalla
            DisplayNextLine();
        }
    }

    /// <summary>
    /// Limpia el contenedor de opciones y asigna botones dinamicamente para cada opcion disponible.
    /// </summary>
    public void RefreshChoices()
    {
        ClearChoices();

        if (story == null) return;

        List<Choice> currentChoices = story.currentChoices;

        if (currentChoices.Count > 0)
        {
            if (choicesContainer != null) choicesContainer.gameObject.SetActive(true);
        }
        else
        {
            if (choicesContainer != null) choicesContainer.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < currentChoices.Count; i++)
        {
            Choice choice = currentChoices[i];
            
            // Instanciamos el boton en el contenedor
            Button button = Instantiate(choiceButtonPrefab, choicesContainer);
            
            // Asignamos el texto de la opcion al boton
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choice.text;
            }

            // Hacer que el boton crezca verticalmente segun el contenido de texto
            ContentSizeFitter sizeFitter = button.GetComponent<ContentSizeFitter>();
            if (sizeFitter == null)
                sizeFitter = button.gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            // Garantizar una altura minima para botones con texto corto
            LayoutElement layoutElement = button.GetComponent<LayoutElement>();
            if (layoutElement == null)
                layoutElement = button.gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = choiceButtonMinHeight;

            // Asignamos el evento de clic
            int choiceIndex = i; // Capturamos el indice para el closure
            button.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
        }
    }

    /// <summary>
    /// Corrutina para mostrar el texto letra por letra (efecto Typewriter).
    /// </summary>
    /// <param name="line">La linea de texto a mostrar.</param>
    private IEnumerator TypewriterEffect(string line, float delayBeforeStart = 0f)
    {
        isTyping = true;
        dialogueText.text = "";
        
        // Ocultamos las opciones mientras se escribe el texto
        ClearChoices();

        // Esperamos si hay un sonido especial que deba reproducirse antes
        if (delayBeforeStart > 0f)
        {
            yield return new WaitForSeconds(delayBeforeStart);
        }

        int letterIndex = 0;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            
            // Producir sonido estilo "Typewriter beep" al ritmo configurado
            // Y no reproducimos el bip si el carácter es un espacio, para dar una pausa natural.
            if (typewriterBeepSound != null && !char.IsWhiteSpace(letter))
            {
                if (letterIndex % lettersPerBeep == 0)
                {
                    typewriterBeepSound.PlaySFX();
                }
            }
            letterIndex++;

            float currentSpeed = typewriterSpeed;
            if (StoryManager.Instance != null && StoryManager.Instance.IsSkippingMode)
            {
                currentSpeed = 0.005f; // Velocidad de skip (aprox 1 letra por frame)
            }
            
            yield return new WaitForSeconds(currentSpeed);
        }

        // Una vez que termina de escribirse el texto, mostramos las opciones si las hay
        isTyping = false;
        RefreshChoices();
    }

    /// <summary>
    /// Logica que se ejecuta al seleccionar una opcion.
    /// </summary>
    /// <param name="index">El indice de la opcion seleccionada.</param>
    private void OnChoiceSelected(int index)
    {
        if (story != null)
        {
            // Le decimos a Ink que opcion fue elegida
            story.ChooseChoiceIndex(index);
            
            // Refrescamos la interfaz mostrando la siguiente linea
            DisplayNextLine();
        }
    }

    /// <summary>
    /// Destruye los botones de opciones existentes en el contenedor.
    /// </summary>
    private void ClearChoices()
    {
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        if (choicesContainer != null)
        {
            choicesContainer.gameObject.SetActive(false);
        }
    }
}
