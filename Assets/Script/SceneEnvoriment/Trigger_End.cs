using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_End : MonoBehaviour
{

    [Header("Player")]
    private bool playerInRange;
    private ControllerPlayer_TopDown controllerPlayer;
    [SerializeField] private GameObject end_panel;


    private void Awake()
    {
        playerInRange = false;

        controllerPlayer = FindFirstObjectByType<ControllerPlayer_TopDown>();
        if (controllerPlayer == null)
        {
            Debug.LogError("ControllerPlayer_TopDown no encontrado en la escena.");
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (!UIManager_TopDown.GetInstance().gameIsPaused && InputManager.GetInstance().GetSubmitPressed() 
                && !UIManager_TopDown.GetInstance().IsPauseCooldownActive())
            {
                controllerPlayer.DisableSpeechBubble();
                end_panel.SetActive(true);
            }
            else
            {
                controllerPlayer.ActivateSpeechBubble();
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
            controllerPlayer.DisableSpeechBubble();
            playerInRange = false;
            end_panel.SetActive(false);
        }
    }
}
