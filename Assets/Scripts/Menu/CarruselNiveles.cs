using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class CarruselNiveles : MonoBehaviour
{
    public Image imagenNivel;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoTiempo;

    public Image estrella1;
    public Image estrella2;
    public Image estrella3;
    public Image estrella4;

    public GameObject candado;

    public Sprite[] imagenesNiveles;
    public string[] tiempos;
    public int[] dificultad;

    int nivelActual = 0;

    void Start()
    {
        ActualizarNivel();
    }

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            NivelAnterior();
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            SiguienteNivel();
        }
    }

    public void SiguienteNivel()
    {
        nivelActual++;

        if (nivelActual >= imagenesNiveles.Length)
        {
            nivelActual = 0;
        }

        ActualizarNivel();
    }

    public void NivelAnterior()
    {
        nivelActual--;

        if (nivelActual < 0)
        {
            nivelActual = imagenesNiveles.Length - 1;
        }

        ActualizarNivel();
    }
    void ActualizarNivel()
    {
        imagenNivel.sprite = imagenesNiveles[nivelActual];

        textoNivel.text = "Nivel " + (nivelActual + 1);

        textoTiempo.text = tiempos[nivelActual];

        // bloquear niveles
        candado.SetActive(nivelActual != 0);

        if (nivelActual == 0)
        {
            // Nivel 1: Las estrellas representan los 4 finales
            estrella1.enabled = true;
            estrella2.enabled = true;
            estrella3.enabled = true;
            estrella4.enabled = true;

            Color unlockedColor = new Color32(255, 210, 0, 255);
            Color lockedColor = Color.white;

            // Orden: Biblioteca (Estoicos, Nietzsche), Arcade (Schopenhauer, Hegel)
            estrella1.color = PlayerPrefs.GetInt(PlayerPrefsKeys.EndingKey("Estoicos"), 0) == 1 ? unlockedColor : lockedColor;
            estrella2.color = PlayerPrefs.GetInt(PlayerPrefsKeys.EndingKey("Nietzsche"), 0) == 1 ? unlockedColor : lockedColor;
            estrella3.color = PlayerPrefs.GetInt(PlayerPrefsKeys.EndingKey("Schopenhauer"), 0) == 1 ? unlockedColor : lockedColor;
            estrella4.color = PlayerPrefs.GetInt(PlayerPrefsKeys.EndingKey("Hegel"), 0) == 1 ? unlockedColor : lockedColor;
        }
        else
        {
            // Otros niveles: Lógica original de dificultad
            int estrellas = dificultad[nivelActual];
            estrella1.enabled = estrellas >= 1;
            estrella2.enabled = estrellas >= 2;
            estrella3.enabled = estrellas >= 3;
            estrella4.enabled = estrellas >= 4;

            estrella1.color = Color.white;
            estrella2.color = Color.white;
            estrella3.color = Color.white;
            estrella4.color = Color.white;
        }
    }

    /// <summary>
    /// Inicia el nivel seleccionado delegando completamente al GameManager.
    /// El GameManager decide si cargar una partida existente o iniciar una nueva,
    /// según si existe un save en disco.
    ///
    /// REGLA: CarruselNiveles NO debe acceder a GameStateSO ni a SaveSystem directamente.
    /// </summary>
    public void CargarNivel()
    {
        if (nivelActual != 0) return; // Por ahora solo el nivel 0 (Parque) está disponible

        if (GameManager.Instance == null)
        {
            // Fallback de emergencia solo para debug directo desde escena de menú
            Debug.LogWarning("[CarruselNiveles] GameManager no encontrado. Cargando escena directamente (solo para debug).");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Parque");
            return;
        }

        // Delegamos completamente: el GameManager verifica el save y decide el flujo.
        GameManager.Instance.RequestLoadLevel("Parque");
    }
}