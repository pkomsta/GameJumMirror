using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected float _turnSpeed = 5.0f;
    public abstract void StartState(Enemy enemy);

    public abstract void UpdateState(Enemy enemy);

    public abstract void ExitState(Enemy enemy);
}
