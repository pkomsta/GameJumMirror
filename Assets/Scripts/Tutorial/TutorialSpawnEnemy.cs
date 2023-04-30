using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnEnemy : MonoBehaviour
{
    
    public GameObject FirstEnemy;
    public GameObject DialoguePanel;
    public string dialogueName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FirstEnemy.gameObject.SetActive(true);
            GameManager.Instance.FreezeGame();
            other.GetComponent<DialogueManager>().StartDialogueWithUnFreeze(dialogueName);
            gameObject.SetActive(false);

        }
    }
}
