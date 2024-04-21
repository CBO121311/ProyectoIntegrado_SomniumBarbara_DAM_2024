using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEnemies : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform controlGround;
    [SerializeField] private float distance;
    [SerializeField] private bool moveRight;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D informGround = Physics2D.Raycast(controlGround.position, Vector2.down, distance);

        rb.velocity = new Vector2(speed, rb.velocity.y);
        if(informGround == false)
        {
            //Girar
            Rotate();
        }
    }


    private void Rotate()
    {
        moveRight = !moveRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        speed *= -1;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controlGround.transform.position, controlGround.transform.position + Vector3.down * distance);
    }
}
