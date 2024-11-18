using UnityEngine;
using TMPro;

public class PasswordBox : BaseBox
{
    [SerializeField] private GameObject passwordUI; 
    [SerializeField] private LockController lockController; 
    [SerializeField] private string correctPassword; 
    [SerializeField] private TMP_Text resultText; 

    public override void Interact()
    {
        base.Interact();
        PasswordManager.Instance.SetCurrentPasswordBox(this); 
        ShowPasswordUI(); 
    }

    public void CheckPassword()
    {
        int inputPassword = lockController.currentCode();
        int correctPasswordInt;

        if (!int.TryParse(correctPassword, out correctPasswordInt))
        {
            Debug.LogError("Invalid correct password format.");
            return;
        }

        if (inputPassword == correctPasswordInt)
        {
            OpenBox(); 
            passwordUI.SetActive(false); 
            Debug.Log("The password is correct! The box has been opened.");
        }
        else
        {
            resultText.text = "Fail"; 
            Debug.Log("The password is incorrect.");
        }
    }

    private void ShowPasswordUI()
    {
        passwordUI.SetActive(true); 
    }
}
