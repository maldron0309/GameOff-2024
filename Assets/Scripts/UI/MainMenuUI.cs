using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public SettingsUI settigns;
    public CreditsUI credits;

    public Button startButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitButton;
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        exitButton.onClick.AddListener(ExitGame);
    }
    public void OpenSettings()
    {
        settigns.OpenSettings();
    }
    public void OpenCredits()
    {
        credits.OpenCredits();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        // Change to scene to what is going to be game scene
        SceneManager.LoadScene("Main");
    }
}
