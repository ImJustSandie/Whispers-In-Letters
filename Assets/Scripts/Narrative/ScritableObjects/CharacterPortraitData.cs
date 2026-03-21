using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PortraitEntry
{
    public string id;
    public Sprite portrait;
}

[CreateAssetMenu(fileName = "NewCharacterPortraitData", menuName = "Narrative/Character Portrait Data")]
public class CharacterPortraitData : ScriptableObject
{
    [SerializeField] private List<PortraitEntry> portraits = new List<PortraitEntry>();

    private Dictionary<string, Sprite> portraitDictionary;

    public Sprite GetPortrait(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        if (portraitDictionary == null || portraitDictionary.Count != portraits.Count)
        {
            portraitDictionary = new Dictionary<string, Sprite>();
            foreach (var entry in portraits)
            {
                if (!portraitDictionary.ContainsKey(entry.id))
                {
                    portraitDictionary.Add(entry.id, entry.portrait);
                }
            }
        }

        if (portraitDictionary.TryGetValue(id.ToLower(), out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"[CharacterPortraitData] No se encontro retrato con id: {id}");
        return null; // Opcionalmente, podrias retornar un Sprite por defecto si el ID no existe
    }
}
