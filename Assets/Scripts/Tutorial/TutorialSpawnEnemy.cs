using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnEnemy : MonoBehaviour
{
    
    public GameObject FirstEnemy;
    public string dialogueName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FirstEnemy.gameObject.SetActive(true);
            DialogueManager.Instance.StartDialogueWithFreeze(dialogueName);
            gameObject.SetActive(false);

        }
    }
}
