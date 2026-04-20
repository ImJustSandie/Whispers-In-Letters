using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PortraitEntry
{
    public string id;
    public string animationStateName;
}

[CreateAssetMenu(fileName = "NewCharacterPortraitData", menuName = "Narrative/Character Portrait Data")]
public class CharacterPortraitData : ScriptableObject
{
    [SerializeField] private List<PortraitEntry> portraits = new List<PortraitEntry>();

    private Dictionary<string, string> portraitDictionary;

    public string GetAnimationState(string id)
    {
        if (string.IsNullOrEmpty(id)) return string.Empty;

        if (portraitDictionary == null || portraitDictionary.Count != portraits.Count)
        {
            portraitDictionary = new Dictionary<string, string>();
            foreach (var entry in portraits)
            {
                if (string.IsNullOrEmpty(entry.id)) continue;

                string cleanId = entry.id.Trim().ToLower();
                if (!portraitDictionary.ContainsKey(cleanId))
                {
                    portraitDictionary.Add(cleanId, entry.animationStateName.Trim());
                }
            }
        }

        // Buscamos usando el id limpiado
        if (portraitDictionary.TryGetValue(id.Trim().ToLower(), out string animState))
        {
            return animState;
        }

        Debug.LogWarning($"[CharacterPortraitData] No se encontro nombre de animacion con id: {id}");
        return string.Empty;
    }
}
