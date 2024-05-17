using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> DetectedColliders = new List<Collider2D>();
    Collider2D col;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger("TargetPlayer");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
