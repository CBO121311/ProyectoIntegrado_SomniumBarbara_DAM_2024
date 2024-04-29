using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    [SerializeField] private Animator portalAnimator;
    [SerializeField] private GameObject panelDescription;
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
            portalAnimator.SetBool("Player", true);

            if (InputManager.GetInstance().GetSubmitPressed())
            {
                panelDescription.SetActive(true);
                panelDescription.transform.position = new Vector2(400,300);
                //DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                //SceneManager.LoadScene(2);
            }
        }
        else
        {
            portalAnimator.SetBool("Player", false);
            panelDescription.SetActive(false);
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
