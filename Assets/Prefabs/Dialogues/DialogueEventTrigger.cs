using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueEventTrigger : MonoBehaviour
{
    public string dialogueName;
    public UnityEvent OnDialogueEnd;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var dm = other.GetComponent<DialogueManager>();
            dm.StartDialogue(dialogueName, this.gameObject);
            dm.OnCloseDialogue.AddListener( StartEvent);

        }
    }
    private void StartEvent()
    {
        OnDialogueEnd?.Invoke();
        Destroy(this.gameObject);
    }
}
