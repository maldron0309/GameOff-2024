using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<BaseItem> items;
    void Start()
    {
        
    }
    public void AddItem(BaseItem item)
    {
        SoundEffectsManager.Instance.PlayPickItemSound();
        items.Add(item);
        PlayerInventoryUI.instance.RefreshInventory();
    }
    public void RemoveItem(BaseItem item)
    {
        if (items.Contains(item))
            items.Remove(item);
    }
    public BaseItem GetItem(int idx)
    {
        return items[idx];
    }
    public bool HasItem(BaseItem item)
    {
        return items.Contains(item);
    }
}
