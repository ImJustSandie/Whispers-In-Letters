using System;
using System.Collections.Generic;

/// <summary>
/// Estructura plana serializable que representa el estado del juego guardado en disco.
/// No contiene ninguna dependencia de Unity para garantizar su correcta serialización con JsonUtility.
/// 
/// REGLA: Solo GameManager debe instanciar y modificar este objeto.
///        Ningún otro sistema debe acceder a GameSaveData directamente.
/// </summary>
[Serializable]
public class GameSaveData
{
    /// <summary>La última escena en la que estuvo el jugador.</summary>
    public string lastSceneName;

    /// <summary>La escena desde la que llegó a la última escena (para SpawnPoint correcto).</summary>
    public string previousSceneName;

    /// <summary>Lista de flags narrativos activados (decisiones, eventos ocurridos).</summary>
    public List<string> unlockedFlags = new List<string>();

    /// <summary>Variables narrativas clave-valor (ej. "actitud_joseph" = "motivado").</summary>
    public List<StoryVariableData> storyVariables = new List<StoryVariableData>();
}

/// <summary>
/// Par clave-valor serializable para variables narrativas.
/// Equivalente plano a GameStateSO.StoryVariable, sin dependencias de Unity.
/// </summary>
[Serializable]
public class StoryVariableData
{
    public string key;
    public string value;
}
