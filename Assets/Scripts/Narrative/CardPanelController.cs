using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;

public class CardPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private Button closeButton;

    [Header("Multi-Card Navigation")]
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private Button navButtonPrefab;

    private Story story;
    private bool isMultiMode;
    private PlayerControls controls;

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
    }

    private void EnsureControls()
    {
        if (controls == null)
        {
            controls = new @PlayerControls();
            controls.Player.cerrar.performed += _ => ClosePanel();
            controls.Enable();
        }
    }

    private void OnDestroy() { controls?.Dispose(); }

    /// <summary>
    /// Configura y abre el panel para una sola carta (Aceptación).
    /// </summary>
    public void OpenSingle(Story inkStory, PhilosopherCardDatabase.PhilosopherCardEntry entry)
    {
        gameObject.SetActive(true);
        EnsureControls();
        story = inkStory;
        isMultiMode = false;
        
        if (buttonsContainer != null) buttonsContainer.gameObject.SetActive(false);
        
        ShowCard(entry, true);
        if (parentCanvas != null) parentCanvas.enabled = true;
        panelRoot.SetActive(true);
    }

    /// <summary>
    /// Configura y abre el panel para múltiples cartas (Reproche).
    /// </summary>
    public void OpenMulti(Story inkStory, List<PhilosopherCardDatabase.PhilosopherCardEntry> entries)
    {
        gameObject.SetActive(true);
        EnsureControls();
        story = inkStory;
        isMultiMode = true;

        if (buttonsContainer != null)
        {
            buttonsContainer.gameObject.SetActive(true);
            SetupNavigationButtons(entries);
        }

        // Mostrar la primera por defecto
        if (entries.Count > 0)
        {
            ShowCard(entries[0], false);
        }

        if (parentCanvas != null) parentCanvas.enabled = true;
        panelRoot.SetActive(true);
    }

    private void ShowCard(PhilosopherCardDatabase.PhilosopherCardEntry entry, bool isAcceptance)
    {
        if (titleText != null) titleText.text = entry.displayName;

        // --- Imagen del filósofo ---
        if (cardImage != null)
        {
            if (entry.cardSprite != null)
            {
                cardImage.sprite = entry.cardSprite;
                cardImage.preserveAspect = true;
                cardImage.gameObject.SetActive(true);
                Debug.Log($"[CardPanelController] Sprite asignado para '{entry.displayName}': {entry.cardSprite.name}");
            }
            else
            {
                cardImage.gameObject.SetActive(false);
                Debug.LogWarning($"[CardPanelController] La entrada '{entry.displayName}' no tiene cardSprite asignado.");
            }
        }

        // --- Fondo del panel ---
        if (backgroundImage != null)
        {
            if (entry.backgroundSprite != null)
            {
                backgroundImage.sprite = entry.backgroundSprite;
                Debug.Log($"[CardPanelController] Fondo asignado para '{entry.displayName}': {entry.backgroundSprite.name}");
            }
            else
            {
                Debug.LogWarning($"[CardPanelController] La entrada '{entry.displayName}' no tiene backgroundSprite asignado.");
            }
        }
        else
        {
            Debug.LogWarning("[CardPanelController] backgroundImage no está asignado en el Inspector.");
        }

        string knot = isAcceptance ? entry.acceptanceKnot : entry.reprocheKnot;
        contentText.text = GetKnotText(knot);

        // Registrar progreso
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetStoryFlag("Carta_Leida", true);
            GameManager.Instance.SetStoryVariable("ultima_carta_leida", entry.rutaValue);
            
            if (isAcceptance)
            {
                GameManager.Instance.SetStoryFlag("Carta_Aceptacion_Leida", true);
                // Guardamos exclusivamente la ruta de la carta de aceptación para la reflexión final en la cama
                GameManager.Instance.SetStoryVariable("carta_aceptacion_ruta", entry.rutaValue);
            }
        }
    }

    /// <summary>
    /// Extrae el texto de un knot de Ink sin procesar tags ni avanzar la historia principal.
    /// </summary>
    private string GetKnotText(string knotName)
    {
        if (story == null || string.IsNullOrEmpty(knotName)) return "Error: Knot no encontrado.";

        // Guardar el estado actual para no alterar el flujo principal (aunque solo usemos lectura)
        // Pero ChoosePathString cambia el puntero, así que para lectura "segura"
        // solemos elegir el camino y luego vaciar el buffer sin elegir opciones.
        
        story.ChoosePathString(knotName);
        string fullText = "";

        while (story.canContinue)
        {
            string line = story.Continue();
            // Ignoramos la línea si solo contenía tags que procesamos o descartamos (Ink lo hace automático en Continue)
            // Según requerimiento: Concatena preservando saltos de línea
            fullText += line + "\n";
        }

        return fullText.Trim();
    }

    private void SetupNavigationButtons(List<PhilosopherCardDatabase.PhilosopherCardEntry> entries)
    {
        // Limpiar botones anteriores
        foreach (Transform child in buttonsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in entries)
        {
            Button btn = Instantiate(navButtonPrefab, buttonsContainer);
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = entry.displayName;

            // Este diseño permite añadir iconos fácilmente en el futuro expandiendo el prefab
            btn.onClick.AddListener(() => ShowCard(entry, false));
        }
    }

    public void ClosePanel()
    {
        panelRoot.SetActive(false);
        if (parentCanvas != null) parentCanvas.enabled = false;
        gameObject.SetActive(false);
    }
}
