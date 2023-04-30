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
        MMConeOfVision cone = enemy.coneOfVision;
        en = enemy;
        con = cone;
        cone.MeshRenderer.enabled = false;
        cone.enabled = false;
        en.canSeePlayer = false;
        en.navMeshAgent.SetDestination(en.transform.position);
        Debug.Log("Stunned!");
        Invoke("TurnOnVision", en.stunTime);
    }

    void TurnOnVision()
    {
        con.enabled = true;
        con.MeshRenderer.enabled = true;

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
