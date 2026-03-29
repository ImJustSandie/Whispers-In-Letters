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

    [Header("Narrative Flags")]
    [Tooltip("Lista de decisiones narrativas o eventos que ya ocurrieron.")]
    [SerializeField] private List<string> unlockedFlags = new List<string>();

    [System.Serializable]
    public struct StoryVariable
    {
        public string key;
        public string value;
    }

    [Header("Story Variables")]
    [Tooltip("Variables dinámicas del juego (ej. actitud_joseph = motivado).")]
    [SerializeField] private List<StoryVariable> storyVariables = new List<StoryVariable>();

    /// <summary>
    /// Marca o desmarca un evento narrativo (path).
    /// </summary>
    public void SetFlag(string flagName, bool state)
    {
        if (string.IsNullOrEmpty(flagName)) return;
        string cleanFlag = flagName.Trim();
        string lowerFlag = cleanFlag.ToLower();

        int index = -1;
        for (int i = 0; i < unlockedFlags.Count; i++)
        {
            if (unlockedFlags[i].Trim().ToLower() == lowerFlag)
            {
                index = i;
                break;
            }
        }

        if (state)
        {
            if (index == -1) unlockedFlags.Add(cleanFlag);
        }
        else
        {
            if (index != -1) unlockedFlags.RemoveAt(index);
        }
    }

    /// <summary>
    /// Devuelve verdadero si el evento ya fue registrado.
    /// </summary>
    public bool HasFlag(string flagName)
    {
        if (string.IsNullOrEmpty(flagName)) return false;
        string lowerFlag = flagName.Trim().ToLower();

        foreach (var f in unlockedFlags)
        {
            if (f.Trim().ToLower() == lowerFlag) return true;
        }
        return false;
    }

    /// <summary>
    /// Guarda o actualiza una variable específica (ej. actitud_joseph = motivado).
    /// Asegura de que nunca hayan variables duplicadas (son mutables o únicas).
    /// </summary>
    public void SetVariable(string key, string value)
    {
        if (string.IsNullOrEmpty(key)) return;
        string lowerKey = key.Trim().ToLower();

        for (int i = 0; i < storyVariables.Count; i++)
        {
            if (storyVariables[i].key.Trim().ToLower() == lowerKey)
            {
                StoryVariable v = storyVariables[i];
                v.value = value.Trim();
                storyVariables[i] = v;
                return;
            }
        }
        
        storyVariables.Add(new StoryVariable { key = key.Trim(), value = value.Trim() });
    }

    /// <summary>
    /// Obtiene el valor guardado de una variable, o un string vacio si la clave no existe.
    /// </summary>
    public string GetVariable(string key)
    {
        if (string.IsNullOrEmpty(key)) return string.Empty;
        string lowerKey = key.Trim().ToLower();

        foreach (var v in storyVariables)
        {
            if (v.key.Trim().ToLower() == lowerKey) return v.value;
        }
        return string.Empty;
    }

    /// <summary>
    /// Reinicia todo el estado a limpio (Util para un nuevo juego).
    /// </summary>
    public void ClearState()
    {
        currentSceneName = "";
        previousSceneName = "";
        playerPosition = Vector3.zero;
        unlockedFlags.Clear();
        storyVariables.Clear();
    }
}
