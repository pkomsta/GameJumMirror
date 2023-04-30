using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundObject : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float playEvery = 10f;
    AudioSource source;

    private void Start()
    {
        source= GetComponent<AudioSource>();
        InvokeRepeating(nameof(PlayRandomSound), playEvery, playEvery);
    }

    private void PlayRandomSound()
    {
        SoundManager.Instance.PlayOneShotOnGivenAudioSource(source,audioClips);
    }
}
