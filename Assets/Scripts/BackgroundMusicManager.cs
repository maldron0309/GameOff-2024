using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;
    public AudioClip backgroundTrack;
    public AudioClip menuTrack;
    public AudioClip endingTrack;
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
        audioSource.loop = false;
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
        if (!audioSource.isPlaying)
        {
            PlayBackgroundTrack();
        }
    }
    public void UpdateMusicVolume(float volume)
    {
        audioSource.volume = volume;
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
    public void PlayEndingTrack()
    {
        audioSource.clip = endingTrack;
        audioSource.Play();
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
