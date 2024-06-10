using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private ControllerPlayerBed controllerPlayerBed;

    private void Awake()
    {
        playerInRange = false;

        controllerPlayerBed = FindFirstObjectByType<ControllerPlayerBed>();
        if (controllerPlayerBed == null)
        {
            Debug.LogError("ControllerPlayerBed no encontrado en la escena.");
        }
    }


    private void Update()
    {
        //Activar de nuevo el dialogo hasta que finalice
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            
            if (!UIManager_Bedroom.GetInstance().gameIsPaused
                && InputManager.GetInstance().GetSubmitPressed())
            {
                controllerPlayerBed.DisableSpeechBubble();
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
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
            controllerPlayerBed.DisableSpeechBubble();
            playerInRange = false;
        }
    }
}
