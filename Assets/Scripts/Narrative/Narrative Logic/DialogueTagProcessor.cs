using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTagProcessor : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterPortraitData portraitData;
    [SerializeField] private NarrativeImageDatabase imageDatabase;

    public NarrativeImageDatabase ImageDatabase => imageDatabase;

    // Evento para separar la logica de parseo de la interfaz visual
    public event Action<string> OnPortraitAnimationChanged;
    
    // Evento para avisar qué personaje está hablando (ej: "sophia", para adaptar su voz)
    public event Action<string> OnCharacterSpeaking;

    // Evento para reproducir un sonido especial (ej: tag #sonido: joseph_suspira)
    public event Action<string> OnSoundRequested;

    // Eventos para imagenes
    public event Action<List<string>> OnSmallImageRequested;
    public event Action<string> OnBigImageRequested;

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

                        }
                        break;
                        
                    case "deleteflag":
                        if (GameManager.Instance != null)
                        {
                            // Elimina un flag dejándolo en falso
                            GameManager.Instance.SetStoryFlag(tagValue, false);

                        }
                        break;
                        
                    case "setvar":
                        if (GameManager.Instance != null && splitTag.Length >= 3)
                        {
                            string varKey = splitTag[1].Trim();
                            string varVal = splitTag[2].Trim();
                            
                            GameManager.Instance.SetStoryVariable(varKey, varVal);

                        }
                        break;
                        
                    case "incrementvar":
                        if (GameManager.Instance != null)
                        {
                            int amount = 1;
                            if (splitTag.Length >= 3) int.TryParse(splitTag[2].Trim(), out amount);
                            int newVal = GameManager.Instance.IncrementStoryVariable(tagValue, amount);

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

                    case "small_image":
                        HandleSmallImageTag(tagValue);
                        break;

                    case "big_image":
                        HandleBigImageTag(tagValue);
                        break;
                }
            }
            else if (cleanTag.ToLower() == "fade_out")
            {
                PendingFadeOut = true;
            }
        }
    }

    /// <summary>
    /// Comprueba si la lista de tags contiene keep_image.
    /// Se llama antes de ProcessTags para decidir si mantener o limpiar imagenes.
    /// </summary>
    public bool HasKeepImageTag(List<string> currentTags)
    {
        if (currentTags == null || currentTags.Count == 0) return false;

        foreach (string tag in currentTags)
        {
            string cleanTag = tag.Trim().ToLower();
            if (cleanTag == "keep_image")
                return true;
        }
        return false;
    }

    private void HandleSmallImageTag(string tagValue)
    {
        if (string.IsNullOrEmpty(tagValue))
        {

            return;
        }

        string[] ids = tagValue.Split(',');
        List<string> imageIds = new List<string>();

        foreach (string id in ids)
        {
            string trimmed = id.Trim();
            if (!string.IsNullOrEmpty(trimmed))
                imageIds.Add(trimmed);
        }

        if (imageIds.Count > 3)
        {

            imageIds = imageIds.GetRange(0, 3);
        }

        OnSmallImageRequested?.Invoke(imageIds);
    }

    private void HandleBigImageTag(string tagValue)
    {
        if (string.IsNullOrEmpty(tagValue))
        {

            return;
        }

        OnBigImageRequested?.Invoke(tagValue);
    }

    private void HandleSpriteTag(string spriteId)
    {
        if (string.IsNullOrEmpty(spriteId))
        {

            return;
        }



        // 1. Extraer nombre base del personaje (e.g. "sophia" de "sophia_happy")
        string[] parts = spriteId.Split('_');
        if (parts.Length > 0)
        {
            OnCharacterSpeaking?.Invoke(parts[0]);
        }

        // 2. Procesar el retrato visual
        if (portraitData == null)
        {

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

        }
    }
}
