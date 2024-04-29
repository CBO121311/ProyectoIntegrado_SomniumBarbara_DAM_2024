using UnityEngine;

public class DoubleJump : MonoBehaviour
{

    Rigidbody2D rb2D;
    Animator animator;

    [Header("Salto")]
    [SerializeField] private LayerMask isGroundMask;
    [SerializeField] private Transform controlGround;
    [SerializeField] private Vector3 dimenBox;
    [SerializeField] private float jumpSpeed = 3;
    private bool enSuelo;
    private bool salto = false;
    [SerializeField] private int saltosExtrasRestantes;
    [SerializeField] private int saltosExtra;
    [SerializeField] private float speedBounce;
    private AudioSource audioSource;
    [SerializeField]private AudioClip jumpSound;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (UIManager.gameIsPaused)
        {
            return;
        }

        enSuelo = Physics2D.OverlapBox(controlGround.position, dimenBox, 0f, isGroundMask);
        animator.SetBool("Jump", !enSuelo);

        if (enSuelo)
        {
            saltosExtrasRestantes = saltosExtra;
        }

        if (InputManager.GetInstance().GetSubmitPressed())
        {
            //Debug.Log("INTENTO SALTAR");
            salto = true;
        }
    }

    private void FixedUpdate()
    {
        if (UIManager.gameIsPaused)
        {
            return;
        }

        Movimiento(salto);
        salto = false;
    }

    private void Movimiento(bool salto)
    {
        if (salto)
        {
            if (enSuelo)
            {
                Salto();
            }
            else
            {
                if (salto && saltosExtrasRestantes > 0)
                {
                    Salto();
                    saltosExtrasRestantes -= 1;
                }
            }
        }
    }

    //El que golpea al enemigo da un bote
    public void BounceEnemyOnHit()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, speedBounce);
    }

    private void Salto()
    {
        // rb2D.AddForce(new Vector2(0f, jumpSpeed));
        audioSource.PlayOneShot(jumpSound);
        rb2D.velocity = new Vector2(0f, jumpSpeed);
        salto = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controlGround.position, dimenBox);
    }
}
