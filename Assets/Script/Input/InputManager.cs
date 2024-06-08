using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector2 moveDirection = Vector2.zero;
    private bool interactPressed = false;
    private bool submitPressed = false;
    private bool menuPressed = false;
    private bool exitPressed = false;

    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontró más de un InputManager en la escena.");
        }
        instance = this;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
            //Debug.Log(moveDirection);
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
            //Debug.Log(moveDirection);
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }


    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menuPressed = true;
        }
        else if (context.canceled)
        {
            menuPressed = false;
        }
    }



    public void ExitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            exitPressed = true;
        }
        else if (context.canceled)
        {
            exitPressed = false;
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public bool GetInMenuPressed()
    {
        bool result = menuPressed;
        menuPressed = false;
        return result;
    }

    public bool GetExitPressed()
    {
        bool result = exitPressed;
        exitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
}
