using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTagProcessor : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterPortraitData portraitData;

    // Evento para separar la logica de parseo de la interfaz visual
    public event Action<string> OnPortraitAnimationChanged;
    
    // Evento para avisar qué personaje está hablando (ej: "sophia", para adaptar su voz)
    public event Action<string> OnCharacterSpeaking;

    // Evento para reproducir un sonido especial (ej: tag #sonido: joseph_suspira)
    public event Action<string> OnSoundRequested;


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
                        
                    case "deleteflag":
                        if (GameManager.Instance != null)
                        {
                            // Elimina un flag dejándolo en falso
                            GameManager.Instance.SetStoryFlag(tagValue, false);
                            Debug.Log($"[Ink] Decisión eliminada del GameState (borrada): {tagValue}");
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
                        
                    case "sonido":
                        OnSoundRequested?.Invoke(tagValue.ToLower());
                        break;
                        
                    case "scene":
                        if (LevelManager.Instance != null)
                        {
                            LevelManager.Instance.ChangeScene(tagValue);
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
        if (string.IsNullOrEmpty(spriteId))
        {
            Debug.LogWarning("[DialogueTagProcessor] Se recibió un tag #sprite sin valor o vacío.");
            return;
        }

        Debug.Log($"[DialogueTagProcessor] Procesando ID de retrato: {spriteId}");

        // 1. Extraer nombre base del personaje (e.g. "sophia" de "sophia_happy")
        string[] parts = spriteId.Split('_');
        if (parts.Length > 0)
        {
            OnCharacterSpeaking?.Invoke(parts[0]);
        }

        // 2. Procesar el retrato visual
        if (portraitData == null)
        {
            Debug.LogWarning("[DialogueTagProcessor] No hay CharacterPortraitData asignado en el Inspector.");
            return;
        }

        string animState = portraitData.GetAnimationState(spriteId);
        
        // Solo enviamos el cambio si realmente devolvió un estado de animación válido.
        // Esto evita que tags mal configurados limpien el retrato por error.
        if (!string.IsNullOrEmpty(animState))
        {
            OnPortraitAnimationChanged?.Invoke(animState);
        }
        else
        {
            Debug.LogWarning($"[DialogueTagProcessor] El ID '{spriteId}' no tiene una animación asignada en el ScriptableObject.");
        }
    }
}
