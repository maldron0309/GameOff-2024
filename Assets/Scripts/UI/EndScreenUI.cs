using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    public static EndScreenUI instance;
    public GameObject root;
    public TextMeshProUGUI endingLabel;
    public GameObject images1;
    public GameObject images2;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        root.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowSceen(string endingText)
    {
        root.SetActive(true);
        endingLabel.text = endingText;
    }
    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
        SoundEffectsManager.Instance.PlayButtonPressSound();
    }
}
