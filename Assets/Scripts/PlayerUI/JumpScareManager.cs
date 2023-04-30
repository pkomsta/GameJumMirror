using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpScareManager : MonoBehaviour
{
    Image image;
    [SerializeField]
    private List<Sprite> jumpscaresList;

    public float showImageTime;
    [Header("Range for random time in seconds")]
    public float minRange;
    public float maxRange;

    int spriteIndex;
    int randomIndex;
    void Awake()
    {

        image = GetComponent<Image>();
        spriteIndex = 0;
        randomIndex = spriteIndex;
    }

    void Start()
    {
        StartCoroutine(ShowJumpScares());

    }

    IEnumerator ShowJumpScares()
    {

        float randomTime = Random.Range(minRange, maxRange);
        while(randomIndex == spriteIndex)
        {
            randomIndex = Random.Range(0, jumpscaresList.Count);
        }
        spriteIndex = randomIndex;
        image.sprite = jumpscaresList[randomIndex];
        image.enabled = true;
        yield return new WaitForSeconds(showImageTime);
        image.enabled = false;
        yield return new WaitForSeconds(randomTime);
        StartCoroutine(ShowJumpScares());


    }

}
