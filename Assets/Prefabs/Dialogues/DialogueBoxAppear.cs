using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxAppear : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Animator animator;

    private void OnEnable()
    {
        animator.SetTrigger("DialogueIn");
    }

}
