using UnityEngine;
using Ink.Runtime;

/// <summary>
/// Gestiona la interacción con la cama para mostrar reflexiones finales
/// basadas en la última carta leída.
/// </summary>
public class FinalReflectionInteractable : MonoBehaviour, IInteractable
{
    [Header("Database")]
    [SerializeField] private PhilosopherCardDatabase cardDatabase;

    [Header("Knots de Ink")]
    [Tooltip("Knot cuando aún no se han cumplido los requisitos narrativos para dormir.")]
    [SerializeField] private string lockedRoomKnot = "Habitacion_Bloqueada";
    
    [Tooltip("Knot cuando la habitación está abierta pero no se ha leído la carta de aceptación.")]
    [SerializeField] private string unreadLetterKnot = "Cama_Bloqueada";

    [Tooltip("Knot de respaldo si no se encuentra una reflexión específica.")]
    [SerializeField] private string defaultReflectionKnot = "Reflexion_Final";

    [Tooltip("Knot de confirmación antes de dormir.")]
    [SerializeField] private string confirmationKnot = "Confirmacion_Dormir";

    public string GetInteractionName()
    {
        return "Descansar";
    }

    public void Interact()
    {
        if (GameManager.Instance == null || StoryManager.Instance == null) return;

        // 1. Verificar si ha alcanzado un final (desbloquea el cuarto)
        if (!GameManager.Instance.GetStoryFlag("Final_Alcanzado"))
        {
            StoryManager.Instance.StartStory(lockedRoomKnot);
            return;
        }

        // 2. Verificar si ha leído una carta de ACEPTACIÓN
        if (!GameManager.Instance.GetStoryFlag("Carta_Aceptacion_Leida"))
        {
            StoryManager.Instance.StartStory(unreadLetterKnot);
            return;
        }

        // 3. Obtener qué carta fue la última leída
        string ultimaCarta = GameManager.Instance.GetStoryVariable("ultima_carta_leida");
        string selectedReflectionKnot = defaultReflectionKnot;

        if (!string.IsNullOrEmpty(ultimaCarta))
        {
            var entry = cardDatabase.GetByRuta(ultimaCarta);
            if (entry != null && !string.IsNullOrEmpty(entry.reflectionKnot))
            {
                selectedReflectionKnot = entry.reflectionKnot;
            }
        }

        // 4. Setear la variable para Ink y mostrar confirmación
        GameManager.Instance.SetStoryVariable("proxima_reflexion", selectedReflectionKnot);
        StoryManager.Instance.StartStory(confirmationKnot);
    }
}
