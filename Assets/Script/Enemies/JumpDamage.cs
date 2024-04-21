using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDamage : MonoBehaviour
{
    private Collider2D collider2D;
    public Collider2D damagecollider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject destroyParticle;
    public float jumpForce = 2.5f;
    public int lifes = 2;


    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = (Vector2.up * jumpForce);
            LossLifeAndHit();
            CheckLife();
        }
    }

    private void LossLifeAndHit()
    {
        lifes--;
        animator.Play("Hit");
    }

    private void CheckLife()
    {
        if(lifes == 0)
        {
            destroyParticle.SetActive(true);
            spriteRenderer.enabled = false;
            collider2D.enabled = false;
            damagecollider2D.enabled = false;
            Invoke("EnemyDie", 0.5f);
        }
    }

    public void EnemyDie()
    {
        Destroy(gameObject);
    }
}
