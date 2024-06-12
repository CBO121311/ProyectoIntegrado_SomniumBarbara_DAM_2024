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
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isDead)
        {
            rb2D.velocity = Vector2.zero; // Asegurarse de que el enemigo esté quieto si está muerto
            rb2D.isKinematic = true;
            return;
        }
        Patrol();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                TakeDamage(50f);
                other.gameObject.GetComponent<PlayerDoubleJump>().BounceEnemyOnHit();
            }
            else
            {
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(1, other.GetContact(0).normal);
            }
        }
    }



    // Método de patrullaje
    protected override void Patrol()
    {
        RaycastHit2D informGround = Physics2D.Raycast(controlGround.position, Vector2.down, distance);

        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        if (!informGround)
        {
            // Girar
            Rotate();
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
        Gizmos.DrawLine(controlGround.transform.position, controlGround.transform.position + Vector3.down * distance);
    }
}
