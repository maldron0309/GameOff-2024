using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateData", menuName = "Dialog/Game State Data")]
public class GameStateData : ScriptableObject
{
    [SerializeField]
    private List<GameStateEntry> stateEntries = new List<GameStateEntry>();

    private Dictionary<string, string> stateDictionary;

    private void OnEnable()
    {
        stateDictionary = new Dictionary<string, string>();
        foreach (var entry in stateEntries)
        {
            stateDictionary[entry.key] = entry.value;
        }
    }

    public bool TryGetValue(string key, out string value)
    {
        return stateDictionary.TryGetValue(key, out value);
        //foreach (var entry in stateEntries)
        //{
        //    if (entry.key == key)
        //    {
        //        value = entry.value;
        //        return true;
        //    }
        //}
        //value = "";
        //return false;
    }

    public void SetValue(string key, string value)
    {
        //foreach (var entry in stateEntries)
        //{
        //    if (entry.key == key)
        //        entry.value = value;
        //}
        if (stateDictionary.ContainsKey(key))
        {
            stateDictionary[key] = value;
        }
    }
}

[Serializable]
public class GameStateEntry
{
    public string key;
    public string value;
}
