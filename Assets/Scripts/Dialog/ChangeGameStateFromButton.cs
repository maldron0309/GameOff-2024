using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class ChangeStateEntry
{
    public string key;
    public string value;
    [TextArea]
    public string buttonLabel;
}
public class ChangeGameStateFromButton : MonoBehaviour
{
    public ChangeStateEntry[] states;
    public GameStateData stateData;
    private int currentState = 0;
    void Start()
    {
        if(states.Length > 0)
        {
            SetState(0);
        }
    }
    public void SetState(int idx)
    {
        stateData.SetValue(states[idx].key, states[idx].value);
        GetComponentInChildren<TextMeshProUGUI>().text = states[idx].buttonLabel;
    }
    public void SetNextState()
    {
        currentState = (currentState + 1) % states.Length;
        SetState(currentState);
    }
}
