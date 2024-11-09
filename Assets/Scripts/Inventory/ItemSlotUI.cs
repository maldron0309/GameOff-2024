using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public BaseItem item;
    public void SetItem(BaseItem itm)
    {
        item = itm;
        icon.sprite = item.icon;
    }
}
