using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] private float detectDistance;
    [SerializeField] private float chaseTime;
    [SerializeField] private AudioClip detectionSound;

    public Vector3 startPoint;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool followPlayer;
    private AudioSource audioSource;

    [Header("Health")]
    public float health = 50f;
    [SerializeField] private AudioClip audioHit;
    private bool isDead = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        startPoint = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        detectDistance = Vector2.Distance(transform.position, player.position);
        animator.SetFloat("Distance", detectDistance);

        if (health <= 0 && !isDead)
        {
            Die();
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
        //animator.SetTrigger("Death");
        GameEventsManager.instance.DeadEnemy();

        Destroy(this.gameObject);
    }


    public void Spin(Vector3 target)
    {
        spriteRenderer.flipX = transform.position.x < target.x;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("LE DI WEAPON COLLIDER");

            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {

                Vector2 contactPoint = other.ClosestPoint(transform.position);
                Vector2 pushDirection = ((Vector2)other.transform.position - contactPoint).normalized;
                Vector2 pushVector = pushDirection * new Vector2(-1, -1); // Magnitud ajustada

                playerCombat.takeDamage(1, pushVector);

                followPlayer = false;
                animator.SetTrigger("Return");
            }
        }
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
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(1, other.GetContact(0).normal);

                followPlayer = false;
                animator.SetTrigger("Return");
            }
        }
    }


    public void StartChasing()
    {
        followPlayer = true;
        //PlayDetectionSound();
    }

    public void StopChasing()
    {
        followPlayer = false;
    }

    public void PlayDetectionSound()
    {
        if (audioSource != null && detectionSound != null)
        {
            audioSource.PlayOneShot(detectionSound);
        }
    }

    public void Hit()
    {
        GameEventsManager.instance.DeadEnemy();

        Destroy(gameObject);
    }
}
