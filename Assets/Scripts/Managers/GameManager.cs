using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum CurrentLevel
{
    level1, level2, level3, level4, level5
}



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public static int mirrorUsesLeft = 4;
    public bool isGamePaused = false;  
    public bool isGameFrozen = false;
    public bool isPlayerDead = false;
    public GameObject UIMenu;
    public CurrentLevel currentLevel = CurrentLevel.level1;
    public Dictionary<CurrentLevel,List<Vector3>> shadowFolowPathsDict;

    // Start is called before the first frame update\
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

        shadowFolowPathsDict = new Dictionary<CurrentLevel, List<Vector3>>();
        player = GameObject.FindGameObjectWithTag("Player");
        UIMenu = GameObject.FindGameObjectWithTag("UIMenu");
        isPlayerDead = false;
        isGamePaused = false;
        Time.timeScale = 1.0f;

    }
    void Start()
    {
        isPlayerDead = false;
        isGamePaused = false;
        Time.timeScale = 1.0f;


    }

    private void Update()
    {
        CheckForPause();
    }

    private void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPlayerDead == false)
        {
            PauseGame();
        }
    }

    public void FreezeGame()
    {
        if (isGameFrozen)
        {
            isGameFrozen = false;

        }
        else
        {
            isGameFrozen = true;
        }
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1.0f;
            UIMenu.SetActive(false);
            UIMenu.GetComponent<HoverSelect>().HideStroke();

        }
        else
        {
            UIMenu.SetActive(true);
            UIMenu.GetComponent<GameUICleaner>().CleanMenus();
            isGamePaused = true;
            Time.timeScale = 0f;
        }
    }

    public void PlayerDied()
    {
        isPlayerDead = true;
        PauseGame();
        UIMenu.GetComponent<GameUICleaner>().GameOverMenu.SetActive(true);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SavePlayerPosition(Vector3 position)
    {
        if (!shadowFolowPathsDict.ContainsKey(currentLevel))
        {
            shadowFolowPathsDict.Add(currentLevel, new List<Vector3>());
        }

        shadowFolowPathsDict[currentLevel].Add(position);
    }

}
