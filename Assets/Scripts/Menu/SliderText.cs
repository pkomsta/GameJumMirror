using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{
    // Musi byc zlinkowane, inaczej z jakiegos powodu funkcja z Enable w SettingsHelperze jest szybsza niz GetComponent  w Awake
    [SerializeField]
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetTextBySlider(Slider slider)
    {
        int val = Mathf.RoundToInt(slider.value * 100);
        text.SetText(val.ToString());
    }
}
