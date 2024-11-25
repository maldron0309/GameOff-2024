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
        // activate while inside dialog
        foreach (var item in actions)
        {
            item.ActivateEffect();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ActivateObject on collision while outside dialog
        if (collision.CompareTag("Player"))
        {
            foreach (var item in actions)
            {
                item.ActivateEffect();
            }
        }
    }
}
