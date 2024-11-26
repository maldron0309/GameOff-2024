using UnityEngine;
using TMPro;

public class PasswordBox : BaseBox
{
    [SerializeField] private GameObject passwordUI; 
    [SerializeField] private LockController lockController; 
    [SerializeField] private string correctPassword; 

    private bool isPasswordUIActive = false; 

    public override void Interact()
    {
        base.Interact();
        PasswordManager.Instance.SetCurrentPasswordBox(this); 
        ShowPasswordUI(); 
    }

    private void Update()
    {
        if (isPasswordUIActive && Input.GetKeyDown(KeyCode.Return))
        {
            CheckPassword();
        }
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
            isPasswordUIActive = false; 
            Debug.Log("The password is correct! The box has been opened.");
        }
        else
        {
            Debug.Log("The password is incorrect.");
        }
    }

    private void ShowPasswordUI()
    {
        passwordUI.SetActive(true); 
        isPasswordUIActive = true;
    }
}
