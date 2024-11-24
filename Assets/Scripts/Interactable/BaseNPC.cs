using UnityEngine;

public class BaseNPC : MonoBehaviour, IInteractable
{
    public DialogGraph dialogGraph;

    public void Interact()
    {
        DialogSystem.instance.StartDialog(dialogGraph);
    }
}
