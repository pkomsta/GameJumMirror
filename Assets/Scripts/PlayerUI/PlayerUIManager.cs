using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;
    public GameObject dialoguePanel;
    public GameObject border;
    public GameObject LightState;
    public GameObject MirrorState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("Start test");
        if(SettingsManager.GetMirrorUIState() == false && SettingsManager.GetLightUIState() == false)
        {
            border.SetActive(false);
            MirrorState.SetActive(false);
            LightState.SetActive(false);
        }
        else if (SettingsManager.GetMirrorUIState() == false)
        {
            MirrorState.SetActive(false);
        }
        else if (SettingsManager.GetLightUIState() == false)
        {
            LightState.SetActive(false);
        }
    }

  
    
}
