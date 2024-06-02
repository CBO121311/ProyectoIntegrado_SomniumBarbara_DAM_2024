using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private Collider2D col;
    [SerializeField] private Animator animator;
    [SerializeField] private Minotaur minotaur;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("TargetPlayer");
            minotaur.StartAttacking();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.ResetTrigger("TargetPlayer");
            minotaur.StopAttacking();
        }
    }
}
