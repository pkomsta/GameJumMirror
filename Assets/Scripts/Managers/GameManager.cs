using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    // Start is called before the first frame update\
    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        
    }
    public GameObject GetPlayer()
    {
        return player;
    }

}
