using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minotaur : MonoBehaviour
{

    [Header("References")]
    private SpriteRenderer spr;
    private Animator animator;
    private Rigidbody2D rb2D;
    private Collider2D col2D;




    private bool isDead = false;
    private bool facingRight = true; // Información si está mirando hacia la derecha
    private bool isAttacking = false;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioHit;

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
    private bool isGrounded; // Información si está sobre el suelo
    private bool isObstacleAhead; // Información si hay un obstáculo enfrente



    [Header("Attack")]
    public float attackCooldown = 1f;
    public GameObject weapon;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        weapon.SetActive(false);
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void FixedUpdate()
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
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        GameEventsManager.instance.DeadEnemy();

        StartCoroutine(FadeOutMinotaur());
    }


    private IEnumerator FadeOutMinotaur()
    {
        yield return new WaitForSeconds(0.5f);
        rb2D.velocity = Vector2.zero;
        rb2D.isKinematic = true;
        col2D.enabled = false;

        Color color = spr.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / 2;
            spr.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                // El jugador golpea al enemigo desde arriba
                animator.SetTrigger("Hit");
                other.gameObject.GetComponent<PlayerDoubleJump>().BounceEnemyOnHit();
                TakeDamage(50f);
                audioSource.PlayOneShot(audioHit);
                Debug.Log("Golpe");
            }
            else
            {
                // El jugador colisiona con el enemigo de otra manera
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(1, other.GetContact(0).normal);
            }
        }
    }

    private void Patrol()
    {
        rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);

        isObstacleAhead = Physics2D.Raycast(obstacleCheck.position, transform.right, obstacleCheckDistance, obstacleLayer);
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isObstacleAhead || !isGrounded)
        {
            Turn();
        }
    }

    private void Turn()
    {
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        moveSpeed *= -1;
    }

    public void StartAttacking()
    {
        if (!isAttacking && !isDead)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    public void StopAttacking()
    {
        isAttacking = false;
        StopCoroutine(AttackPlayer());
    }

    private IEnumerator AttackPlayer()
    {
        Debug.Log("ATACA");

        isAttacking = true;
        animator.SetTrigger("TargetPlayer");
        rb2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    public void ActivateWeaponCollider()
    {
        weapon.SetActive(true);
    }

    public void DeactivateWeaponCollider()
    {
        weapon.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(obstacleCheck.position, obstacleCheck.position + transform.right * obstacleCheckDistance);
    }
}
