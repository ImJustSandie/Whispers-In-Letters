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

    [Tooltip("Arrastra aqui el objeto Estado (GameStateSO) para que el menu pueda leer la ultima escena sin necesitar un GameManager.")]
    public GameStateSO gameState;

    public void CargarNivel()
    {
        if (nivelActual == 0)
        {
            string escenaDestino = "Parque"; // Escena por defecto

            // Si le damos el reference por Inspector, leemos los datos directo del archivo SO.
            // Asi evitamos problemas si el GameManager todavia no ha cargado en la escena inicial.
            if (gameState != null)
            {
                string escenaGuardada = gameState.currentSceneName;
                if (!string.IsNullOrEmpty(escenaGuardada))
                {
                    escenaDestino = escenaGuardada;
                }
            }
            // Fallback por si usan GameManager en lugar de asignarlo directo
            else if (GameManager.Instance != null && GameManager.Instance.GetGameState() != null)
            {
                string escenaGuardada = GameManager.Instance.GetGameState().currentSceneName;
                if (!string.IsNullOrEmpty(escenaGuardada))
                {
                    escenaDestino = escenaGuardada;
                }
            }

            Debug.Log($"[CarruselNiveles] Cargando partida en: {escenaDestino}");

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ChangeScene(escenaDestino);
            }
            else
            {
                SceneManager.LoadScene(escenaDestino);
            }
        }
    }
}