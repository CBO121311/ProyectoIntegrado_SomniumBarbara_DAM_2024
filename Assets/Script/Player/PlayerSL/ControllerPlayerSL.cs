using UnityEngine;


public class ControllerPlayerSL : MonoBehaviour, IDataPersistence
{
    [SerializeField] private bool loadPosition = false;

    public Animator animator;
    public Rigidbody2D rb2D;
    private Vector2 moveDirection;
    public float velocidadMovimiento;
    float horizontalInput, verticalInput;

    private bool hasStoppedMovement = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        var uiManager = UIManager_SelectionLevel.GetInstance();

        if (!uiManager.gameIsPaused && InputManager.GetInstance().GetInteractPressed())
        {
            uiManager.ToggleInventoryUI();
        }

        if ( !uiManager.inventoryIsActivated && InputManager.GetInstance().GetInMenuPressed())
        {
            uiManager.TogglePauseUI();
        }
    }

    private void FixedUpdate()
    {

        MovePlayer();

        //Si hay un dialogo, pausa, o selección de nivel te impido moverte
        if (DialogueManager.GetInstance().dialogueIsPlaying || 
            UIManager_SelectionLevel.GetInstance().gameIsPaused ||
            UIManager_SelectionLevel.GetInstance().levelSelectionIsActive)
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
        if (DialogueManager.GetInstance().dialogueIsPlaying || 
            UIManager_SelectionLevel.GetInstance().gameIsPaused || 
            UIManager_SelectionLevel.GetInstance().levelSelectionIsActive)
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

        if (!loadPosition) return;

        if (TemporaryData.UseTemporaryPosition)
        {
            this.transform.position = TemporaryData.PlayerPosition;
            TemporaryData.UseTemporaryPosition = false; // Restablecer la variable
        }
        else
        {
            this.transform.position = data.playerPosition;
        }
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
