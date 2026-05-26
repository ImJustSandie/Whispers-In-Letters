using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransitionCondition
{
    [Tooltip("El flag que se requiere para permitir la transición.")]
    public string requiredFlag;
    [Tooltip("El nudo de Ink que se reproducirá si este flag NO está presente.")]
    public string fallbackKnot;
}

[RequireComponent(typeof(Collider))] // O Collider2D si estas trabajando en 2D
public class SceneTransitionTrigger : MonoBehaviour
{
    [Tooltip("El nombre exacto de la escena a la que quieres viajar al tocar este objeto.")]
    public string destinationSceneName;

    [Tooltip("Asegurate de que Sophia tenga el 'Tag' correcto configurado en el Inspector.")]
    public string playerTag = "Player";

    [Tooltip("Opcional: Si escribes un flag aquí (ej: 'Ruta Terminada'), la transición solo funcionará si el jugador tiene este flag activo.")]
    public string requiredFlag = "";

    [Tooltip("Opcional: Nudo de Ink que se reproducirá si el jugador intenta cruzar pero no tiene el flag requerido.")]
    public string fallbackKnot = "";

    [Tooltip("Opcional: Nudo de Ink de confirmación. Si tiene un valor y se cumplen las condiciones, se reproducirá este nudo en lugar de cambiar de escena instantáneamente. Para efectuar el viaje, el script de Ink deberá usar el tag #scene:nombre_escena.")]
    public string confirmationKnot = "";

    [Header("Visual Feedback")]
    [Tooltip("Distancia que se empuja al jugador hacia atrás si la entrada está bloqueada.")]
    [SerializeField] private float pushDistance = 1.2f;

    [Header("Condiciones Adicionales (Pila/Secuencia)")]
    [Tooltip("Lista de condiciones evaluadas en orden. La primera que falle detendrá la transición.")]
    public List<TransitionCondition> conditions = new List<TransitionCondition>();

    private bool isTransitioning = false;
    private bool isPushingBack = false;

    // Si tu juego es 3D usa OnTriggerEnter, si es 2D usa OnTriggerEnter2D
    private void OnTriggerEnter(Collider other)
    {
        // Revisamos si quien toco la puerta fue realmente el jugador y si no estamos ya cargando
        if (other.CompareTag(playerTag) && !isTransitioning)
        {
            // 1. Evaluar condición individual inicial (Retrocompatibilidad)
            if (!string.IsNullOrEmpty(requiredFlag))
            {
                if (GameManager.Instance == null || !GameManager.Instance.GetStoryFlag(requiredFlag))
                {
                    HandleAccessDenied(requiredFlag, fallbackKnot);
                    if (!isPushingBack) StartCoroutine(NaturalPushBackRoutine(other.gameObject));
                    return;
                }
            }

            // 2. Evaluar lista de condiciones secuenciales
            foreach (var condition in conditions)
            {
                if (GameManager.Instance == null || !GameManager.Instance.GetStoryFlag(condition.requiredFlag))
                {
                    HandleAccessDenied(condition.requiredFlag, condition.fallbackKnot);
                    if (!isPushingBack) StartCoroutine(NaturalPushBackRoutine(other.gameObject));
                    return;
                }
            }

            // 3. Si todas las condiciones pasaron, verificar diálogo de confirmación
            if (!string.IsNullOrEmpty(confirmationKnot))
            {
                Debug.Log($"[SceneTransition] Iniciando diálogo de confirmación: {confirmationKnot}");
                if (StoryManager.Instance != null)
                {
                    StoryManager.Instance.StartStory(confirmationKnot);
                    if (!isPushingBack) StartCoroutine(NaturalPushBackRoutine(other.gameObject));
                }
                return;
            }

            // 4. Si no hay confirmación, cambiar escena directamente
            if (LevelManager.Instance != null && !string.IsNullOrEmpty(destinationSceneName))
            {
                isTransitioning = true;
                Debug.Log($"[SceneTransition] Sophia toco la zona. Viajando hacia: {destinationSceneName}");
                LevelManager.Instance.ChangeScene(destinationSceneName);
            }
            else
            {
                Debug.LogWarning("[SceneTransition] No hay un LevelManager instanciado o la escena de destino esta en blanco.");
            }
        }
    }

    private void HandleAccessDenied(string flag, string knot)
    {
        Debug.Log($"[SceneTransition] Acceso denegado. Falta el flag: {flag}");
        if (!string.IsNullOrEmpty(knot) && StoryManager.Instance != null)
        {
            StoryManager.Instance.StartStory(knot);
        }
    }

    /// <summary>
    /// Espera a que el diálogo termine y mueve suavemente al jugador en dirección opuesta.
    /// Esto imita el movimiento natural (giro + caminata) en lugar de un salto instantáneo.
    /// </summary>
    private IEnumerator NaturalPushBackRoutine(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc == null) yield break;

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        isPushingBack = true;

        // 1. Esperar un breve momento para que StoryManager registre el diálogo activo
        yield return new WaitForSeconds(0.1f);

        // 2. Esperar a que el jugador cierre el diálogo (Sophia no se mueve mientras habla)
        while (StoryManager.Instance != null && StoryManager.Instance.IsDialogueActive)
        {
            yield return null;
        }

        // 3. Iniciar movimiento de retroceso natural
        Vector3 pushDir = (player.transform.position - transform.position);
        pushDir.y = 0;
        if (pushDir.sqrMagnitude < 0.01f) pushDir = -transform.forward; 
        pushDir.Normalize();

        float walkSpeed = 3.5f; 
        float rotSpeed = 10f;
        
        // RESTRICCIÓN DE SEGURIDAD: Usamos tiempo en lugar de distancia para evitar bucles infinitos en esquinas/paredes
        float pushDuration = 0.5f; 
        float timer = 0f;

        while (timer < pushDuration)
        {
            timer += Time.deltaTime;

            // Girar suavemente hacia la dirección de retroceso
            Quaternion targetRotation = Quaternion.LookRotation(pushDir);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            // Mover hacia el punto de destino
            cc.Move(pushDir * walkSpeed * Time.deltaTime);
            
            yield return null;
        }

        if (pm != null) pm.enabled = true;
        isPushingBack = false;
        Debug.Log("[SceneTransition] Retroceso natural completado después del diálogo.");
    }
    
    // Descomenta esto y borra la funcion de arriba si tu juego es estrictamente 2D
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !isTransitioning)
        {
            // Evaluación de condiciones secuenciales
            if (!string.IsNullOrEmpty(requiredFlag) && (GameManager.Instance == null || !GameManager.Instance.GetStoryFlag(requiredFlag)))
            {
                HandleAccessDenied(requiredFlag, fallbackKnot);
                return;
            }

            foreach (var condition in conditions)
            {
                if (GameManager.Instance == null || !GameManager.Instance.GetStoryFlag(condition.requiredFlag))
                {
                    HandleAccessDenied(condition.requiredFlag, condition.fallbackKnot);
                    return;
                }
            }

            if (LevelManager.Instance != null && !string.IsNullOrEmpty(destinationSceneName))
            {
                isTransitioning = true;
                LevelManager.Instance.ChangeScene(destinationSceneName);
            }
        }
    }
    */
}
