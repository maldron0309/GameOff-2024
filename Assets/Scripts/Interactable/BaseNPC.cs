using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : MonoBehaviour, IInteractable
{
    public DialogGraph dialog;
    public void Interact()
    {
        DialogSystem.instance.StartDialog(dialog);
    }
}
