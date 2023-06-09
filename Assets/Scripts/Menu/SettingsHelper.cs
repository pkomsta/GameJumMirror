using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsHelper : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider EffectVolume;
    public Slider MusicVolume;
    public Toggle LightToggle;
    public Toggle MirrorToggle;
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
        LightToggle.isOn = SettingsManager.GetLightUIState();
        MirrorToggle.isOn = SettingsManager.GetMirrorUIState();
    }

    public void SaveValues()
    {
        SoundManager.Instance.SaveVolumeSettings(MasterVolume.value, EffectVolume.value, MusicVolume.value);
        if (MirrorToggle.isOn == false && LightToggle.isOn == false)
        {
            SettingsManager.DeactivateBorder();
        }
        if (LightToggle.isOn)
        {
            SettingsManager.ActivateLightUI();
            SettingsManager.ActivateBorder();
        }
        else
        {
            SettingsManager.DeactivateLightUI();
        }
        if (MirrorToggle.isOn)
        {
            SettingsManager.ActivateMirrorUI();
            SettingsManager.ActivateBorder();

        }
        else
        {
            SettingsManager.DeactivateMirrorUI();
        }
        
    }

    
}
