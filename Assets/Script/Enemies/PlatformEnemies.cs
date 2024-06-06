using UnityEngine;

public class PlatformEnemies : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform controlGround;
    [SerializeField] private float distance;
    [SerializeField] private bool moveRight;
    private Rigidbody2D rb2D;

    private bool isDead = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb2D.velocity = Vector2.zero; // Asegurarse de que el enemigo esté quieto si está muerto
            rb2D.isKinematic = true;
            return;
        }

        RaycastHit2D informGround = Physics2D.Raycast(controlGround.position, Vector2.down, distance);

        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
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

    public void StopMovement()
    {
        isDead = true; 
    }
}
