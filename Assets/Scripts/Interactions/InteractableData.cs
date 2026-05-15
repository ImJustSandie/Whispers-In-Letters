using UnityEngine;

[CreateAssetMenu(menuName = "Narrative/Interactable")]
public class InteractableData : ScriptableObject
{
    public string interactionName;
    public string inkKnot;

    [Header("Collectable")]
    [Tooltip("Si está activo, este objeto funciona como coleccionable: " +
             "al interactuar con el flag requerido cumplido, se recoge (set flag + save + desaparece). " +
             "Si el flag no se cumple, redirige al fallbackKnot del InteractableObject.")]
    public bool isCollectable = false;

    [Tooltip("Flag que se activará en GameState al recoger este objeto. " +
             "También se usa para detectar si ya fue recogido al recargar la escena.")]
    public string flagToSetOnCollect;
}