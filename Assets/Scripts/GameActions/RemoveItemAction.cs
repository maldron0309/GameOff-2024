using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveItemAction : BaseAction
{
    public BaseItem item;
    void Start()
    {

    }
    public override void ActivateEffect()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        inventory.RemoveItem(item);
    }
}
