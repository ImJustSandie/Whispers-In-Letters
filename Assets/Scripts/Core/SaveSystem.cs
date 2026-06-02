using System.IO;
using UnityEngine;

/// <summary>
/// Servicio estático responsable EXCLUSIVAMENTE de leer y escribir el archivo de guardado en disco o caché local.
/// 
/// REGLA DE ARQUITECTURA:
///   - Solo GameManager debe llamar a este servicio.
///   - Ningún otro sistema (LevelManager, StoryManager, UI, etc.) debe invocar SaveSystem directamente.
///   - No contiene lógica de negocio del juego; solo serialización/deserialización.
/// </summary>
public static class SaveSystem
{
    private const string SAVE_FILE_NAME = "save.json";
    private const string PREFS_SAVE_KEY = "GameSaveData_JSON";

    /// <summary>Ruta completa del archivo de guardado en el dispositivo del jugador (Standalone).</summary>
    private static string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    // ─────────────────────────────────────────────────────────────────────────
    // API Pública
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Devuelve true si existe un archivo de guardado válido.
    /// </summary>
    public static bool HasSave()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return PlayerPrefs.HasKey(PREFS_SAVE_KEY);
#else
        return File.Exists(SavePath);
#endif
    }

    /// <summary>
    /// Serializa el GameSaveData a JSON y lo escribe en disco o caché.
    /// Debe llamarse solo desde GameManager.
    /// </summary>
    public static void Save(GameSaveData data)
    {
        if (data == null)
        {

            return;
        }

        try
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);

#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString(PREFS_SAVE_KEY, json);
            PlayerPrefs.Save(); // Asegura de que se guarde en IndexedDB (caché del navegador)

#else
            File.WriteAllText(SavePath, json);

#endif
        }
        catch (System.Exception e)
        {

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

            return null;
        }

        try
        {
            string json;
#if UNITY_WEBGL && !UNITY_EDITOR
            json = PlayerPrefs.GetString(PREFS_SAVE_KEY);

#else
            json = File.ReadAllText(SavePath);

#endif
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            if (data == null)
            {

                return null;
            }

            return data;
        }
        catch (System.Exception e)
        {

            return null;
        }
    }

    /// <summary>
    /// Elimina el archivo de guardado.
    /// Se usa al iniciar una nueva partida.
    /// </summary>
    public static void DeleteSave()
    {
        if (!HasSave())
        {

            return;
        }

        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.DeleteKey(PREFS_SAVE_KEY);
            PlayerPrefs.Save();

#else
            File.Delete(SavePath);

#endif
        }
        catch (System.Exception e)
        {

        }
    }
}
