using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Level 1")]
    public AudioClip[] musicLvl1;
    public AudioClip[] SFXLvl1;
    [Header("Level 2")]
    public AudioClip[] musicLvl2;
    public AudioClip[] SFXLvl2;

    [Header("Level 3")]
    public AudioClip[] musicLvl3;
    public AudioClip[] SFXLvl3;
    [Header("Level 4")]
    public AudioClip[] musicLvl4;
    public AudioClip[] SFXLvl4;
    [Header("Random time in seconds for screams/whispers")]
    public float minRange = 60f;
    public float maxRange = 120f;

    List<AudioClip[]> musicLists;
    List<AudioClip[]> SFXLists;

    int lvlIndex;

    AudioClip[] selectedMusicList;
    int currentMusicClip;
    AudioClip[] selectedSFXList;
    int currentSFXClip;

    bool ambientHasEnded = false;
    private void Awake()
    {
        instance = this;

        musicLists = new List<AudioClip[]>();
        musicLists.Add(musicLvl1);
        musicLists.Add(musicLvl2);
        musicLists.Add(musicLvl3);
        musicLists.Add(musicLvl4);

        SFXLists = new List<AudioClip[]>();
        SFXLists.Add(SFXLvl1);
        SFXLists.Add(SFXLvl2);
        SFXLists.Add(SFXLvl3);
        SFXLists.Add(SFXLvl4);

        currentMusicClip = 0;
        currentSFXClip = 0;
        // -1 dlatego ze scena 0 to main menu
    }

    private void Start()
    {
        lvlIndex = LevelManager.GetActiveSceneBuildIndex() - 1;
        selectedMusicList = musicLists[lvlIndex];
        selectedSFXList = SFXLists[lvlIndex];
        StartCoroutine(PlayerRandomAmbient());
        StartCoroutine(PlayerRandomMusicClip());

    }

    private void Update()
    {
       // TestRandomClip();

    }

    void TestRandomClip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int randomSFX = currentSFXClip;
            while (randomSFX == currentSFXClip)
            {
                randomSFX = Random.Range(0, selectedSFXList.Length - 1);
            }
            Debug.Log("Clip index:" + randomSFX + " clip name " + selectedSFXList[randomSFX].name);
            currentSFXClip = randomSFX;
            SoundManager.Instance.PlaySFX(selectedSFXList[randomSFX]);
        }
    }

    IEnumerator PlayerRandomAmbient()
    {
        
        float randomTime = Random.Range(minRange, maxRange);
        yield return new WaitForSeconds(randomTime);
        int randomSFX = currentSFXClip;
        while (randomSFX == currentSFXClip)
        {
            randomSFX = Random.Range(0, selectedSFXList.Length - 1);
        }
        currentSFXClip = randomSFX;
        SoundManager.Instance.PlaySFX(selectedSFXList[randomSFX]);
        yield return new WaitUntil(()=> !SoundManager.Instance.IsSFXPlaying());
        Debug.Log("SFX has ended!");
        StartCoroutine(PlayerRandomAmbient());
        yield break;

    }

    IEnumerator PlayerRandomMusicClip()
    {

        int randomMusicClip = currentMusicClip; 
        while(randomMusicClip == currentMusicClip)
        {
            randomMusicClip = Random.Range(0, selectedMusicList.Length - 1);
        }
        currentMusicClip = randomMusicClip;
        SoundManager.Instance.PlayMusic(selectedMusicList[randomMusicClip]);
        yield return new WaitUntil(() => !SoundManager.Instance.IsMusicPlaying());
        Debug.Log("Music has ended!");
        StartCoroutine(PlayerRandomMusicClip());
        yield break;

    }

}
