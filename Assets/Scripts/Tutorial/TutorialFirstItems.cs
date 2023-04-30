using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFirstItems : MonoBehaviour
{
    public GameObject DialoguePanel;
    public string dialogueName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.FreezeGame();
            other.GetComponent<DialogueManager>().StartDialogueWithUnFreeze(dialogueName);
            gameObject.SetActive(false);

        }
    }
}
