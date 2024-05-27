using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Señal Visual")]
    [SerializeField] private GameObject speechBubble;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        speechBubble.SetActive(false);
    }

    private void Update()
    {
        //Activar de nuevo el dialogo hasta que finalice
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            speechBubble.SetActive(true);
            if (!UIManager_SelectionLevel.GetInstance().inventoryIsActivated && !UIManager_SelectionLevel.GetInstance().gameIsPaused 
                && InputManager.GetInstance().GetSubmitPressed())
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            speechBubble.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
