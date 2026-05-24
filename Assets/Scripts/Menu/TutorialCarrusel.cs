using UnityEngine;

public class TutorialCarrusel : MonoBehaviour
{
    [Header("Paneles en orden")]
    public GameObject[] paneles;

    private int panelActual = 0;

    private static bool tutorialCompletado = false;

    void Start()
    {
        if (tutorialCompletado)
        {
            gameObject.SetActive(false);
            return;
        }
        MostrarPanel(panelActual);
    }

    // Mostrar panel seg�n �ndice
    void MostrarPanel(int indice)
    {
        for (int i = 0; i < paneles.Length; i++)
        {
            paneles[i].SetActive(i == indice);
        }
    }

    // Bot�n siguiente >
    public void Siguiente()
    {
        if (panelActual < paneles.Length - 1)
        {
            panelActual++;
            MostrarPanel(panelActual);
        }
    }

    // Bot�n anterior <
    public void Anterior()
    {
        if (panelActual > 0)
        {
            panelActual--;
            MostrarPanel(panelActual);
        }
    }

    // Bot�n SI
    public void EmpezarTutorial()
    {
        panelActual = 1;
        MostrarPanel(panelActual);
    }

    // Botn NO
    public void CerrarTutorial()
    {
        tutorialCompletado = true;
        gameObject.SetActive(false);
    }

    // Finalizar tutorial
    public void FinalizarTutorial()
    {
        gameObject.SetActive(false);
        tutorialCompletado = true;
    }
}