using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    private bool playerInRange;
    private Transform playerTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private ControllerPlayerSL controllerPlayerSL;

    private void Awake()
    {
        playerInRange = false;

        controllerPlayerSL = FindFirstObjectByType<ControllerPlayerSL>();
        if (controllerPlayerSL == null)
        {
            Debug.LogError("ControllerPlayerBed no encontrado en la escena.");
        }
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {

            if (!UIManager_Bedroom.GetInstance().gameIsPaused
                && InputManager.GetInstance().GetSubmitPressed())
            {
                controllerPlayerSL.DisableSpeechBubble();
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

            }
            else
            {
                controllerPlayerSL.ActivateSpeechBubble();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange=true;
            playerTransform = collision.transform;
            FlipTowardsPlayer();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }


    private void FlipTowardsPlayer()
    {
        if (playerTransform != null)
        {
            if (playerTransform.position.x < transform.position.x && spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
            else if (playerTransform.position.x > transform.position.x && spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
