using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockTesting : MonoBehaviour
{
    [SerializeField] TMP_InputField correctCode;
    [SerializeField] TMP_Text outcome;
    [SerializeField] LockController lockCode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(int.Parse(correctCode.text) == lockCode.currentCode()) { outcome.text = "Current: Correct"; }
        else { outcome.text = "Current: Incorrect"; }
    }
}
