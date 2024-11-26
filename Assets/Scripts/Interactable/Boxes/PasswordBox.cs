using UnityEngine;
using TMPro;

public class PasswordBox : BaseBox
{
    [SerializeField] private LockController lockController; 
    [SerializeField] private string correctPassword;
    public AudioClip correctCombinationSound;
    public AudioClip wrongCombinationSound;
    public ActionsContainer onSuccessActions;


    public override void Interact()
    {
        base.Interact();
        //PasswordManager.Instance.SetCurrentPasswordBox(this); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
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
            Debug.Log("The password is correct! The box has been opened.");
            SoundEffectsManager.Instance.PlaySound(correctCombinationSound);
            onSuccessActions.ProcessActions();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("The password is incorrect.");
            SoundEffectsManager.Instance.PlaySound(wrongCombinationSound);
        }
    }
}
