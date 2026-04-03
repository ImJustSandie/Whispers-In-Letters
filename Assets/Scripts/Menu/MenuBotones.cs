using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBotones : MonoBehaviour
{
    public GameObject panelNiveles;
    public GameObject panelMenu;

    public void IrAlMenu()
    {
        panelMenu.SetActive(true);
        panelNiveles.SetActive(false);

    }
}