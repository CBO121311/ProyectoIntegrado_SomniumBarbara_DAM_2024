using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float maxLife;
    private float life;
    private PlayerMoveSquirrel playerMovement;
    [SerializeField] private float controlLossTime;
    private Animator animatorSquirrel;
    [SerializeField] private Animator lifePanelAnimator;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMoveSquirrel>();
        animatorSquirrel = GetComponent<Animator>();
        life = maxLife;
    }

    /// <summary>
    /// Reduce la vida del jugador según la cantidad especificada.
    /// </summary>
    /// <param name="daño">Cantidad de daño que se aplica al jugador.</param>
    public void takeDamage(float damage)
    {
        life -= damage;
    }


    /// <summary>
    /// Aplica daño al jugador, activa una animación de golpe, y realiza acciones adicionales.
    /// </summary>
    /// <param name="damage">Cantidad de daño que se aplica al jugador.</param>
    /// <param name="position">Posición del golpe recibido.</param>
    public void takeDamage(float damage, Vector2 position)
    {
      
        life -= damage;

        if (life == 1)
        {
            lifePanelAnimator.SetTrigger("hitOne");
        }

        animatorSquirrel.SetTrigger("Hit");
        StartCoroutine(LoseControl());
        StartCoroutine(DisableCollision());
        


        if(life <= 0)
        {
            lifePanelAnimator.SetTrigger("hitTwo");

            PlayerSpawnSquirrel playerSpawn = GetComponent<PlayerSpawnSquirrel>();
            playerSpawn.Death();
            life = maxLife;
            return;
        }
        playerMovement.BounceOnDamage(position);
    }


    private IEnumerator DisableCollision()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(controlLossTime);
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    private IEnumerator LoseControl()
    {
        playerMovement.canMove = false;
        yield return new WaitForSeconds(controlLossTime);
        playerMovement.canMove = true;
    }

}
