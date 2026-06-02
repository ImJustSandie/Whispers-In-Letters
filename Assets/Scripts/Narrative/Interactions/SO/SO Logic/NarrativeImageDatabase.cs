using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NarrativeImageEntry
{
    public string id;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "NarrativeImageDatabase", menuName = "Narrative/Image Database")]
public class NarrativeImageDatabase : ScriptableObject
{
    [SerializeField] private List<NarrativeImageEntry> entries = new List<NarrativeImageEntry>();

    private Dictionary<string, Sprite> imageDictionary;

    private void BuildDictionary()
    {
        if (imageDictionary != null && imageDictionary.Count == entries.Count)
            return;

        imageDictionary = new Dictionary<string, Sprite>();
        foreach (var entry in entries)
        {
            if (string.IsNullOrEmpty(entry.id)) continue;

            string cleanId = entry.id.Trim().ToLower();
            if (!imageDictionary.ContainsKey(cleanId))
            {
                imageDictionary.Add(cleanId, entry.sprite);
            }
        }
    }

    public Sprite GetImage(string id)
    {
        if (string.IsNullOrEmpty(id))
        {

            return null;
        }

        BuildDictionary();

        string cleanId = id.Trim().ToLower();
        if (imageDictionary.TryGetValue(cleanId, out Sprite sprite))
        {
            return sprite;
        }


        return null;
    }
}
