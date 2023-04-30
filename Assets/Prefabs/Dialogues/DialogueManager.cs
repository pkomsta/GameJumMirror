using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText, nickText;
    public GameObject panel;
    private string[] lines;
    private string[] dividedLine;
    private int currentIndex = 0;

    Coroutine coroutine;
    public IEnumerator waitAndWriteLetter(float delay, int index)
    {
        dividedLine = lines[currentIndex].Split();
        while (index < dividedLine.Length)
        {
            yield return new WaitForSeconds(delay);
            lines[currentIndex] = string.Join(" ", dividedLine.Take(index + 1).ToArray());
            index++;
        }

        if(currentIndex == dividedLine.Length)
        {
            StopCoroutine(coroutine);
            isTyping = false;
        }
    }

    bool isTyping = false;
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
                else if (lines[currentIndex].Contains("{"))
                {
                    int start = lines[currentIndex].LastIndexOf("{");
                    int end = lines[currentIndex].LastIndexOf("}") + 1;

                    nickText.text = lines[currentIndex].Substring(start + 1, end - 2);

                    lines[currentIndex] = lines[currentIndex]
                                          .Replace(lines[currentIndex]
                                          .Substring(start, end), "");
                }


                dialogueText.text = lines[currentIndex];
                if(!isTyping)
                {
                    coroutine = StartCoroutine(waitAndWriteLetter(0.2f, 0));
                    isTyping = true;
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
    }

    public void StartDialogue(string dialogueNameWithExtention)
    {
        panel.SetActive(true);
        string filePath = Application.streamingAssetsPath
                              + "/Dialogues/" + dialogueNameWithExtention;
        lines = File.ReadAllLines(filePath);
    }

    public void CloseDialogue()
    {
        panel.SetActive(false);
        dialogueText.text = "...";
        nickText.text = "...";
        currentIndex = 0;
    }
}
