using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    
    public override void StartState(Enemy enemy)
    {
        enemy.coneOfVision.MeshRenderer.material = enemy.coneOfVision.Materials[2];
        enemy.coneOfVision.VisionRadius = enemy.coneOfVision.VisionRadiusBig;
        enemy.navMeshAgent.speed = 3f;
    }

    public override void UpdateState(Enemy enemy)
    {
        if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Monster_Run"))
        {
            enemy.GetAnimator().CrossFade("Monster_Run", 0.2f);

        }
        if (enemy.canSeePlayer)
        {
            enemy.coneOfVision.MeshRenderer.material = enemy.coneOfVision.Materials[2];

            enemy.navMeshAgent.SetDestination(enemy.playerRef.transform.position);
            enemy.lastSeenPos = enemy.playerRef.transform.position;

        }
        else
        {
            enemy.coneOfVision.MeshRenderer.material = enemy.coneOfVision.Materials[1];

            enemy.navMeshAgent.SetDestination(enemy.lastSeenPos);
 
        }
        Quaternion _rotDirection = Quaternion.LookRotation(enemy.navMeshAgent.destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotDirection, Time.deltaTime * _turnSpeed);



        float distance = Vector3.Distance(enemy.lastSeenPos, enemy.transform.position);
        //Debug.Log("Distance " + distance + "enemy pos " + enemy.transform.position + " player pos " + enemy.lastSeenPos);
        if (distance < 1.1f && !enemy.canSeePlayer)
        {
            enemy.ChangeState(enemy.patrol);
        }
    }

    public override void ExitState(Enemy enemy)
    {
        enemy.previousState = enemy.chase;
    }
}
