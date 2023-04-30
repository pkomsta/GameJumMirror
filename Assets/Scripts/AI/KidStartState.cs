using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KidStartState : State
{
    
    public override void StartState(Enemy enemy)
    {
        if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Death"))
        {
            enemy.GetAnimator().CrossFade("Death", 0.2f);

        }
    }

    public override void UpdateState(Enemy enemy)
    {
        
    }
    public override void ExitState(Enemy enemy)
    {
        if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Anger"))
        {
            enemy.GetAnimator().CrossFade("Anger", 1f);

        }
    }
}
