using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private bool handleSpawning = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (handleSpawning)
        {
            HandlePlayerSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movement(){

    }

    /// <summary>
    /// Busca un SpawnPoint que coincida con la escena de donde viene el jugador.
    /// </summary>
    private void HandlePlayerSpawn()
    {
        // Cancelamos si no detecta la arquitectura orquestadora
        if (GameManager.Instance == null || GameManager.Instance.GetGameState() == null)
            return;

        string previousScene = GameManager.Instance.GetGameState().previousSceneName;

        if (!string.IsNullOrEmpty(previousScene))
        {
            // Busca todos los SpawnPoints en la nueva escena
            SpawnPoint[] spawnPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

            foreach (SpawnPoint sp in spawnPoints)
            {
                if (sp.fromSceneName == previousScene)
                {
                    // Movemos al jugador a esta posicion
                    transform.position = sp.transform.position;
                    transform.rotation = sp.transform.rotation;
                    
                    Debug.Log($"[PlayerManager] Jugador spawneado desde la ubicacion correspondiente a: {previousScene}");
                    return; // Terminamos, ya lo acomodamos
                }
            }
        }
        
        Debug.Log("[PlayerManager] No se encontro SpawnPoint especifico para la escena, posicion base mantenida.");
    }
}
