using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    private ControllerPlayer_TopDown controllerPlayerBed;

    private bool playerInRange = false;
    private UIManager_TopDown uiManager;

    private void Start()
    {
        uiManager = UIManager_TopDown.GetInstance();

        controllerPlayerBed = FindFirstObjectByType<ControllerPlayer_TopDown>();
        if (controllerPlayerBed == null)
        {
            Debug.LogError("ControllerPlayerBed no encontrado en la escena.");
        }
    }

    private void Update()
    {
        if (playerInRange && !uiManager.gameIsPaused)
        {
            if (!uiManager.inventoryIsActivated && InputManager.GetInstance().GetSubmitPressed())
            {
                uiManager.ToggleInventoryUI();
                controllerPlayerBed.DisableSpeechBubble();
            } else if (uiManager.inventoryIsActivated && InputManager.GetInstance().GetExitPressed())
            {
                uiManager.ToggleInventoryUI();
            }
            else
            {
                controllerPlayerBed.ActivateSpeechBubble();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            controllerPlayerBed.DisableSpeechBubble();
        }
    }
}
