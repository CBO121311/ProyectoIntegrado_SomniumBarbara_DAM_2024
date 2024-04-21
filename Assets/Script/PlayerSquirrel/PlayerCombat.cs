using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float life;
    private PlayerMoveSquirrel playerMovement;
    [SerializeField] private float controlLossTime;
    private Animator animator;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMoveSquirrel>();
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("Hit");
        StartCoroutine(LoseControl());
        StartCoroutine(DisableCollision());
        playerMovement.BounceOnDamage(position);
    }


    private IEnumerator DisableCollision()
    {
        Physics2D.IgnoreLayerCollision(10,11,true);
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
