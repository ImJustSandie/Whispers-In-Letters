using System.IO;
using UnityEngine;

/// <summary>
/// Servicio estático responsable EXCLUSIVAMENTE de leer y escribir el archivo de guardado en disco.
/// 
/// REGLA DE ARQUITECTURA:
///   - Solo GameManager debe llamar a este servicio.
///   - Ningún otro sistema (LevelManager, StoryManager, UI, etc.) debe invocar SaveSystem directamente.
///   - No contiene lógica de negocio del juego; solo serialización/deserialización.
/// </summary>
public static class SaveSystem
{
    private const string SAVE_FILE_NAME = "save.json";

    /// <summary>Ruta completa del archivo de guardado en el dispositivo del jugador.</summary>
    private static string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    // ─────────────────────────────────────────────────────────────────────────
    // API Pública
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Devuelve true si existe un archivo de guardado válido en disco.
    /// </summary>
    public static bool HasSave()
    {
        return File.Exists(SavePath);
    }

    /// <summary>
    /// Serializa el GameSaveData a JSON y lo escribe en disco.
    /// Debe llamarse solo desde GameManager.
    /// </summary>
    public static void Save(GameSaveData data)
    {
        if (data == null)
        {
            Debug.LogWarning("[SaveSystem] Intentando guardar datos nulos. Operación abortada.");
            return;
        }

        try
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"[SaveSystem] Juego guardado en: {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveSystem] Error al guardar: {e.Message}");
        }
    }

    /// <summary>
    /// Lee el archivo de guardado y devuelve un GameSaveData deserializado.
    /// Devuelve null si el archivo no existe o está corrupto.
    /// </summary>
    public static GameSaveData Load()
    {
        if (!HasSave())
        {
            Debug.Log("[SaveSystem] No se encontró archivo de guardado.");
            return null;
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            if (data == null)
            {
                Debug.LogWarning("[SaveSystem] El archivo de guardado está vacío o malformado.");
                return null;
            }

            Debug.Log($"[SaveSystem] Guardado cargado desde: {SavePath}");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveSystem] Error al cargar el guardado (posiblemente corrupto): {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Elimina el archivo de guardado del disco.
    /// Se usa al iniciar una nueva partida.
    /// </summary>
    public static void DeleteSave()
    {
        if (!HasSave())
        {
            Debug.Log("[SaveSystem] No hay archivo de guardado que eliminar.");
            return;
        }

        try
        {
            File.Delete(SavePath);
            Debug.Log("[SaveSystem] Archivo de guardado eliminado.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveSystem] Error al eliminar el guardado: {e.Message}");
        }
    }
}
