using UnityEngine;

/// <summary>
/// Contiene constantes para las claves de PlayerPrefs, centralizando su uso.
/// </summary>
public static class PlayerPrefsKeys
{
    public const string ENDING_PREFIX = "EndingUnlocked_";

    public static string EndingKey(string philosopherKey) 
    {
        return ENDING_PREFIX + philosopherKey;
    }
}
