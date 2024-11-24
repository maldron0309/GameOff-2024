using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : BaseAction
{
    public GameObject obj;
    public bool newState;
    void Start()
    {
        
    }
    public override void ActivateEffect()
    {
        obj.gameObject.SetActive(newState);
    }
}
