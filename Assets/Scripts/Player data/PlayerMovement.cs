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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
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




        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        // Rotar el modelo suavemente hacia la direccion de movimiento
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}