using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.LoadNextLevel();
        }
    }
}
