using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerBed : MonoBehaviour, IControllerPlayer
{

    [Header("Visual Signal")]
    [SerializeField] private GameObject speechBubble;

    private Animator animator;
    private Rigidbody2D rb2D;
    private Vector2 moveDirection;
    public float velocidadMovimiento;
    float horizontalInput, verticalInput;


    private bool hasStoppedMovement = false;

    private void Awake()
    {
        speechBubble.SetActive(false);
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);
        animator.SetFloat("LastX", 0);
        animator.SetFloat("LastY", -1);
    }

    private void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || UIManager_Bedroom.GetInstance().inventoryIsActivated)
        {
            return;
        }

        if (InputManager.GetInstance().GetInMenuPressed())
        {
            UIManager_Bedroom.GetInstance().TogglePauseUI();
        }
    }

    private void FixedUpdate()
    {

        MovePlayer();

        //Si hay un dialogo, pausa, o selección de nivel te impido moverte
        if (DialogueManager.GetInstance().dialogueIsPlaying ||
            UIManager_Bedroom.GetInstance().gameIsPaused)
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

        //Debug.Log($"Moving with h: {horizontalInput}, v: {verticalInput}, speed: {velocidadMovimiento}");

        rb2D.MovePosition(rb2D.position + Time.fixedDeltaTime * velocidadMovimiento * new Vector2(horizontalInput, verticalInput).normalized);
    }

    private void MovePlayer()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying ||
            UIManager_Bedroom.GetInstance().gameIsPaused)
        {
            return;
        }

        moveDirection = InputManager.GetInstance().GetMoveDirection();

        // Obtener los valores de dirección x e y.
        horizontalInput = moveDirection.x;
        verticalInput = moveDirection.y;

        // Definir un umbral para la dirección
        float umbral = 0.1f;

        if (horizontalInput > umbral)
        {
            horizontalInput = 1;

        }
        else if (horizontalInput < -umbral)
        {
            horizontalInput = -1;
        }
        else
        {
            horizontalInput = 0;
        }

        if (verticalInput > umbral)
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

        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            animator.SetFloat("LastX", horizontalInput);
            animator.SetFloat("LastY", verticalInput);
        }
    }

    public void ActivateSpeechBubble()
    {
        speechBubble.SetActive(true);
    }
    public void DisableSpeechBubble()
    {
        speechBubble.SetActive(false);
    }
}
