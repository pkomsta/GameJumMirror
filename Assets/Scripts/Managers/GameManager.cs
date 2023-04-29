using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public bool isGamePaused;
    public GameObject UIMenu;
    // Start is called before the first frame update\
    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        UIMenu = GameObject.FindGameObjectWithTag("UIMenu");
        isGamePaused = false;
    }
    void Start()
    {
        
    }

    private void Update()
    {
        CheckForPause();
    }

    private void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1.0f;
            UIMenu.SetActive(false);
        }
        else
        {
            UIMenu.SetActive(true);
            UIMenu.GetComponent<GameUICleaner>().CleanMenus();
            isGamePaused = true;
            Time.timeScale = 0f;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

}
