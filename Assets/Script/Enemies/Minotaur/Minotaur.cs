using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minotaur : Enemy
{

    [SerializeField] private GameObject weapon;

    protected override void Start()
    {
        base.Start();
        weapon.SetActive(false);
    }


    protected override void Patrol()
    {
        rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);

        isObstacleAhead = Physics2D.Raycast(obstacleCheck.position, transform.right, obstacleCheckDistance, obstacleLayer);
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isObstacleAhead || !isGrounded)
        {
            Turn();
        }
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
        //Debug.Log("ATACA");

        isAttacking = true;
        animator.SetTrigger("TargetPlayer");
        rb2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }


    protected override void Die()
    {
        base.Die();
        StartCoroutine(FadeOutEnemy());
    }


    IEnumerator FadeOutEnemy()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.isKinematic = true;
        col2D.enabled = false;
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(0.5f);

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
                animator.SetTrigger("Hit");
                other.gameObject.GetComponent<PlayerDoubleJump>().BounceEnemyOnHit();
                TakeDamage(50f);
                audioSource.PlayOneShot(audioHit);
                //Debug.Log("Golpe");
            }
            else
            {
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(1, other.GetContact(0).normal);
            }
        }
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
