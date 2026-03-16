using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CarruselNiveles : MonoBehaviour
{
    public Image imagenNivel;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoTiempo;

    public Image estrella1;
    public Image estrella2;
    public Image estrella3;

    public GameObject candado;

    public Sprite[] imagenesNiveles;
    public string[] tiempos;
    public int[] dificultad;

    int nivelActual = 0;

    void Start()
    {
        ActualizarNivel();
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

        // bloquear niveles
        if (nivelActual == 0)
            candado.SetActive(false);
        else
            candado.SetActive(true);
    }

    public void CargarNivel()
    {
        if (nivelActual == 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}