using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTagProcessor : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterPortraitData portraitData;

    // Evento para separar la logica de parseo de la interfaz visual
    public event Action<Sprite> OnPortraitSpriteChanged;
    
    public bool PendingFadeOut { get; set; }

    private const string SPRITE_TAG = "sprite";

    /// <summary>
    /// Recibe la lista de tags de la linea actual y la procesa.
    /// </summary>
    public void ProcessTags(List<string> currentTags)
    {
        if (currentTags == null || currentTags.Count == 0) return;

        foreach (string tag in currentTags)
        {
            string cleanTag = tag.Trim();
            
            // Un tag de Ink tipicamente se ve como "#sprite:sophia_happy", lo spliteamos
            string[] splitTag = cleanTag.Split(':');
            
            if (splitTag.Length >= 2)
            {
                string tagKey = splitTag[0].Trim().ToLower(); // "sprite", "setflag", etc.
                string tagValue = splitTag[1].Trim(); // "sophia_happy", "Ruta Terminada", etc.

                switch (tagKey)
                {
                    case SPRITE_TAG:
                        HandleSpriteTag(tagValue.ToLower());
                        break;
                    
                    case "setflag":
                        if (GameManager.Instance != null)
                        {
                            // Si el tag es #setflag:conocio_sophia, guardamos "conocio_sophia" en el SO
                            GameManager.Instance.SetStoryFlag(tagValue, true);
                            Debug.Log($"[Ink] Nueva decisión registrada en el GameState: {tagValue}");
                        }
                        break;
                        
                    case "setvar":
                        if (GameManager.Instance != null && splitTag.Length >= 3)
                        {
                            string varKey = splitTag[1].Trim();
                            string varVal = splitTag[2].Trim();
                            
                            GameManager.Instance.SetStoryVariable(varKey, varVal);
                            Debug.Log($"[Ink] Variable mutada registrada: {varKey} = {varVal}");
                        }
                        break;

                    case "fade_out":
                        PendingFadeOut = true;
                        break;
                }
            }
            else if (cleanTag.ToLower() == "fade_out")
            {
                PendingFadeOut = true;
            }
        }
    }

    private void HandleSpriteTag(string spriteId)
    {
        if (portraitData == null)
        {
            Debug.LogWarning("[DialogueTagProcessor] No hay CharacterPortraitData asignado en el Inspector.");
            return;
        }

        Sprite sprite = portraitData.GetPortrait(spriteId);
        
        // Disparamos el evento pasando la imagen obtenida. 
        // Asi el UI Controller la escucha sin que este script este forzado a usar librerias de UI.
        OnPortraitSpriteChanged?.Invoke(sprite);
    }
}
