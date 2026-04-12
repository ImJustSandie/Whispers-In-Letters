using UnityEngine;

public class InteractUI : MonoBehaviour
{
    public GameObject iconoInteractuar;

    public void Mostrar()
    {
        iconoInteractuar.SetActive(true);
    }

    public void Ocultar()
    {
        iconoInteractuar.SetActive(false);
    }
}