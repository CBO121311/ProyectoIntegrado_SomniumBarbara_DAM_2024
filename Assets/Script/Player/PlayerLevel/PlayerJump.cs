using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private Animator animator;
    private PlayerOneWayPlatform playerOneWayPlatform;

    [Header("Jump Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 groundCheckSize;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float bounceForce = 5f;
    [SerializeField] private int maxExtraJumps = 1;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private ParticleSystem jumpParticles;

    private bool isGrounded;
    private int extraJumpsLeft;
    private bool jumpRequest = false;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerOneWayPlatform = GetComponent<PlayerOneWayPlatform>();
        extraJumpsLeft = maxExtraJumps;
    }
    private void Update()
    {
        if (UIManager_Level.GetInstance().gameIsPaused)
        {
            return;
        }
        CheckGroundStatus();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        if (UIManager_Level.GetInstance().gameIsPaused)
        {
            return;
        }

        PerformJump();
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        animator.SetBool("Jump", !isGrounded);

        if (isGrounded)
        {
            extraJumpsLeft = maxExtraJumps;
        }
    }

    private void HandleJumpInput()
    {

        if (GameController.instance.IsGameRunning && InputManager.GetInstance().GetSubmitPressed() && !UIManager_Level.GetInstance().IsPauseCooldownActive())
        {
            Debug.Log("Saltó");
            jumpRequest = true;
        }

    }

    private void PerformJump()
    {
        if (jumpRequest)
        {
            if (isGrounded)
            {
                ExecuteJump();
            }
            else if(extraJumpsLeft > 0)
            {
                ExecuteJump();
                extraJumpsLeft--;
                
            }
            jumpRequest = false;
        }
    }



    private void ExecuteJump()
    {
        playerOneWayPlatform.CheckAndDisableCollision();
        jumpParticles.Play();
        AudioManager.Instance.PlaySFX(2);
        rb2D.velocity = new Vector2(0f, jumpForce);
    }

    //Golpea al enemigo da un bote
    public void BounceEnemyOnHit()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, bounceForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
