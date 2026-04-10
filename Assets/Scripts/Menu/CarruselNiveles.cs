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

        int estrellas = dificultad[nivelActual];

        estrella1.enabled = estrellas >= 1;
        estrella2.enabled = estrellas >= 2;
        estrella3.enabled = estrellas >= 3;
        estrella4.enabled = estrellas >= 4;

        // bloquear niveles
        if (nivelActual == 0)
            candado.SetActive(false);
        else
            candado.SetActive(true);
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