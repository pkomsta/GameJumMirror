using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    Enemy en;
    MMConeOfVision con;
    public override void StartState(Enemy enemy)
    {
        MMConeOfVision cone = enemy.coneOfVision;
        en = enemy;
        con = cone;
        cone.MeshRenderer.enabled = false;
        cone.enabled = false;
        en.canSeePlayer = false;
        en.navMeshAgent.SetDestination(en.transform.position);
        Invoke("TurnOnVision", en.stunTime);
    }

    void TurnOnVision()
    {
        con.enabled = true;
        con.MeshRenderer.enabled = true;
        en.ChangeState(en.chase);
    }

    public override void UpdateState(Enemy enemy)
    {
        
    }

    public override void ExitState(Enemy enemy)
    {

    }
}
