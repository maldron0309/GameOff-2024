using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    public static PasswordManager Instance; 

    private PasswordBox currentPasswordBox; 

    private void Awake()
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
    }

    public void SetCurrentPasswordBox(PasswordBox passwordBox)
    {
        currentPasswordBox = passwordBox;
    }

    public void CheckCurrentPassword()
    {
        currentPasswordBox?.CheckPassword(); 
    }
}
