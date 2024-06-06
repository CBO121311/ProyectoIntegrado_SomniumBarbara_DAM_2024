using UnityEngine;
using UnityEngine.Audio;

public class Opossum : MonoBehaviour
{
    [Header("References")]
    private Animator animator;
    private Rigidbody2D rb2D;
    private Collider2D col2D;
    private PlatformEnemies platformEnemies;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        platformEnemies = GetComponent<PlatformEnemies>();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                platformEnemies.StopMovement();
                other.gameObject.GetComponent<PlayerDoubleJump>().BounceEnemyOnHit();
            }
            else
            {
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(1, other.GetContact(0).normal);
            }
        }
    }

    public void Hit()
    { 
        col2D.enabled = false;
        animator.SetTrigger("Death");
    }

    public void Death()
    {
        GameEventsManager.instance.DeadEnemy();
        Destroy(gameObject);
    }
}
