using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsContainer : MonoBehaviour
{
    public List<BaseAction> actions;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ProcessActions()
    {
        foreach (var item in actions)
        {
            item.ActivateEffect();
        }
    }
}
