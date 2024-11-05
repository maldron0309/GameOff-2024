using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogFromButton : MonoBehaviour
{
    public DialogGraph dialogGraph;
    public void StartDialog()
    {
        DialogSystem.instance.StartDialog(dialogGraph);
    }
}
