using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated = false;
    private AudioSource audioSource;
    private Animator animator;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            collision.GetComponent<PlayerSpawnSquirrel>().RecordCheckPoint(transform.position.x, transform.position.y);
            animator.SetTrigger("Player");
            AudioManager.Instance.PlaySFX(5);
        }
    }
}
