using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool handleSpawning = true;

    [Header("Player Reference")]
    [Tooltip("Si se deja vacío, el script buscará automáticamente al jugador por Tag 'Player'.")]
    [SerializeField] private GameObject playerObject;

    void Start()
    {
        if (handleSpawning)
        {
            HandlePlayerSpawn();
        }
    }

    void Update()
    {

    }

    public void movement()
    {

    }

    /// <summary>
    /// Busca un SpawnPoint que coincida con la escena de donde viene el jugador.
    /// </summary>
    private void HandlePlayerSpawn()
    {
        // 1. Verificar que el GameManager exista
        if (GameManager.Instance == null || GameManager.Instance.GetGameState() == null)
        {
            Debug.Log("[PlayerManager] No hay GameManager o GameState. Spawn cancelado.");
            return;
        }

        string previousScene = GameManager.Instance.GetGameState().previousSceneName;
        Debug.Log($"[PlayerManager] Escena anterior registrada: '{previousScene}'");

        if (string.IsNullOrEmpty(previousScene))
        {
            Debug.Log("[PlayerManager] No hay escena anterior (primera carga del juego). Posicion base mantenida.");
            return;
        }

        // 2. Obtener la referencia al jugador
        GameObject player = GetPlayerReference();
        if (player == null)
        {
            Debug.LogWarning("[PlayerManager] No se encontro al jugador. Asegurate de asignar la referencia o de que tenga el Tag 'Player'.");
            return;
        }

        // 3. Buscar todos los SpawnPoints en la escena
        SpawnPoint[] spawnPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        Debug.Log($"[PlayerManager] Se encontraron {spawnPoints.Length} SpawnPoint(s) en la escena.");

        foreach (SpawnPoint sp in spawnPoints)
        {
            Debug.Log($"[PlayerManager] Comparando SpawnPoint '{sp.fromSceneName}' con escena anterior '{previousScene}'");

            // Comparacion robusta: sin importar mayusculas ni espacios
            if (sp.fromSceneName.Trim().Equals(previousScene.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                TeleportPlayer(player, sp.transform.position, sp.transform.rotation);
                Debug.Log($"[PlayerManager] Jugador spawneado en SpawnPoint correspondiente a: '{previousScene}'");
                return;
            }
        }

        Debug.LogWarning($"[PlayerManager] No se encontro SpawnPoint con fromSceneName='{previousScene}'. Posicion base mantenida.");
    }

    /// <summary>
    /// Obtiene la referencia al jugador, ya sea por la asignacion manual o por Tag.
    /// </summary>
    private GameObject GetPlayerReference()
    {
        if (playerObject != null)
            return playerObject;

        // Fallback: buscar por Tag
        playerObject = GameObject.FindWithTag("Player");
        return playerObject;
    }

    /// <summary>
    /// Teletransporta al jugador desactivando temporalmente el CharacterController si existe.
    /// </summary>
    private void TeleportPlayer(GameObject player, Vector3 position, Quaternion rotation)
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null) cc.enabled = false;

        player.transform.position = position;
        player.transform.rotation = rotation;

        if (cc != null) cc.enabled = true;
    }
}
