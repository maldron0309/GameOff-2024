using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;
    public List<AudioClip> backgroundTracks;
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
        PlayRandomTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }
    public void UpdateMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void PlayRandomTrack()
    {
        if (backgroundTracks.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, backgroundTracks.Count);
        audioSource.clip = backgroundTracks[randomIndex];
        audioSource.Play();
    }
}
