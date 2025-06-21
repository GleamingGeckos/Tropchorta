using UnityEngine;

public class AnimationCaller : StateMachineBehaviour
{
    private bool isBlokMovingTransition=false;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("attack"))
        {
            animator.GetComponent<PlayerAnimation>().weaponAnimator.SetTrigger("atakTrigger");

        }
     
    }

   //  OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.GetAnimatorTransitionInfo(layerIndex).IsUserName("blokMovingExit"))
        {
            isBlokMovingTransition = true;
        };

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if ((stateInfo.IsName("blok")||stateInfo.IsName("bloktransition")||stateInfo.IsName("parry")||stateInfo.IsName("blokloop"))&&animator.GetBool("isBlocking")&&isBlokMovingTransition)
        {
            animator.SetTrigger("blockTrigger");
            isBlokMovingTransition = false;
        }
        
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
