using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableDuckDatabase", menuName = "Narrative/Collectable Duck Database")]
public class CollectableDuckDatabase : ScriptableObject
{
    [Serializable]
    public class DuckEntry
    {
        public string duckName;       // Nombre para mostrar, ej. "Pato Schopenhauer"
        public string flagName;       // El nombre del flag en GameStateSO, ej. "Pato_Schopenhauer"
        public Sprite collectedSprite; // Imagen normal
        public Sprite lockedSprite;    // Imagen silueta/bloqueada
    }

    public List<DuckEntry> ducks = new List<DuckEntry>();
}
