using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI instance;
    private PlayerInventory linkedInventory;
    public GameObject container;
    public BaseItem SelectedItem;
    public int itemIdx;

    public GameObject itemsContainer;
    public GameObject itemSlotPrefab;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        linkedInventory = FindObjectOfType<PlayerInventory>();
        if (linkedInventory.items.Count > 0)
            itemIdx = 0;
        else
            itemIdx = -1;
        RefreshInventory();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            container.gameObject.SetActive(!container.gameObject.activeInHierarchy);
            if(container.gameObject.activeInHierarchy)
                RefreshInventory();
        }
        if (container.gameObject.activeInHierarchy)
        {
            if(linkedInventory.items.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    itemIdx = Mathf.Max(0, itemIdx - 1);
                    SelectedItem = linkedInventory.items[itemIdx];
                    RefreshInventory();
                    Debug.Log("Pick left item");
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    itemIdx = Mathf.Min(linkedInventory.items.Count - 1, itemIdx + 1);
                    SelectedItem = linkedInventory.items[itemIdx];
                    RefreshInventory();
                    Debug.Log("Pick right item");
                }
            }
            else
            {
                itemIdx = -1;
            }
        }
    }
    public void LinkInventory(PlayerInventory inventory)
    {
        linkedInventory = inventory;
        RefreshInventory();
    }
    public void RefreshInventory()
    {
        if (linkedInventory.items.Count > 0)
            itemIdx = 0;
        else
            itemIdx = -1;

        foreach (Transform child in itemsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < linkedInventory.items.Count; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemsContainer.transform);
            slot.transform.localPosition = Vector3.right * 100 * (i - itemIdx);
            slot.GetComponent<ItemSlotUI>().SetItem(linkedInventory.items[i]);
        }        
    }
}
