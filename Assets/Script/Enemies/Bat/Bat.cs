using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] public Transform player;
    [SerializeField] private float detectDistance;
    [SerializeField] private float chaseTime;
    [SerializeField] private AudioClip detectionSound;

    public Vector3 startPoint;
    public bool followPlayer;

    protected override void Start()
    {
        base.Start();
        startPoint = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if(!isDead)
        {
            detectDistance = Vector2.Distance(transform.position, player.position);
            animator.SetFloat("Distance", detectDistance);
        }
    }

    protected override void Patrol()
    {
        // no se utiliza
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(ActiveDeath());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                other.gameObject.GetComponent<PlayerDoubleJump>().BounceEnemyOnHit();
                TakeDamage(50f);
                audioSource.PlayOneShot(audioHit);
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
        yield return new WaitForSeconds(0.2f);
        animator.SetTrigger("Death");
    }

    public void Death()
    {
        Destroy(gameObject);
    }


}
