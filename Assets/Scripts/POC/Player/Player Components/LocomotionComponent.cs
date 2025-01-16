using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LocomotionComponent : PlayerComponent
{
    public InputActionReference moveActionReference;
    public InputActionReference lookActionReference;
    public InputActionReference sprintReference;
    public InputActionReference jumpReference;

    public CharacterController characterController;

    public bool controllerInput = false;
    public bool moving = false;
    public bool isGrounded;

    public Vector2 vec;
    public Vector2 lookVec;
    public bool sprinting;

    public float playerSpeed = 3;
    public float sprintSpeed = 10;
    public float normalSpeed = 3;

    [SerializeField] private Transform playerCamera; // Reference to the camera
    [SerializeField] private float sensitivity = 100f; // Mouse sensitivity
    private float pitch = 0f; // Vertical rotation

    public Transform groundCheck; // Reference to a ground check object
    public float groundDistance = 0.4f; // Radius of the ground check sphere
    public LayerMask groundMask; // Layer to identify what counts as "ground"
    

    private Vector3 velocity; // Stores the vertical velocity
    [SerializeField] private float gravity = -9.81f; // Gravity value
    [SerializeField] private float jumpHeight = 2f; // Jump height

    public override void EnableComponent(Player player)
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerSpeed = normalSpeed;

        base.EnableComponent(player);

        characterController = GetComponent<CharacterController>();

        if (controllerInput)
        {
            moveActionReference.action.performed += PlayerMoveController;
        }
        else
        {
            moveActionReference.action.performed += PlayerMoveKeyboard;
        }

        moveActionReference.action.canceled += Stopped;
        lookActionReference.action.performed += Look;
        sprintReference.action.started += Sprint;
        sprintReference.action.canceled += Sprint;
        jumpReference.action.started += Jump;
    }

    public override void DisableComponent()
    {
        base.DisableComponent();

        characterController = null;

        if (controllerInput)
        {
            moveActionReference.action.performed -= PlayerMoveController;
        }
        else
        {
            moveActionReference.action.performed -= PlayerMoveKeyboard;
        }

        moveActionReference.action.canceled -= Stopped;
        lookActionReference.action.performed -= Look;

        sprintReference.action.started -= Sprint;
        sprintReference.action.canceled -= Sprint;
        jumpReference.action.started -= Jump;
    }

    void PlayerMoveController(InputAction.CallbackContext context)
    {
        moving = true;
        Vector2 moveVec = context.ReadValue<Vector2>();

        characterController.Move(new Vector3(moveVec.x, 0, moveVec.y));
    }

    void PlayerMoveKeyboard(InputAction.CallbackContext context)
    {
        moving = true;
        Vector2 moveVec = context.ReadValue<Vector2>();

        vec = moveVec;
    }

    void Stopped(InputAction.CallbackContext context)
    {
        moving = false;
        vec = Vector3.zero;
    }

    void Sprint(InputAction.CallbackContext context)
    {
        float inp = context.ReadValue<float>();
        if (inp == 1)
        {
            sprinting = true;
            playerSpeed = sprintSpeed;
        }
        else
        {
            sprinting = false;
            playerSpeed = normalSpeed;
        }
    }

    void Look(InputAction.CallbackContext context)
    {
        lookVec = context.ReadValue<Vector2>();

        float mouseX = lookVec.x * sensitivity * Time.deltaTime;
        float mouseY = lookVec.y * sensitivity * Time.deltaTime;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensure the character stays grounded
        }

        if (!controllerInput && moving)
        {
            Vector3 move = transform.right * vec.x + transform.forward * vec.y;
            characterController.Move(move * playerSpeed * Time.deltaTime);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the character vertically
        characterController.Move(velocity * Time.deltaTime);
    }
}
