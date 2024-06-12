using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    protected SpriteRenderer spr;
    protected Animator animator;
    protected Rigidbody2D rb2D;
    protected Collider2D col2D;
    protected AudioSource audioSource;

    protected bool isDead = false;
    protected bool facingRight = true;
    protected bool isAttacking = false;

    [Header("Audio")]
    [SerializeField] protected AudioClip audioHit;

    [Header("Health")]
    public float health = 100f;

    [Header("Movement")]
    public float moveSpeed; // Velocidad de movimiento del enemigo
    public LayerMask groundLayer; // Capa que detecta el suelo debajo
    public LayerMask obstacleLayer; // Capa que detecta obstáculos enfrente

    public float groundCheckDistance; // Distancia de chequeo hacia abajo
    public float obstacleCheckDistance; // Distancia de chequeo hacia adelante

    public Transform groundCheck; // Punto de chequeo hacia abajo
    public Transform obstacleCheck; // Punto de chequeo hacia adelante
    protected bool isGrounded; // Información si está sobre el suelo
    protected bool isObstacleAhead; // Información si hay un obstáculo enfrente

    [Header("Attack")]
    public float attackCooldown = 1f;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isDead && !isAttacking)
        {
            Patrol();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            Debug.Log("MUERTO");
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        
        GameEventsManager.instance.DeadEnemy();
    }

    protected abstract void Patrol();

    protected void Turn()
    {
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        moveSpeed *= -1;
    }

    public void Spin(Vector3 target)
    {
       spr.flipX = transform.position.x > target.x;
    }
}
