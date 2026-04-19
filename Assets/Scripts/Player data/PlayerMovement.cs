using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;
    public Animator Animator;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        // El input se lee directamente en Update() para evitar desfase de frames
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        if (StoryManager.Instance != null && StoryManager.Instance.IsDialogueActive)
        {
            return;
        }

        if(controls.Player.Sprint.IsPressed())
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 5f;
        }




        // Leer el input en el mismo frame que se procesa el movimiento
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        // Rotar el modelo suavemente hacia la direccion de movimiento
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.Move(movement * moveSpeed * Time.deltaTime);

        if (Animator != null)
        {
            Animator.SetFloat("MovementA", moveInput.magnitude);
            Animator.SetBool("Sprint", controls.Player.Sprint.IsPressed() && moveInput.magnitude > 0.01f);
        }
    }
}