using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject effect;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

      
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("Hit");
                other.gameObject.GetComponent<DoubleJump>().BounceEnemyOnHit();
            }
            else
            {
                other.gameObject.GetComponent<PlayerCombat>().takeDamage(20, other.GetContact(0).normal);
            }
        }
    }

    public void Hit()
    {
        Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
