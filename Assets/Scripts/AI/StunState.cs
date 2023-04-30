using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    Enemy en;
    MMConeOfVision con;
    public override void StartState(Enemy enemy)
    {
        CancelInvoke();
        if(enemy.coneOfVision!= null)
        {
            MMConeOfVision cone = enemy.coneOfVision;
            con = cone;
            cone.MeshRenderer.enabled = false;
            cone.enabled = false;
            en.canSeePlayer = false;
        }
       
        en = enemy;
        
        en.navMeshAgent.SetDestination(en.transform.position);
        Debug.Log("Stunned!");
        if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Monster_Stun"))
        {
            enemy.GetAnimator().CrossFade("Monster_Stun", 0.2f);

        }
        Invoke("TurnOnVision", en.stunTime);
    }

    void TurnOnVision()
    {
        if(con !=null)
        {
            con.enabled = true;
            con.MeshRenderer.enabled = true;
        }
        

        if(en.previousState == en.chase)
            en.ChangeState(en.chase);
        if (en.previousState == en.patrol)
            en.ChangeState(en.patrol);
    }

    public override void UpdateState(Enemy enemy)
    {
        
    }

    public override void ExitState(Enemy enemy)
    {

    }
}
