using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActionWithItem : MonoBehaviour
{
    public BaseItem item;
    public ActionsContainer actions;
    private PlayerInventory inventory;
    void Start()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && inventory.HasItem(item))
        {
            actions.ProcessActions();
        }
    }
}
