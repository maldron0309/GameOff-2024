using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance;

    public AudioClip buttonPressSound;
    public AudioClip citySelectionSound;
    public AudioClip transportSelectionSound;
    public AudioClip newDaySound;
    public GameSettings gameSettings;

    private AudioSource audioSource;

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
        audioSource.playOnAwake = false;
    }
    private void Start()
    {
        UpdateSoundEffectsVolume(gameSettings.soundVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void UpdateSoundEffectsVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
