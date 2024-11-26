using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPuzzleAction : MonoBehaviour, IInteractable
{
    public GameObject puzzle;
    void Start()
    {

    }

    public void Interact()
    {
        puzzle.SetActive(true);
    }
}
