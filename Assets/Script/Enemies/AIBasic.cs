using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasic : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float speed = 2f;

    //Contador para esté parad.
    private float waitTime;
    public float startWaitTime = 2;
    private int i = 0;
    private Vector2 actualPos;
    public Transform[] moveSpots;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckEnemyMoving());

        //Se mueve hasta la posición
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[i].transform.position, speed * Time.deltaTime);

        //Si llegamos muy cerca del punto
        if (Vector2.Distance(transform.position, moveSpots[i].transform.position) < 0.1f)
        {
            if (waitTime <= 0)
            {
                //Comprueba la animación
                if (moveSpots[i] != moveSpots[moveSpots.Length - 1])
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    IEnumerator CheckEnemyMoving()
    {
        actualPos = transform.position;

        yield return new WaitForSeconds(0.5f);

        //Compara una posición a una posición anterior, sirve para de hacer flip al sprite
        if (transform.position.x > actualPos.x)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("Idle",false);
        }
        else if (transform.position.x < actualPos.x)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
        }
    }
}
