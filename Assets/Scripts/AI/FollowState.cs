using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    [SerializeField] float folowAfterSeconds = 5f;
    Vector3 nextPos;
    int index = 0;
    bool started = false;
    IsometricCharacterController player;
    public override void StartState(Enemy enemy)
    {
        StartCoroutine(WaitForMove(enemy));
        player = enemy.playerRef.GetComponent<IsometricCharacterController>();
    }

    public override void UpdateState(Enemy enemy)
    {
        if (!started) { return; }
        if (player.IsDead()) return;
        float distance = Vector3.Distance(enemy.transform.position, nextPos);
        //Debug.Log(distance);
        if (distance < 1.5f)
        {
            index++;
            if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Idle"))
            {
                enemy.GetAnimator().CrossFade("Idle", 0.2f);

            }
            nextPos = GameManager.Instance.shadowFolowPathsDict[GameManager.Instance.currentLevel][index];
            enemy.navMeshAgent.SetDestination(nextPos); ;

        }
        else if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Walk"))
        {
            enemy.GetAnimator().CrossFade("Walk", 0.2f);

        }
        Quaternion _rotDirection = Quaternion.LookRotation(enemy.navMeshAgent.destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotDirection, Time.deltaTime);


    }
    private IEnumerator WaitForMove(Enemy enemy)
    {
        yield return new WaitForSeconds(folowAfterSeconds);
        nextPos = GameManager.Instance.shadowFolowPathsDict[GameManager.Instance.currentLevel][0];
        enemy.navMeshAgent.SetDestination(nextPos);
        started= true;
    }
    public override void ExitState(Enemy enemy)
    {
        enemy.previousState = enemy.patrol;

    }
}
