using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{

    TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetTextBySlider(Slider slider)
    {
        int val = Mathf.RoundToInt(slider.value * 100);
        text.SetText(val.ToString());
    }
}
