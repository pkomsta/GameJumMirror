using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUICleaner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> menuList;


    public void CleanMenus()
    {
        foreach(GameObject obj in menuList)
        {
            obj.SetActive(false);
        }
        menuList[0].SetActive(true);
    }
}
