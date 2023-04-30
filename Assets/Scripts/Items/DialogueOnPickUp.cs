using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnPickUp : PickupObject
{
    public string dialogue;
    public override void PickUp(IsometricCharacterController isometricCharacterController)
    {

        DialogueManager.Instance.StartDialogue(dialogue);

    }
}
