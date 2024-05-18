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

    private void Awake()
    {
        
        gameObject.SetActive(false);
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        puntoInicial = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        distancia = Vector2.Distance(transform.position, jugador.position);
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

            //Debug.Log("Encontrado");
        }
    }
}
