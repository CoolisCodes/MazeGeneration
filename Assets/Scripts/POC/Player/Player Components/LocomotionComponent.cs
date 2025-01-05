using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LocomotionComponent : PlayerComponent
{
    public InputActionReference moveActionReference;
    public InputActionReference lookActionReference;
    public CharacterController characterController;

    public bool controllerInput = false;

    public bool moving = false;

    public Vector2 vec;

    public float speed = 3;


    public override void EnableComponent(Player player)
    {
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
    void Update()
    {
        if (!controllerInput && moving)
        {
            characterController.Move(new Vector3(vec.x, 0, vec.y) * speed * Time.deltaTime);
        }
    }
}
