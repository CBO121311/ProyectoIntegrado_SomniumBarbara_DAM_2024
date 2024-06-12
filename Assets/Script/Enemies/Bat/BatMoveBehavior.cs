using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMoveBehavior : StateMachineBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseTime;

    private float chaseTime;
    private Transform player;
    private Bat bat;

    //Cambia el estado del animator
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chaseTime = baseTime;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bat = animator.gameObject.GetComponent<Bat>();
        bat.StartChasing();
    }

    //De mientra está en el estado
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(bat.followPlayer);

        if(bat.followPlayer)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, player.position, moveSpeed * Time.deltaTime);
            bat.Spin(player.position);
            chaseTime -= Time.deltaTime;
            if (chaseTime <= 0)
            {
                animator.SetTrigger("Return");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       bat.StopChasing();
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
