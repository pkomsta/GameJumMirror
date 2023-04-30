using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText, nickText;
    public GameObject panel;
    public Animator animator;
    public AudioClip talkingClip;
    private string[] lines;
    private string[] dividedLine;
    private int currentIndex = 0;
    bool isTyping = false;
    bool unFreezeFlag = false;
    AudioSource audioSource;
    GameObject usedTrigger;
    Coroutine coroutine;
    public static DialogueManager Instance;
    public UnityEvent OnCloseDialogue;
    private void Awake()
    {

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        panel = PlayerUIManager.Instance.dialoguePanel;
    }
    private void Update()
    {
        if(lines != null)
        {
            if (currentIndex < lines.Length)
            {
                if (lines[currentIndex].Contains("#END"))
                {
                    lines[currentIndex] = lines[currentIndex]
                                          .Replace("#END", "");
                    CloseDialogue();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    NextLine();
                }
            }
        }
    }

    public void NextLine()
    {
        currentIndex++;
        StartCoroutine(waitAndWriteLetter(0.1f, 0));
    }
    public IEnumerator waitAndWriteLetter(float delay, int index)
    {
        dividedLine = lines[currentIndex].Split();
        while (index < dividedLine.Length)
        {
            SoundManager.Instance.PlayClipOnGivenAudioSource(audioSource, talkingClip);
            yield return new WaitForSeconds(delay);
            lines[currentIndex] = string.Join(" ", dividedLine.Take(index + 1).ToArray());

            if (lines[currentIndex].Contains("{"))
            {
                int start = lines[currentIndex].LastIndexOf("{");
                int end = lines[currentIndex].LastIndexOf("}") + 1;

                nickText.text = lines[currentIndex].Substring(start + 1, end - 2);

                lines[currentIndex] = lines[currentIndex]
                                      .Replace(lines[currentIndex]
                                      .Substring(start, end), "");
            }

            dialogueText.text = lines[currentIndex];
            index++;
        }

        if (currentIndex == dividedLine.Length)
        {
            StopCoroutine(coroutine);
        }
    }

    public void StartDialogue(string dialogueNameWithExtention, GameObject trig)
    {
        if (isTyping)
            return;
        isTyping = true;
        usedTrigger = trig;
        usedTrigger.SetActive(false);
        panel.SetActive(true);
        string filePath = Application.streamingAssetsPath
                              + "/Dialogues/" + dialogueNameWithExtention;
        lines = File.ReadAllLines(filePath);
        StartCoroutine(waitAndWriteLetter(0.2f, 0));
    }

    public void StartDialogue(string dialogueNameWithExtention)
    {
        if (isTyping)
        {
            CloseDialogue();
        }
            
        isTyping = true;
        panel.SetActive(true);
        string filePath = Application.streamingAssetsPath
                              + "/Dialogues/" + dialogueNameWithExtention;
        lines = File.ReadAllLines(filePath);
        StartCoroutine(waitAndWriteLetter(0.2f, 0));
    }


    public void StartDialogueWithFreeze(string dialogueNameWithExtention)
    {
        if (isTyping)
        {
            CloseDialogue();
        }
        GameManager.Instance.FreezeGame();
        unFreezeFlag = true;
        isTyping = true;
        panel.SetActive(true);
        string filePath = Application.streamingAssetsPath
                              + "/Dialogues/" + dialogueNameWithExtention;
        lines = File.ReadAllLines(filePath);
        StartCoroutine(waitAndWriteLetter(0.2f, 0));
    }


    public void CloseDialogue()
    {
        if (unFreezeFlag)
        {
            unFreezeFlag = false;
            GameManager.Instance.FreezeGame();
        }

        isTyping = false;
        if(usedTrigger != null)
            usedTrigger.SetActive(true);

        panel.SetActive(false);
        dialogueText.text = "";
        nickText.text = "";
        currentIndex = 0;
        OnCloseDialogue?.Invoke();
    }
}
