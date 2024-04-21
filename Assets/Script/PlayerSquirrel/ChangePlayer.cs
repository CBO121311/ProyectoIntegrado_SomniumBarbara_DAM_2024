using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayer : MonoBehaviour
{
    public enum Player {Squirrelv1, Squirrelv2 }
    public Player playerSelected;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public RuntimeAnimatorController[] playersController;
    public Sprite[] playerRenderer;


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (playerSelected)
        {
            case Player.Squirrelv1:
                spriteRenderer.sprite = playerRenderer[0];
                animator.runtimeAnimatorController = playersController[0];
                break;
            case Player.Squirrelv2:
                spriteRenderer.sprite = playerRenderer[1];
                animator.runtimeAnimatorController = playersController[1];
                break;
        }
    }


    public void changePlay()
    {
        playerSelected = Player.Squirrelv2;


        switch (playerSelected)
        {
            case Player.Squirrelv1:
                spriteRenderer.sprite = playerRenderer[0];
                animator.runtimeAnimatorController = playersController[0];
                break;
            case Player.Squirrelv2:
                spriteRenderer.sprite = playerRenderer[1];
                animator.runtimeAnimatorController = playersController[1];
                break;
        }
    }
}
