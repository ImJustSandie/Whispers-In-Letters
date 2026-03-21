using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameState", menuName = "Core/Game State")]
public class GameStateSO : ScriptableObject
{
    [Header("Scene Information")]
    public string currentSceneName;
    public string previousSceneName;

    [Header("Player Information")]
    public Vector3 playerPosition;

    [Header("Narrative Flags (Opcionales)")]
    [Tooltip("Lista de decisiones binarias que ya ocurrieron.")]
    [SerializeField] private List<string> unlockedFlags = new List<string>();

    [System.Serializable]
    public struct StoryVariable
    {
        public string key;
        public string value;
    }

    [Header("Narrative Variables (Clave-Valor)")]
    [Tooltip("Lista de variables donde una clave especifica guarda un único valor.")]
    [SerializeField] private List<StoryVariable> storyVariables = new List<StoryVariable>();

    /// <summary>
    /// Marca o desmarca un evento narrativo (path).
    /// </summary>
    public void SetFlag(string flagName, bool state)
    {
        if (state)
        {
            if (!unlockedFlags.Contains(flagName)) unlockedFlags.Add(flagName);
        }
        else
        {
            if (unlockedFlags.Contains(flagName)) unlockedFlags.Remove(flagName);
        }
    }

    /// <summary>
    /// Devuelve verdadero si el evento ya fue registrado.
    /// </summary>
    public bool HasFlag(string flagName)
    {
        return unlockedFlags.Contains(flagName);
    }

    /// <summary>
    /// Guarda o actualiza una variable específica (ej. actitud_joseph = motivado).
    /// Asegura de que nunca hayan variables duplicadas (son mutables o únicas).
    /// </summary>
    public void SetVariable(string key, string value)
    {
        for (int i = 0; i < storyVariables.Count; i++)
        {
            if (storyVariables[i].key == key)
            {
                // Si la clave ya existe, mutamos su valor sobreescribiendo el viejo
                StoryVariable v = storyVariables[i];
                v.value = value;
                storyVariables[i] = v;
                return;
            }
        }
        
        // Si no existía de antes, la creamos nueva
        storyVariables.Add(new StoryVariable { key = key, value = value });
    }

    /// <summary>
    /// Obtiene el valor guardado de una variable, o un string vacio si la clave no existe.
    /// </summary>
    public string GetVariable(string key)
    {
        foreach (var v in storyVariables)
        {
            if (v.key == key) return v.value;
        }
        return string.Empty;
    }

    /// <summary>
    /// Reinicia todo el estado a limpio (Util para un nuevo juego).
    /// </summary>
    [ContextMenu("Resetear Estado (Para Debugging)")]
    public void ClearState()
    {
        currentSceneName = "";
        previousSceneName = "";
        playerPosition = Vector3.zero;
        unlockedFlags.Clear();
        storyVariables.Clear();
    }
}
