using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance;

    public AudioClip buttonPressSound;
    public AudioClip tickSound;
    public AudioClip rainSound;
    public AudioClip itemPickupSound;
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
        Debug.Log($"sound volume: {gameSettings.soundVolume}");
        Debug.Log($"music volume: {gameSettings.musicVolume}");
    }
    private void Start()
    {
        UpdateSoundEffectsVolume(0.65f);
    }
    public void PlaySound(AudioClip clip)
    {
        // play any sound. just pass audio clip
        audioSource.PlayOneShot(clip);
    }
    public void PlayButtonPressSound()
    {
        audioSource.PlayOneShot(buttonPressSound);
    }
    public void PlayTickPressSound()
    {
        audioSource.PlayOneShot(buttonPressSound);
    }
    public void PlayPickItemSound()
    {
        audioSource.PlayOneShot(itemPickupSound);
    }
    public void UpdateSoundEffectsVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
}
