using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : PickupObject
{
    [SerializeField] float IncreaseValue = 33f;
    public override void PickUp(IsometricCharacterController isometricCharacterController)
    {
        isometricCharacterController.IncreaseLightIntensity(IncreaseValue);
        Destroy(this.gameObject);
    }
}
