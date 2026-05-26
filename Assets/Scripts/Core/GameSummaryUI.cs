using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSummaryUI : MonoBehaviour
{
    [Header("UI Elements - Main Panel")]
    [Tooltip("El panel principal que se activará. Permite que el Canvas padre esté siempre encendido.")]
    [SerializeField] private GameObject summaryPanel;

    [Header("UI Elements - Ending")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [Header("UI Elements - Ducks")]
    [SerializeField] private TextMeshProUGUI ducksProgressText;
    [SerializeField] private Transform ducksGridContainer;
    
    [Header("Prefabs")]
    [Tooltip("Prefab con Image (y opcionalmente TextMeshProUGUI para el nombre) para instanciar en el grid")]
    [SerializeField] private GameObject duckIconPrefab;
    
    [Header("Buttons")]
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
    }

    public void ShowSummary(string endingName, string chosenPath, int collectedDucks, int totalDucks, List<DuckDisplayData> duckData)
    {
        if (summaryPanel != null) summaryPanel.SetActive(true);

        if (titleText != null)
        {
            titleText.text = "Final Obtenido: " + endingName;
        }

        if (descriptionText != null)
        {
            descriptionText.text = $"Ruta escogida: {chosenPath}\nFilósofo: {endingName}";
        }

        if (ducksProgressText != null)
        {
            ducksProgressText.text = $"Patos recolectados: {collectedDucks} / {totalDucks}";
        }

        // Limpiar contenedor de patos
        if (ducksGridContainer != null)
        {
            foreach (Transform child in ducksGridContainer)
            {
                Destroy(child.gameObject);
            }

            // Instanciar patos
            foreach (var duck in duckData)
            {
                GameObject duckObj = Instantiate(duckIconPrefab, ducksGridContainer);
                Image img = duckObj.GetComponentInChildren<Image>();
                TextMeshProUGUI txt = duckObj.GetComponentInChildren<TextMeshProUGUI>();

                if (img != null)
                {
                    img.sprite = duck.isCollected ? duck.collectedSprite : duck.lockedSprite;
                    img.preserveAspect = true;
                }
                
                if (txt != null)
                {
                    txt.text = duck.isCollected ? duck.duckName : "???";
                }
            }
        }
    }

    private void OnContinueClicked()
    {
        Time.timeScale = 1f;

        if (summaryPanel != null) summaryPanel.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetStoryFlag(GameManager.Instance.CompletionFlag, true);
            GameManager.Instance.SaveGame();
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ChangeScene("Menu");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }

    public void HideSummary()
    {
        if (summaryPanel != null) summaryPanel.SetActive(false);
    }

    public struct DuckDisplayData
    {
        public string duckName;
        public bool isCollected;
        public Sprite collectedSprite;
        public Sprite lockedSprite;
    }
}
