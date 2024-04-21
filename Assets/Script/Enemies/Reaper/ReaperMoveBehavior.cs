using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperMoveBehavior : StateMachineBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaMinima = 0.8f;
    private Transform jugador;
    private Reaper reaper;

    //Cambia el estado del animator
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        reaper = animator.gameObject.GetComponent<Reaper>();
    }

    //De mientra está en el estado
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Vector2 direccion = (jugador.position - animator.transform.position).normalized;
        float distanciaAlJugador = Vector2.Distance(animator.transform.position, jugador.position);

        if (distanciaAlJugador > distanciaMinima)
        {
            animator.transform.position += (Vector3)direccion * velocidadMovimiento * Time.deltaTime;
            reaper.Spin(jugador.position);
        }
        // Si la distancia es menor que el umbral, detén el movimiento
        else
        {
            // Puedes agregar aquí alguna lógica adicional, como la animación de detención.
        }


        //Antiguo código, se pega demasiado al jugador.

        /*Vector2 direccion = (jugador.position - animator.transform.position).normalized;
        animator.transform.position += (Vector3)direccion * velocidadMovimiento * Time.deltaTime;
        reaper.Spin(jugador.position);*/
        //--------------------------------------
        //No se qué hace abajo
        /*animator.transform.position = Vector2.MoveTowards(animator.transform.position, jugador.position, velocidadMovimiento*Time.deltaTime);
        reaper.Spin(jugador.position);*/
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
