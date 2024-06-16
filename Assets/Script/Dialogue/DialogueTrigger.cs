using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Player")]
    private bool playerInRange;
    private ControllerPlayer_TopDown controllerPlayer;

    [Header("NPC Sprite")]
    [SerializeField] private bool isNpc = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

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
        //Activar de nuevo el dialogo hasta que finalice
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            
            if (DialogueManager.GetInstance().CanStartDialogue() && !UIManager_TopDown.GetInstance().gameIsPaused
                && InputManager.GetInstance().GetSubmitPressed() && !UIManager_TopDown.GetInstance().IsPauseCooldownActive())
            {
                controllerPlayer.DisableSpeechBubble();
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
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
            if (isNpc)
            {
                playerTransform = collision.transform;
                FlipTowardsPlayer();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            controllerPlayer.DisableSpeechBubble();
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
