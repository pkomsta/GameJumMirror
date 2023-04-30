using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowStateKid : State
{
    [SerializeField] float startChaseAfter = 1f;
    int index = 0;
    bool started = false;
    IsometricCharacterController player;
    public override void StartState(Enemy enemy)
    {
        player = enemy.playerRef.GetComponent<IsometricCharacterController>();
        StartCoroutine(StartChase());
    }

    public override void UpdateState(Enemy enemy)
    {
        if (!started) { return; }
        if (player.IsDead()) { }

        
        enemy.navMeshAgent.SetDestination(player.transform.position); 

        if (!enemy.GetAnimator().GetNextAnimatorStateInfo(0).IsName("Run"))
        {
            enemy.GetAnimator().CrossFade("Run", 0.2f);

        }



    }
    IEnumerator StartChase()
    {
        yield return new WaitForSeconds(startChaseAfter);
        started= true;
    }
    public override void ExitState(Enemy enemy)
    {
        enemy.previousState = enemy.patrol;

    }
}
