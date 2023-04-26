using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    float _timeSinceLastAttack = Mathf.Infinity;

    public override void StartState(Enemy enemy)
    {
        enemy.navMeshAgent.isStopped = true;
    }

    public override void UpdateState(Enemy enemy)
    {
        _timeSinceLastAttack += Time.deltaTime;
        if (IsNotInAttackRange(enemy) && _timeSinceLastAttack > enemy.GetAttackCooldown() && !enemy.GetAnimator().GetCurrentAnimatorStateInfo(1).IsName("Attack"))
        {
            enemy.ChangeState(enemy.movement);
            return;
        }


        if (_timeSinceLastAttack > enemy.GetAttackCooldown())
        {

           // SoundManager.Instance.StopOnGivenAudioSource(enemy.GetAudioSource());
            _timeSinceLastAttack = 0;
            enemy.GetAnimator().CrossFade(Enemy.AAttack, 0.3f);
        }
        else if (!enemy.GetAnimator().GetCurrentAnimatorStateInfo(1).IsName("Attack"))
        {

            Quaternion rotation = Quaternion.LookRotation(SetRotationTarget() - transform.position);

            // Smoothly rotate towards the player's position
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _turnSpeed * Time.deltaTime);
        }
    }

    private static bool IsNotInAttackRange(Enemy enemy)
    {
        return Vector3.Distance(enemy.transform.position, GameManager.Instance.GetPlayer().transform.position) > enemy.GetRange();

    }

    private static Vector3 SetRotationTarget()
    {
          return GameManager.Instance.GetPlayer().transform.position;
    }

    public override void ExitState(Enemy enemy)
    {

    }
}
