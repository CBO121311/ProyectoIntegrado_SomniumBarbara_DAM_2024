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
            var playerCombat = other.gameObject.GetComponent<PlayerCombat>();

            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                other.gameObject.GetComponent<PlayerJump>().BounceEnemyOnHit();

                if (playerCombat != null && playerCombat.CanDealDamage())
                {
                    TakeDamage(50f);
                }

                AudioManager.Instance.PlaySFX(6);
            }
            else
            {
                playerCombat.TakeDamage(1, other.GetContact(0).normal);
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
        AudioManager.Instance.PlaySFX(3);
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
