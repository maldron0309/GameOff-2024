using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOrRemoveItemOnButton : MonoBehaviour
{
    public BaseItem item;
    public void AddRemoveItem()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        if (inventory.items.Contains(item))
        {
            inventory.RemoveItem(item);
        }
        else
        {
            inventory.AddItem(item);
        }
        PlayerInventoryUI.instance.RefreshInventory();
    }
}
