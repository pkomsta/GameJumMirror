using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    [SerializeField] float flickerStrength = 0.75f;
    [SerializeField] float flickerSpeed = 0.2f;
    Light light;
    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    private void Update()
    {

    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float randomIntensity = Random.Range(light.intensity * flickerStrength,light.intensity);
            light.intensity = randomIntensity;

            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
