using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ControllerPlayer_Level : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb2D;
    private SpriteRenderer spr;
    private Animator squirrelAnimator;
    private Collider2D col2D;

    [Header("Values")]
    public float runSpeed = 2;
    private float moveX;
    private float moveY;
    public float fallMutiplier = 0.5f;
    public float lowJumpMultiplier = 1f;


    public bool canMove = true;
    [SerializeField] private Vector2 velocidadRebote;
    private bool isDead = false;

    private LevelSquirrelTransition levelSquirrelTransition;

    void Start()
    {
        levelSquirrelTransition = FindFirstObjectByType<LevelSquirrelTransition>();
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        squirrelAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead)
        {
            PauseGame();
        }
    }

    void FixedUpdate()
    {
        if (canMove && !isDead)
        {
            Move();
        }
    }

    private void PauseGame()
    {
        var uiManager = UIManager_Level.GetInstance();

        if (!uiManager.gameIsPaused && InputManager.GetInstance().GetInMenuPressed())
        {
            uiManager.TogglePauseUI();
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
            squirrelAnimator.SetBool("Run", true);
        }
        else if (moveX > 0)
        {
            spr.flipX = false;
            squirrelAnimator.SetBool("Run", true);
        }
        else
        {
            squirrelAnimator.SetBool("Run", false);
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



    public void Die()
    {
        if (!isDead)
        {
            squirrelAnimator.SetTrigger("Death");
            col2D.enabled = false; // Desactiva el colisionador del jugador
            PlayerState.nightmare = true;
            StartCoroutine(FadeOutAndChangeScene());
            isDead = true;
        }
    }

    private IEnumerator FadeOutAndChangeScene()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.isKinematic = true;
        col2D.enabled = false;

        Color color = spr.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / 2; // Ajusta el valor para controlar la velocidad de desvanecimiento
            spr.color = color;
            yield return null;
        }

        levelSquirrelTransition.FadeOutAndLoadScene("BedroomScene");
    }

}
