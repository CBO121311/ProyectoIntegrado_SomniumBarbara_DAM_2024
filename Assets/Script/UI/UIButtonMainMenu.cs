using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonMainMenu : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Pressed()
    {
        //Debug.Log("ANIMACION");
        animator.SetTrigger("Click");
    }
}
