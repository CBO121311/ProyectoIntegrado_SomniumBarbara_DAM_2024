using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool playerInRange = false;
    [SerializeField] public Transform player;
    [SerializeField] private float distancePlayer;
    public Vector3 startPosition;

    [SerializeField] private GameObject weapon;


    private void Awake()
    {
        gameObject.SetActive(false);
        weapon.SetActive(false);
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position);
    }


    public void Spin(Vector3 target)
    {
        if (transform.position.x < target.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            animator.SetTrigger("Player");
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
}
