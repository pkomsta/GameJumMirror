using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public override void StartState(Enemy enemy)
    {
        enemy.spotLight.color = Color.red;
    }

    public override void UpdateState(Enemy enemy)
    {
        if(enemy.canSeePlayer)
        {
            enemy.navMeshAgent.SetDestination(enemy.playerRef.transform.position);
            enemy.lastSeenPos = enemy.playerRef.transform.position;

        }
        else
        {
            enemy.spotLight.color = Color.yellow;
            enemy.navMeshAgent.SetDestination(enemy.lastSeenPos);

            
        }
        Quaternion _rotDirection = Quaternion.LookRotation(enemy.navMeshAgent.destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotDirection, Time.deltaTime * _turnSpeed);

        Debug.Log(enemy.navMeshAgent.destination + "  " + enemy.playerRef.transform.position);

        float distance = Vector3.Distance(enemy.lastSeenPos, enemy.transform.position);
        if (distance < 0.2f && !enemy.canSeePlayer)
        {
            enemy.ChangeState(enemy.patrol);
        }
    }

    public override void ExitState(Enemy enemy)
    {

    }
}
