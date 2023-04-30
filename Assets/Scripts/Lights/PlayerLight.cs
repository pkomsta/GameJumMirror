using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] float maxIntensity = 1500;
    public float intensityTakenPerTick = 1f;
    [HideInInspector]
    public float currentIntensity = 0;
    public float flicerStrength = 0.75f;
    public float flickerSpeed = 0.1f;
    [SerializeField] Light pointLight;
    public UnityEvent OnLightFadeed;
    bool lightFaded = false;
    void Start()
    {
        currentIntensity = maxIntensity;
        if(LightMeter.instance != null)
            LightMeter.instance.SetMaxItensity(maxIntensity);
        StartCoroutine(Flicker());
    }

    private void Update()
    {

    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float randomIntensity = Random.Range(currentIntensity * flicerStrength, currentIntensity);
            pointLight.intensity = randomIntensity;
           
            
            if(currentIntensity < 0)
            {
                OnLightFadeed?.Invoke();
                Debug.Log("Light faded...");
                yield break;
            }
            yield return new WaitForSeconds(flickerSpeed);
        }
    }


    public float GetCurrentIntensity()
    {
        return currentIntensity;
    }
    public float GetMaxIntensity()
    {
        return maxIntensity;
    }
    public void ChangeCurrentIntensity(float value)
    {
        currentIntensity = Mathf.Clamp(currentIntensity+value,0f,maxIntensity);
        if(LightMeter.instance != null)
            LightMeter.instance.SetLightState(currentIntensity);
    }

}
