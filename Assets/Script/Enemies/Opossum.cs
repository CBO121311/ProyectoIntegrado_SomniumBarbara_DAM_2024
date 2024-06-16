using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Opossum : Enemy
{

    [SerializeField] private float speed;
    [SerializeField] private Transform controlGround;
    [SerializeField] private float distance;
    [SerializeField] private bool moveRight;

    // Método de inicialización



    // Método de patrullaje
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

    protected override void Die()
    {
        base.Die();
        StartCoroutine(ActiveDeath());
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


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerCombat = other.gameObject.GetComponent<PlayerCombat>();

            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                if (playerCombat != null && playerCombat.CanDealDamage())
                {
                    AudioManager.Instance.PlaySFX(6);
                    TakeDamage(50f);
                }
                other.gameObject.GetComponent<PlayerJump>().BounceEnemyOnHit();
            }
            else
            {
                playerCombat.TakeDamage(1, other.GetContact(0).normal);
            }
        }
    }



    // Método de rotación
    private void Rotate()
    {
        moveRight = !moveRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        speed *= -1;
    }

    // Método para detener el movimiento
    public void StopMovement()
    {
        isDead = true;
    }

    

    // Método para manejar la muerte
    public void Death()
    {
        Destroy(gameObject);
    }

    // Método para dibujar gizmos en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(obstacleCheck.position, obstacleCheck.position + transform.right * obstacleCheckDistance);
    }
}
