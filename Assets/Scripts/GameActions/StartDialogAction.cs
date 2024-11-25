using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogAction : BaseAction
{
    public DialogGraph dialogGraph;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void ActivateEffect()
    {
        DialogSystem.instance.StartDialog(dialogGraph);
    }
}
