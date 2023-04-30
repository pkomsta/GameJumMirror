using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorMeter : MonoBehaviour
{
    [SerializeField]
    GameObject[] MirrorUses;
    [SerializeField]
    Sprite[] MirrorStates;

    Image image;
    int indexSprite = 0;
    int indexUses;
    public static MirrorMeter instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        image = GetComponent<Image>();
        indexUses = MirrorUses.Length-1;
    }

    public void ChangeMirrorUI()
    {
        DecreaseMirrorUses();
        ChangeMirrorState();
    }

    public void ActivateMirrorUI()
    {
        ShowUsesLeft();
        SetMirrorState();

    }
    private void ShowUsesLeft()
    {

        for(int index = MirrorUses.Length -1; index>=GameManager.mirrorUsesLeft; index--)
        {
            MirrorUses[index].SetActive(false);
        }
    }

    private void SetMirrorState()
    {
        if (GameManager.mirrorUsesLeft == 4)
            return;
        indexSprite = 4 - GameManager.mirrorUsesLeft + 1;
        image.sprite = MirrorStates[indexSprite];
        indexSprite++;
    }

    private void DecreaseMirrorUses()
    {
        MirrorUses[indexUses].SetActive(false);
        indexUses--;
    }
    private void ChangeMirrorState()
    {
        image.sprite = MirrorStates[indexSprite];
        indexSprite++;
    }
}
