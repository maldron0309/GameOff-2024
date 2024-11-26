using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInfo : MonoBehaviour
{

    public string dir;
    public GameObject to;
    public bool locked = true;
    public KeyItem keyItem;

    public void Unlock()
    {
        locked = false;
        Debug.Log("Door Unlocked");
    }
}
