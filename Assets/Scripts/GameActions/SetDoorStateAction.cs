using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDoorStateAction : BaseAction
{
    public DoorInfo door;
    public bool locked;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void ActivateEffect()
    {
        door.locked = locked;
    }
}
