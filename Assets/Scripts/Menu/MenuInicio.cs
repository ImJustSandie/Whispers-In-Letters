
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuInicio : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelCreditos;
    public GameObject panelAjustes;

    public void Jugar()
    {
        SceneManager.LoadScene("Niveles");
    }

    public void MostrarCreditos()
    {
        panelMenu.SetActive(false);
        panelCreditos.SetActive(true);
    }

    public void MostrarAjustes()
    {
        panelMenu.SetActive(false);
        panelAjustes.SetActive(true);
    }

    public void VolverMenu()
    {
        panelCreditos.SetActive(false);
        panelAjustes.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }
}