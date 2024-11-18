using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepInBed : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        StartCoroutine(Sleep()) ;
    }
    public IEnumerator Sleep()
    {
        PlayerMove player = FindAnyObjectByType<PlayerMove>();
        player.canMove = false;
        BlackScreenUI.instance.FadeOut();
        yield return new WaitForSeconds(2);

        // add other action that should happen when character goes to sleep

        print("WaitAndPrint " + Time.time);
        BlackScreenUI.instance.FadeIn();
        yield return new WaitForSeconds(1);
        player.canMove = true;
    }
    
}
