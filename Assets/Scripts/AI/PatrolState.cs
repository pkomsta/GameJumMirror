using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    
    public override void StartState(Enemy enemy)
    {
        enemy.coneOfVision.MeshRenderer.material = enemy.coneOfVision.Materials[0];
        enemy.navMeshAgent.SetDestination(enemy._path[0].position);
        enemy.coneOfVision.VisionRadius = enemy.coneOfVision.VisionRadiusSmall;
    }

    public override void UpdateState(Enemy enemy)
    {
        float distance = Vector3.Distance(enemy.navMeshAgent.destination, enemy._floorPointer.position);
        if (distance < 0.2f)
        {

            if (enemy._forwardFlag)
            {
                if(enemy._pathIndex < enemy._path.Count -1)
                    enemy._pathIndex++;
                enemy.navMeshAgent.SetDestination(enemy._path[enemy._pathIndex].position);
                if (enemy._pathIndex == enemy._path.Count - 1)
                {
                    enemy._forwardFlag = false;
                }
            }
            if (!enemy._forwardFlag)
            {
                if(enemy._pathIndex > 0)
                    enemy._pathIndex--;
                enemy.navMeshAgent.SetDestination(enemy._path[enemy._pathIndex].position);
                if (enemy._pathIndex == 0)
                {
                    enemy._forwardFlag = true;
                }
            }

        }
        Quaternion _rotDirection = Quaternion.LookRotation(enemy.navMeshAgent.destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotDirection, Time.deltaTime);

        if (enemy.canSeePlayer)
        {
            enemy.ChangeState(enemy.chase);
        }

    }

    public override void ExitState(Enemy enemy)
    {
        enemy.previousState = enemy.patrol;

    }
}
