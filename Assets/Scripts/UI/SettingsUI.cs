using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject root;
    public Slider soundVolumeSlider;
    public Slider musicVolumeSlider;
    public GameSettings gameSettings;
    public Button backButton;

    void Awake()
    {
        gameSettings.LoadSettings();
    }
    private void Start()
    {
        soundVolumeSlider.value = gameSettings.soundVolume;
        musicVolumeSlider.value = gameSettings.musicVolume;

        soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChange);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        backButton.onClick.AddListener(CloseSettings);
        backButton.onClick.AddListener(SoundEffectsManager.Instance.PlayButtonPressSound);
        CloseSettings();
    }

    public void OpenSettings()
    {
        root.SetActive(true);
    }
    public void CloseSettings()
    {
        root.SetActive(false);
        gameSettings.SaveSettings();
    }
    public void OnSoundVolumeChange(float value)
    {
        gameSettings.soundVolume = value;
        SoundEffectsManager.Instance.UpdateSoundEffectsVolume(value);
    }

    public void OnMusicVolumeChange(float value)
    {
        gameSettings.musicVolume = value;
        BackgroundMusicManager.Instance.UpdateMusicVolume(value);
    }
}
