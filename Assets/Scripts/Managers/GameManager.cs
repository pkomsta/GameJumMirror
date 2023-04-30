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
    public bool isGamePaused = false;  
    public bool isGameFrozen = false;
    public GameObject UIMenu;
    public CurrentLevel currentLevel = CurrentLevel.level1;
    public Dictionary<CurrentLevel,List<Vector3>> shadowFolowPathsDict;
    // Start is called before the first frame update\
    private void Awake()
    {
        shadowFolowPathsDict = new Dictionary<CurrentLevel, List<Vector3>>();
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

    public void SavePlayerPosition(Vector3 position)
    {
        if (!shadowFolowPathsDict.ContainsKey(currentLevel))
        {
            shadowFolowPathsDict.Add(currentLevel, new List<Vector3>());
        }

        shadowFolowPathsDict[currentLevel].Add(position);
    }

}
