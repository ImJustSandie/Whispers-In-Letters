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
    /// Reinicia todo el estado a limpio (Util para un nuevo juego).
    /// </summary>
    public void ClearState()
    {
        currentSceneName = "";
        previousSceneName = "";
        playerPosition = Vector3.zero;
        unlockedFlags.Clear();
    }
}
