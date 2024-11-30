using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public DialogGraph dialogGraph;
    public bool triggerOnce;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogSystem.instance.StartDialog(dialogGraph);
            if (triggerOnce == true)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
