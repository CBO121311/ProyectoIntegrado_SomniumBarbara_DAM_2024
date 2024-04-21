using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;


public class ControllerPlayerSL : MonoBehaviour, IDataPersistence
{

    public Animator animator;
    public Rigidbody2D rb2D;
    private Vector2 moveDirection;
    public float velocidadMovimiento;
    float horizontalInput, verticalInput;

    public Transform raycastOrigin; // Punto desde donde se lanza el rayo
    public LayerMask interactableLayer; // Capa de los objetos con los que el jugador puede interactuar

    public float raycastDistance = 5f; // Distancia máxima del rayo

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    private void PauseGame()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if(InputManager.GetInstance().GetInMenuPressed() && !UIManager.gameIsPaused)
        {
            UIManager.changeValueGameIsPaused();
            Debug.Log("HOLAAA");
        }
    }

    private bool hasStoppedMovement = false;
    private void FixedUpdate()
    {

        MovePlayer();
        PauseGame();

        // Lanzar un rayo hacia adelante desde el punto de origen en la dirección del movimiento
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, moveDirection, raycastDistance, interactableLayer);

        // Visualizar el rayo en tiempo de ejecución
        Debug.DrawRay(raycastOrigin.position, moveDirection * raycastDistance, Color.red);

        // Verificar si el rayo colisionó con un objeto interactable
        if (hit.collider != null)
        {
            // El rayo colisionó con un objeto interactable
            GameObject interactableObject = hit.collider.gameObject;
            Debug.Log("El jugador está frente a: " + interactableObject.name);

            // Aquí puedes tomar alguna acción, como activar un interruptor, iniciar una conversación, etc.
        }
        else
        {
            // El rayo no colisionó con un objeto interactable
            Debug.Log("No hay nada frente al jugador.");
        }

        //Si hay un dialogo te impido moverte
        if (DialogueManager.GetInstance().dialogueIsPlaying || UIManager.gameIsPaused)
        {
            // Solo ejecutar una vez
            if (!hasStoppedMovement)
            {

                horizontalInput = 0;
                verticalInput = 0;
                animator.SetFloat("Horizontal", horizontalInput);
                animator.SetFloat("Vertical", verticalInput);

                hasStoppedMovement = true;
            }

            return;
        }

        // Restablecer el estado si ya no está en diálogo o pausa
        hasStoppedMovement = false;

        rb2D.MovePosition(rb2D.position + Time.fixedDeltaTime * velocidadMovimiento * new Vector2(horizontalInput, verticalInput).normalized);
    }

    private void MovePlayer()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || UIManager.gameIsPaused)
        {
            return;
        }

        moveDirection = InputManager.GetInstance().GetMoveDirection();

        // Obtener los valores de dirección x e y.
        horizontalInput = moveDirection.x;
        verticalInput = moveDirection.y;

        // Definir un umbral para la dirección
        float umbral = 0.1f;

        if( horizontalInput > umbral)
        {
            horizontalInput = 1;

        }else if(horizontalInput < -umbral)
        {
            horizontalInput = -1;
        }
        else
        {
            horizontalInput = 0;
        }

        if(verticalInput > umbral)
        {
            verticalInput = 1;
        }
        else if (verticalInput < -umbral)
        {
            verticalInput = -1;
        }
        else
        {
            verticalInput = 0;
        }

        //Debug.Log("Direccion X: " + horizontalInput);
        //Debug.Log("Direccion Y: " + verticalInput);

        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetFloat("LastX", horizontalInput);
            animator.SetFloat("LastY", verticalInput);
        }
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
