using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour, IInteractable
{
    public BaseItem itemObj;

    public void Interact()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        inventory.AddItem(itemObj);
        Destroy(gameObject);
    }

    void Start()
    {
        
    }
}
