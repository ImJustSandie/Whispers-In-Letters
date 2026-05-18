using UnityEngine;

public class TutorialCarrusel : MonoBehaviour
{
    [Header("Paneles en orden")]
    public GameObject[] paneles;

    private int panelActual = 0;

    void Start()
    {
        MostrarPanel(panelActual);
    }

    // Mostrar panel según índice
    void MostrarPanel(int indice)
    {
        for (int i = 0; i < paneles.Length; i++)
        {
            paneles[i].SetActive(i == indice);
        }
    }

    // Botón siguiente >
    public void Siguiente()
    {
        if (panelActual < paneles.Length - 1)
        {
            panelActual++;
            MostrarPanel(panelActual);
        }
    }

    // Botón anterior <
    public void Anterior()
    {
        if (panelActual > 0)
        {
            panelActual--;
            MostrarPanel(panelActual);
        }
    }

    // Botón SI
    public void EmpezarTutorial()
    {
        panelActual = 1;
        MostrarPanel(panelActual);
    }

    // Botón NO
    public void CerrarTutorial()
    {
        gameObject.SetActive(false);
    }

    // Finalizar tutorial
    public void FinalizarTutorial()
    {
        gameObject.SetActive(false);
    }
}