using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Range(0, 1.0f)] public float masterVolume = 1f;
    [Range(0, 1.0f)] public float musicVolume = 1f;
    [Range(0, 1.0f)] public float effectVolume = 1f;

    public AudioSource EffectsSource;
    public AudioSource MusicSource;


    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    public static SoundManager Instance = null;


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


    void Update()
    {
        if (!SoundManager.Instance.MusicSource.isPlaying && MusicSource.clip != null)
        {
            PlayMusic(MusicSource.clip);
        }
    }
    public void Play(AudioClip clip)
    {
        EffectsSource.volume = effectVolume * masterVolume;
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.volume = musicVolume * masterVolume;
        MusicSource.clip = clip;
        MusicSource.Play();
    }
    public void PlayOnGivenAudioSource(AudioSource audioSource, params AudioClip[] clips)
    {

        if (!audioSource.isPlaying)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Length);
            audioSource.volume = effectVolume * masterVolume;
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
        }

    }
    public void StopOnGivenAudioSource(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    public void PlayOneShotOnGivenAudioSource(AudioSource audioSource, params AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(clips[randomIndex], effectVolume * masterVolume);

    }

    public void StartFadeCoroutine(AudioSource audioSource, float duration, float targetVolume)
    {
        StartCoroutine(StartFade(audioSource, duration, targetVolume));
    }

    public void VolumeChanged()
    {
        MusicSource.volume = musicVolume * masterVolume;
        EffectsSource.volume = effectVolume * masterVolume;
    }

    public void SaveVolumeSettings(float master, float effect, float music)
    {
        masterVolume = master;
        effectVolume = effect;
        musicVolume = music;

        VolumeChanged();
    }
    IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
