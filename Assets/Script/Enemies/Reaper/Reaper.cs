using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool playerInRange = false;
    //public MainMenuManager mainMenuManager;
    //public PlayerRespawn pr;

    [SerializeField] public Transform jugador;
    [SerializeField] private float distancia;
    public Vector3 puntoInicial;


    void Start()
    {
        animator = GetComponent<Animator>();
        puntoInicial = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //if (UIManager.gameIsPaused) return;

        distancia = Vector2.Distance(transform.position, jugador.position);
        //animator.SetFloat("Distance", distancia);

        /**
         * 
        if (playerInRange)
        {
            animator.SetBool("Player", true);
        }
        else
        {
            animator.SetBool("Player", false);
        }*/
    }


    public void Spin(Vector3 target)
    {
        if (transform.position.x < target.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            animator.SetTrigger("Player");

            //animator.SetTrigger("Death");
            //playerInRange = true;

            Debug.Log("Encontrado");
            //pr.EndGame();
            //collision.gameObject.GetComponent<PlayerDeath>().Die();
            //Invoke("ResetPlayerInRange", 2f);
        }
    }


    private void ResetPlayerInRange()
    {
        //playerInRange = false;
        //mainMenuManager.OpenMenuScene();
    }
}
