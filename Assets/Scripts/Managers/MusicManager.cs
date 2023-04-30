using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Level 1")]
    public AudioClip[] musicLvl1;
    [Header("Level 2")]
    public AudioClip[] musicLvl2;
    [Header("Level 3")]
    public AudioClip[] musicLvl3;
    [Header("Level 4")]
    public AudioClip[] musicLvl4;

    private void Awake()
    {
        instance = this;
    }

}
