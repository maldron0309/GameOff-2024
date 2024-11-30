using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;
    public AudioClip backgroundTrack;
    public AudioClip menuTrack;
    public AudioClip endingTrack1;
    public AudioClip endingTrack2;
    private AudioSource audioSource;
    public GameSettings gameSettings;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        audioSource.volume = gameSettings.musicVolume;
        //PlayBackgroundTrack();
    }

    void Update()
    {
        //if (!audioSource.isPlaying)
        //{
        //    PlayBackgroundTrack();
        //}
    }
    public void UpdateMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void PlayBackgroundTrack()
    {
        audioSource.clip = backgroundTrack;
        audioSource.Play();
    }
    public void PlayMenuTrack()
    {
        audioSource.clip = menuTrack;
        audioSource.Play();
    }
    public void PlayEndingTrack1()
    {
        audioSource.clip = endingTrack1;
        audioSource.Play();
    }
    public void PlayEndingTrack2()
    {
        audioSource.clip = endingTrack2;
        audioSource.Play();
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
