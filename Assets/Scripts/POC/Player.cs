using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public InputActionReference moveActionReference;
    public CharacterController characterController;

    public bool controllerInput = false;

    public bool moving = false;

    void Start()
    {
        if (controllerInput)
        {
            moveActionReference.action.performed += PlayerMoveController;
            moveActionReference.action.canceled += Stopped;
        }
    }

    void PlayerMoveController(InputAction.CallbackContext context)
    {
        moving = true;
        Vector2 moveVec = context.ReadValue<Vector2>();
        characterController.Move(new Vector3(moveVec.x, 0, moveVec.y));
    }

    void Stopped(InputAction.CallbackContext context)
    {
        moving = false;
    }
}