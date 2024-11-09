using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCondition : BaseCondition
{
    public enum ItemConditionType
    {
        HasItem,
        MissingItem
    }

    public BaseItem item;
    public ItemConditionType conditionType;

    public override bool Evaluate(GameStateData gameState)
    {
        PlayerInventory linkedInventory = GameObject.FindFirstObjectByType<PlayerInventory>();
        switch (conditionType)
        {
            case ItemConditionType.HasItem: return linkedInventory.items.Contains(item);
            case ItemConditionType.MissingItem: return !linkedInventory.items.Contains(item);
        }
        return false;
    }
}
