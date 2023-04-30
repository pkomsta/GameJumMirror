using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStateKid : State
{
    public override void ExitState(Enemy enemy)
    {
        
    }

    public override void StartState(Enemy enemy)
    {
        //load level or something
        Debug.Log("Stunek the stupid kid hah !!");
        LevelManager.LoadGoodEnd();
    }

    public override void UpdateState(Enemy enemy)
    {
        
    }
}
