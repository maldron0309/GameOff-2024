using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Hi");
    }
}
