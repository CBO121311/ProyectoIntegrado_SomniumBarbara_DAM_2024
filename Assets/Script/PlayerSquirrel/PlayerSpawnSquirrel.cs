using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnSquirrel : MonoBehaviour
{
    private float checkPointPositionX, checkPointPositionY;
    private Animator animator;
    public ChangePlayer changePlayer;
    private Collider2D collider2;
    [SerializeField] private bool initCheckpoint = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        changePlayer = GetComponent<ChangePlayer>();

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
        animator.SetTrigger("Hit");
        Invoke("Death",0.2f);
    }


    //Cuando muere el jugador, cambia su aspecto.
    public void Death()
    {
        //changePlayer.changePlay();
        transform.position = new Vector2(checkPointPositionX, checkPointPositionY);
    }
}
