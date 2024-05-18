using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnSquirrel : MonoBehaviour
{
    private float checkPointPositionX, checkPointPositionY;
    private Animator animator;
    [SerializeField] private bool initCheckpoint = false;


    private void Awake()
    {

        // Eliminar las claves específicas de PlayerPrefs
        PlayerPrefs.DeleteKey("checkPointPositionX");
        PlayerPrefs.DeleteKey("checkPointPositionY");


        PlayerPrefs.Save();

    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (initCheckpoint)
        {
            RestorePlayerPositionCheckPoint();
        }
    }


    // Al iniciar, el jugador será colocado en el checkpoint.
    private void RestorePlayerPositionCheckPoint()
    {
        if (PlayerPrefs.GetFloat("checkPointPositionX") != 0)
        {
            transform.position = (new Vector2(PlayerPrefs.GetFloat("checkPointPositionX"),
                PlayerPrefs.GetFloat("checkPointPositionY")));

            checkPointPositionX = PlayerPrefs.GetFloat("checkPointPositionX");
            checkPointPositionY = PlayerPrefs.GetFloat("checkPointPositionY");
        }
    }

    /// <summary>
    /// Guarda las coordenadas del punto control alcanzado por el jugador
    /// </summary>
    /// <param name="x">La coordenada X del punto de control.</param>
    /// <param name="y">La coordenada Y del punto de control.</param>
    public void RecordCheckPoint(float x, float y)
    {
        PlayerPrefs.SetFloat("checkPointPositionX", x);
        PlayerPrefs.SetFloat("checkPointPositionY", y);

        checkPointPositionX = PlayerPrefs.GetFloat("checkPointPositionX");
        checkPointPositionY = PlayerPrefs.GetFloat("checkPointPositionY");
    }

    //Cuando el jugador recibe daño, invoca la animación hit. 
    public void PlayerDamaged()
    {
        Debug.Log("GOOOLPE");
        animator.SetTrigger("Hit");
        Invoke("Death",0.2f);
    }


    //Cuando muere el jugador, cambia su aspecto.
    public void Death()
    {
        transform.position = new Vector2(checkPointPositionX, checkPointPositionY);

        // Detener el movimiento del jugador
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = Vector2.zero;

        transform.position = new Vector2(checkPointPositionX, checkPointPositionY);

        // Restablecer la velocidad del jugador a cero
        rb2D.velocity = Vector2.zero;
    }
}
