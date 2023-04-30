using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandfatherPickaxe : PickupObject
{
    public string dialogue;
    public override void PickUp(IsometricCharacterController isometricCharacterController)
    {
        SettingsManager.ActivateMirrorUI();
        DialogueManager.Instance.StartDialogue(dialogue);
        Destroy(this.gameObject);
    }
}
