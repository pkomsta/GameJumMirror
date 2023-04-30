using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public static bool IsMirrorUIActive = false;
    public static bool IsLightUIActive = true;
    public static bool isBorderActive = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    public static void ActivateMirrorUI()
    {
        IsMirrorUIActive = true;
        if(PlayerUIManager.Instance != null)
            PlayerUIManager.Instance.MirrorState.SetActive(true);

    }

    public static void DeactivateMirrorUI()
    {
        IsMirrorUIActive = false;
        if (PlayerUIManager.Instance != null)
            PlayerUIManager.Instance.MirrorState.SetActive(false);
    }

    public static void ActivateLightUI()
    {
        IsLightUIActive = true;
        if (PlayerUIManager.Instance != null)
            PlayerUIManager.Instance.LightState.SetActive(true);

    }

    public static void DeactivateLightUI()
    {
        IsLightUIActive = false;
        if (PlayerUIManager.Instance != null)
            PlayerUIManager.Instance.LightState.SetActive(false);

    }
    public static void ActivateBorder()
    {
        isBorderActive = true;
        if (PlayerUIManager.Instance != null)
            PlayerUIManager.Instance.border.SetActive(true);

    }
    public static void DeactivateBorder()
    {
        isBorderActive = false;
        if (PlayerUIManager.Instance != null)
            PlayerUIManager.Instance.border.SetActive(false);

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
