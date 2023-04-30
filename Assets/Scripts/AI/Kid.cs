using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Kid : Enemy
{
    public UnityEvent OnStartChase;
    public void StartChase()
    {
        OnStartChase?.Invoke();
        ChangeState(chase);
    }
}
