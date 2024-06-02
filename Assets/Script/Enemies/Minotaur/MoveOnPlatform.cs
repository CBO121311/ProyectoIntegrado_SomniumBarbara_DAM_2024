using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveOnPlatform : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float moveSpeed;
    public LayerMask groundLayer; // Capa que detecta el suelo debajo
    public LayerMask obstacleLayer; // Capa que detecta el obstáculos enfrente

    public float groundCheckDistance; //Distancia de chequeo hacia abajo
    public float obstacleCheckDistance; // Distancia de chequeo hacia adelanta

    public Transform groundCheck;  // Punto de chequeo hacia abajo
    public Transform obstacleCheck; // Punto de chequeo hacia adelante
    public bool isGrounded;
    public bool isObstacleAhead;

    private bool facingRight = true;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
        isObstacleAhead = Physics2D.Raycast(obstacleCheck.position, transform.right, obstacleCheckDistance, obstacleLayer);

        isGrounded= Physics2D.Raycast(groundCheck.position, transform.up * -1, groundCheckDistance, groundLayer);


        if (isObstacleAhead || !isGrounded)
        {
            Turn();
        }
    }

    private void Turn()
    {
        // Cambia la dirección a la que está mirando el enemigo
        facingRight = !facingRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        moveSpeed *= -1; // Invierte la velocidad para moverse en la dirección opuesta
    }

    // Dibuja líneas en el editor para mostrar las distancias de chequeo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.transform.position, groundCheck.transform.position + Vector3.down * groundCheckDistance);
    
        Gizmos.DrawLine(obstacleCheck.transform.position, obstacleCheck.transform.position + transform.right * -1 * obstacleCheckDistance);
    }
}
