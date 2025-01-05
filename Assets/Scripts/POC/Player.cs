using JetBrains.Annotations;
using System.Runtime.CompilerServices;
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
            moveActionReference.action.performed += PlayerMoveKeyboard;
            moveActionReference.action.canceled += Stopped;
        }
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
        Vector2 horizontalInput = context.ReadValue<Vector2>();
        Vector2 verticalInput = context.ReadValue<Vector2>();
        float speed = 2.5f;
        new Vector3(horizontalInput, 0, verticalInput);
        

    }

    void Stopped(InputAction.CallbackContext context)
    {
        moving = false;
    }
    void Update()
    {



        // float based on the Axis of player's input
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        
        //float speed = 3.5f;

        //Vector3 direction = new Vector3(horizontalInput, 0 , verticalInput);
        //transform.Translate(direction * speed * Time.deltaTime);
    }
}
    
        
        

