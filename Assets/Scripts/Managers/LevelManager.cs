using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{


    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static int GetActiveSceneBuildIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
    public static void LoadGoodEnd()
    {
        SceneManager.LoadScene(4);
    }
    public static void LoadBadEnd()
    {
        SceneManager.LoadScene(3);
    }
}
