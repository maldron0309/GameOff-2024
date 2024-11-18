using UnityEngine;

public abstract class BaseBox : MonoBehaviour, IInteractable
{
    public virtual void Interact()
    {
        Debug.Log("Interacting with the base box."); 
    }

    protected void OpenBox()
    {
        Debug.Log("Box opened!"); 
    }
}
