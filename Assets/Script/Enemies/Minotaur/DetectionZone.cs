using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Minotaur minotaur;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    private bool playerDetected = false;
    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            if (!playerDetected)
            {
                playerDetected = true;
                animator.SetTrigger("TargetPlayer");
                minotaur.StartAttacking();
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;
                animator.ResetTrigger("TargetPlayer");
                minotaur.StopAttacking();
            }
        }
    }


    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("TargetPlayer");
            minotaur.StartAttacking();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.ResetTrigger("TargetPlayer");
            minotaur.StopAttacking();
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
