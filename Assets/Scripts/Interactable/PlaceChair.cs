using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceChair : MonoBehaviour, IInteractable
{
    public GameObject chairObj;
    public Transform playerLocation;
    public BaseItem chairItem;
    public BaseItem leverItem;
    public DialogGraph dialog;
    public void Interact()
    {
        PlayerInventory inventory = FindAnyObjectByType<PlayerInventory>();
        inventory.RemoveItem(chairItem);
        inventory.AddItem(leverItem);

        FindAnyObjectByType<PlayerMove>().transform.position = playerLocation.position;
        chairObj.SetActive(true);


        DialogSystem.instance.StartDialog(dialog);

        PlayerMove player = FindAnyObjectByType<PlayerMove>();
        player.canMove = false;

        BlackScreenUI.instance.FadeIn();
    }    
}
