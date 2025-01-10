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
    public Vector3 velocity;
    public bool sprinting;



    public float playerSpeed = 3;
    public float sprintSpeed = 10;
    public float normalSpeed = 3;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [SerializeField] private Transform playerCamera; // Reference to the camera
    [SerializeField] private float sensitivity = 100f; // Mouse sensitivity
    private float pitch = 0f; // Vertical rotation

    public Transform groundCheck; // Reference to a ground check object
    public float groundDistance = 0.4f; // Radius of the ground check sphere
    public LayerMask groundMask; // Layer to identify what counts as "ground"


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

        jumpReference.action.performed += Jump;
        jumpReference.action.canceled += Jump;

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
        jumpReference.action.performed -= Jump;
        jumpReference.action.canceled -= Jump;
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

        // Adjust pitch (vertical rotation) and clamp it
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Apply pitch to the camera
        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Rotate the player capsule for yaw (horizontal rotation)
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump(InputAction.CallbackContext context)
    {
        
        
        if (context.performed && isGrounded && velocity.y < 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
    }


    void GroundCheck()
    {
        characterController = GetComponent<CharacterController>();
        if (groundCheck == null)
        {
            Debug.LogWarning("GroundCheck transform is not assigned. Please set it in the Inspector.");
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (!controllerInput && moving)
        {
            Vector3 move = transform.right * vec.x + transform.forward * vec.y;
            characterController.Move(move * playerSpeed * Time.deltaTime);
            //characterController.Move(new Vector3(vec.x, 0, vec.y) * speed * Time.deltaTime);
            
        }
        
    }
}
