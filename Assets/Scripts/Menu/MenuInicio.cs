
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuInicio : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelCreditos;
    public GameObject panelAjustes;
    public GameObject panelNiveles;
    public Animator animPanelNiveles;

    [Header("Sonidos")]
    public AudioEvent clickSound;

    public void Jugar()
    {
        // 1. Reproducir el sonido explícitamente primero
        if (clickSound != null) clickSound.PlayUI();

        // 2. Darle un micro-tiempo al motor de audio para sonar 
        // antes del congelamiento por activar el otro canvas (Unity Layout Rebuild)
        StartCoroutine(JugarRoutine());
    }

    private System.Collections.IEnumerator JugarRoutine()
    {
        // Esperamos 1 solo frame, apenas lo suficiente para inyectar el audio en el buffer
        yield return null; 

        panelMenu.SetActive(false);
        panelNiveles.SetActive(true);
        if (animPanelNiveles != null)
        {
            animPanelNiveles.Play("PanelNiveles_Enter");
        }
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