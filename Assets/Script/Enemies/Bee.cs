using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bee : MonoBehaviour
{
    [Header("References")]
    private SpriteRenderer spr;
    private Animator animator;
    //private Rigidbody2D rb2D;
    private Collider2D col2D;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioHit;


    [Header("Movement")]
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private float attackDistance = 2f;
    private Vector2 initialPosition;
    private bool isAttacking = false;
    private Vector2 attackTargetPosition;

    [Header("Raycast")]
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask playerLayer;


    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, attackTargetPosition, attackSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, attackTargetPosition) < 0.01f)
            {
                isAttacking = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
            animator.SetBool("Player", false);
            if (Vector2.Distance(transform.position, initialPosition) < 0.01f)
            {
                transform.position = initialPosition;
                //rb2D.velocity = Vector2.zero;
                
            }
        }

        DetectPlayer();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                audioSource.PlayOneShot(audioHit);
                other.gameObject.GetComponent<PlayerDoubleJump>().BounceEnemyOnHit();
            }
            else
            {
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(1, other.GetContact(0).normal);
            }
        }
    }

    private void DetectPlayer()
    {
        Vector2 rayOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("Player", true);
                attackTargetPosition = new Vector2(initialPosition.x, initialPosition.y - attackDistance);
                StartCoroutine(Attack());
            }
        }
    }


    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f); 
        isAttacking = false;
    }


    private void OnDrawGizmos()
    {
        if (isAttacking)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }

        Vector2 rayOrigin = transform.position;
        Vector2 rayEnd = rayOrigin + Vector2.down * raycastDistance;

        Gizmos.DrawLine(rayOrigin, rayEnd);
        Gizmos.DrawSphere(rayEnd, 0.1f);
    }
}
