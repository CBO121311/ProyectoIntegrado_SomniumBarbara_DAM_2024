using UnityEngine;

public class PlayerMoveSquirrel : MonoBehaviour
{
    [Header("References")]
    Rigidbody2D rb2D;
    SpriteRenderer spr;
    Animator animator;

    [Header("Jump")]
    public float jumpSpeed = 3;


    [Header("Values")]
    public float runSpeed = 2;

    private float moveX;
    private float moveY;
    public float fallMutiplier = 0.5f;
    public float lowJumpMultiplier = 1f;


    public bool canMove = true;
    [SerializeField] private Vector2 velocidadRebote;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }

        PauseGame();
    }

    private void PauseGame()
    {
        if (InputManager.GetInstance().GetInMenuPressed() && !UIManager.GameIsPaused)
        {
            UIManager.changeGameIsPaused();
        }
    }

    public void Move()
    {
        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();

        moveX = moveDirection.x;
        moveY = moveDirection.y;


        float velocityX = moveX * runSpeed * runSpeed;

        rb2D.velocity = new Vector2(velocityX, rb2D.velocity.y);

        if (moveX < 0)
        {
            spr.flipX = true;
            animator.SetBool("Run", true);
        }
        else if (moveX > 0)
        {
            spr.flipX = false;
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }


        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMutiplier) * Time.deltaTime;
        }
        if (rb2D.velocity.y > 0 && moveY == 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
        }
    }
    public void BounceOnDamage(Vector2 pointHit)
    {
        rb2D.velocity = new Vector2(-velocidadRebote.x * pointHit.x, velocidadRebote.y);
    }
}
