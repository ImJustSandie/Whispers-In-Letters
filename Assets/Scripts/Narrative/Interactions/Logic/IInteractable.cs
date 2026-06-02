public interface IInteractable
{
    /// <summary>
    /// Se llama cuando el jugador presiona el botón de interactuar.
    /// </summary>
    void Interact();

    /// <summary>
    /// Obtiene el nombre con el que se mostrará este objeto en la UI u logs.
    /// </summary>
    string GetInteractionName();
}
