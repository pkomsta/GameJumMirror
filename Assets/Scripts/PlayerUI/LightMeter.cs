using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightMeter : MonoBehaviour
{
    public Sprite[] LightStates;
    public static LightMeter instance;

    [HideInInspector]
    public float maxIntensity;

    public Image fill;
    Image image;

    private void Awake()
    {
        instance = this;
        image = GetComponent<Image>();
    }

    public void SetMaxItensity(float val)
    {
        maxIntensity = val;
    }
    public void SetLightState(float currentIntensity)
    {
        float val = Mathf.Round((currentIntensity / maxIntensity) * 100);
        fill.fillAmount = val / 100 ;
        if(val <= 33)
        {
            image.sprite = LightStates[0];
        }
        else if(val <= 66)
        {
            image.sprite = LightStates[1];

        }
        else
        {
            image.sprite = LightStates[2];

        }
    }
}
