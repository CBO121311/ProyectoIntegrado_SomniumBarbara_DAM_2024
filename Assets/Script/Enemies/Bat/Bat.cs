using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [Header("References")]
    private Animator animator;
    private Rigidbody2D rb2D;
    private Collider2D col2D;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;


    [SerializeField] public Transform player;
    [SerializeField] private float detectDistance;
    [SerializeField] private float chaseTime;
    [SerializeField] private AudioClip detectionSound;

    public Vector3 startPoint;

    public bool followPlayer;


    [Header("Health")]
    public float health = 50f;
    [SerializeField] private AudioClip audioHit;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
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
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(ActiveDeath());
        }
    }

    public void Spin(Vector3 target)
    {
        spriteRenderer.flipX = transform.position.x > target.x;
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

                //Debug.Log("Golpe");
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

    IEnumerator ActiveDeath()
    {
        col2D.enabled = false;
        if (rb2D != null)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.isKinematic = true;
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Death");
    }

    public void Death()
    {
        GameEventsManager.instance.DeadEnemy();
        Destroy(gameObject);
    }
}
