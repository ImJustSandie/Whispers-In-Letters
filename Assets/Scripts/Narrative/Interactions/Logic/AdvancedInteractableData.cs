using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InteractionCondition
{
    public enum ConditionType { Ninguna, RequerirFlag, RequerirVariable }
    
    [Tooltip("¿Qué tipo de condición se debe cumplir?")]
    public ConditionType conditionType;
    
    [Tooltip("Nombre del flag (Si se eligió RequerirFlag)")]
    public string flagName;
    
    [Tooltip("Clave de la variable (Ej: ruta_actual)")]
    public string variableKey;
    
    [Tooltip("Valor que debe tener la variable (Ej: 2)")]
    public string requiredValue;

    public bool IsMet()
    {
        if (GameManager.Instance == null) return false;

        switch (conditionType)
        {
            case ConditionType.Ninguna:
                return true;
            case ConditionType.RequerirFlag:
                return GameManager.Instance.GetStoryFlag(flagName);
            case ConditionType.RequerirVariable:
                return GameManager.Instance.GetStoryVariable(variableKey) == requiredValue;
            default:
                return true;
        }
    }
}

[System.Serializable]
public class InteractionEntry
{
    [Tooltip("Nombre descriptivo para organizar en el inspector (ej: 'Después de hablar con Joseph')")]
    public string description;
    
    [Tooltip("El knot de Ink a ejecutar si se cumplen las condiciones")]
    public string inkKnot;
    
    [Tooltip("Lista de condiciones. Se deben cumplir TODAS para que se asigne este Knot.")]
    public List<InteractionCondition> conditions = new List<InteractionCondition>();

    public bool CheckConditions()
    {
        foreach (var cond in conditions)
        {
            if (!cond.IsMet()) return false;
        }
        return true; 
    }
}

[CreateAssetMenu(menuName = "Narrative/Advanced Interactable")]
public class AdvancedInteractableData : ScriptableObject
{
    public string interactionName;
    
    [Tooltip("Lista de posibles interacciones. Se revisarán en ORDEN de arriba a abajo. La primera que cumpla sus condiciones enviará su Knot.")]
    public List<InteractionEntry> interactions = new List<InteractionEntry>();

    public string GetValidKnot()
    {
        foreach (var entry in interactions)
        {
            if (entry.CheckConditions())
            {
                return entry.inkKnot;
            }
        }
        return string.Empty;
    }
}
