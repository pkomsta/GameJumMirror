using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public static bool IsMirrorUIActive = true;
    public static bool IsLightUIActive = true;
    private void Awake()
    {
        Instance = this;
    }
    
    public static void ActivateMirrorUI()
    {
        IsMirrorUIActive = true;
    }

    public static void DeactivateMirrorUI()
    {
        IsMirrorUIActive = false;
    }

    public static void ActivateLightUI()
    {
        IsLightUIActive = true;
    }

    public static void DeactivateLightUI()
    {
        IsLightUIActive = false;
    }

    public static bool GetMirrorUIState()
    {
        return IsMirrorUIActive;
    }

    public static bool GetLightUIState()
    {
        return IsLightUIActive;
    }
}
