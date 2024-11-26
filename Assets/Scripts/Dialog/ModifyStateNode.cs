using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action Node", menuName = "Dialog/Action Node")]
public class ModifyStateNode : BaseDialogNode
{
    public string actionName;
    public GameStateData gameState;
    public List<GameStateEntry> gameStateChanges = new List<GameStateEntry>();

    public void TriggerEvent()
    {
        foreach (var entry in gameStateChanges)
        {
            if (gameState != null)
                gameState.SetValue(entry.key, entry.value);
            else
                DialogSystem.instance.gameData.SetValue(entry.key, entry.value);
        }
    }
    private void OnValidate()
    {
        UpdateNodeName();
    }

    public void UpdateNodeName()
    {
        name = string.IsNullOrEmpty(actionName) ? "Action" : $"Action: {actionName}";
    }

}
