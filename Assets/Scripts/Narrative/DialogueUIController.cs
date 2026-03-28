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

    // Evento que se dispara al finalizar el dialogo
    public event Action OnDialogueEnded;

    private Story story;
    private Coroutine typewriterCoroutine;
    private bool isTyping;
    private string currentLineText;

    private void Awake()
    {
        if (tagProcessor != null)
        {
            // Suscribir a eventos que disparan cambios visuales a raiz de tags
            tagProcessor.OnPortraitSpriteChanged += UpdatePortraitImage;
        }

        // Asegurarse de ocultar inicialmente la imagen de retrato si esta vacia
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
    }

    /// <summary>
    /// Asigna la historia de Ink actual a este controlador.
    /// </summary>
    public void SetStory(Story currentStory)
    {
        story = currentStory;
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
            
            // Procesamos los tags de la linea actual para cualquier efecto visual, de audio o sprite
            if (tagProcessor != null)
            {
                tagProcessor.ProcessTags(story.currentTags);
            }

            // Detenemos cualquier efecto Typewriter que este en ejecucion
            if (typewriterCoroutine != null)
            {
                StopCoroutine(typewriterCoroutine);
            }
            
            // Iniciamos el efecto Typewriter para la nueva linea
            typewriterCoroutine = StartCoroutine(TypewriterEffect(currentLineText));
        }
        else if (story.currentChoices.Count == 0)
        {
            // Fin del dialogo
            ClearChoices();
            dialogueText.text = "";
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
    private IEnumerator TypewriterEffect(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        
        // Ocultamos las opciones mientras se escribe el texto
        ClearChoices();

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
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
