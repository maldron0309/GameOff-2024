using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    public GameObject root;
    public Button backButton;
    void Start()
    {
        backButton.onClick.AddListener(CloseCredits);
        backButton.onClick.AddListener(SoundEffectsManager.Instance.PlayButtonPressSound);
    }
    public void CloseCredits()
    {
        root.SetActive(false);
    }
    public void OpenCredits()
    {
        root.SetActive(true);
    }
}
