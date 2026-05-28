using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PhilosopherCardDatabase", menuName = "Narrative/Philosopher Card Database")]
public class PhilosopherCardDatabase : ScriptableObject
{
    [Serializable]
    public class PhilosopherCardEntry
    {
        public string philosopherKey;   // e.g., "schopenhauer"
        public string rutaValue;        // valor exacto en Ink: "Epilogo1"
        public string acceptanceKnot;   // "epilogo_schopenhauer"
        public string reprocheKnot;     // "epilogo_schopenhauer_reproche"
        public string reflectionKnot;   // "reflexion_schopenhauer"
        public Sprite cardSprite;         // retrato del filósofo
        public Sprite silhouetteSprite;   // silueta del filósofo (para cartas de rechazo)
        public Sprite backgroundSprite;   // fondo del panel de carta
        public string displayName;        // "Schopenhauer"
    }

    public List<PhilosopherCardEntry> entries = new List<PhilosopherCardEntry>();

    /// <summary>
    /// Busca una entrada por el valor de la variable "ruta" de Ink (case-insensitive).
    /// </summary>
    public PhilosopherCardEntry GetByRuta(string ruta)
    {
        if (string.IsNullOrEmpty(ruta)) 
        {

            return null;
        }

        string trimmedRuta = ruta.Trim();
        var found = entries.Find(e => e.rutaValue.Trim().Equals(trimmedRuta, StringComparison.OrdinalIgnoreCase));
        
        if (found == null)
        {

        }

        return found;
    }

    /// <summary>
    /// Obtiene todos los filósofos que NO coinciden con la ruta dada.
    /// </summary>
    public List<PhilosopherCardEntry> GetAllExcept(string ruta)
    {
        List<PhilosopherCardEntry> others = new List<PhilosopherCardEntry>();
        foreach (var entry in entries)
        {
            if (!entry.rutaValue.Equals(ruta, StringComparison.OrdinalIgnoreCase))
            {
                others.Add(entry);
            }
        }
        return others;
    }
}
