using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsHelper : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider EffectVolume;
    public Slider MusicVolume;

    void Start()
    {
       // MasterVolume.value = SoundManager.Instance.masterVolume;
       // EffectVolume.value = SoundManager.Instance.effectVolume;
       // MusicVolume.value = SoundManager.Instance.musicVolume;

      //  MasterVolume.GetComponent<SliderText>().SetTextBySlider(MasterVolume);
      //  EffectVolume.GetComponent<SliderText>().SetTextBySlider(EffectVolume);
      //  MusicVolume.GetComponent<SliderText>().SetTextBySlider(MusicVolume);
    }

    private void OnEnable()
    {
        MasterVolume.value = SoundManager.Instance.masterVolume;
        EffectVolume.value = SoundManager.Instance.effectVolume;
        MusicVolume.value = SoundManager.Instance.musicVolume;
    }

    public void SaveSliderValues()
    {
        SoundManager.Instance.SaveVolumeSettings(MasterVolume.value, EffectVolume.value, MusicVolume.value);
    }

    
}
