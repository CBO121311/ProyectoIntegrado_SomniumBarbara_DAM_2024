using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public bool canReceiveInput;
    public bool inputReceive;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(canReceiveInput)
            {
                inputReceive = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }


    public void InputaManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput=true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
}
