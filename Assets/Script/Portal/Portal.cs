using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    [SerializeField] Animator animator;
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
    }

    private void Update()
    {
        //No podremos activar el dialogo hasta que finalice
        if (playerInRange)
        {
            animator.SetBool("Player", true);
            if (InputManager.GetInstance().GetSubmitPressed())
            {
                //DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

                
                SceneManager.LoadScene(2);
            }
        }
        else
        {
            animator.SetBool("Player", false);
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
        }
    }
}
