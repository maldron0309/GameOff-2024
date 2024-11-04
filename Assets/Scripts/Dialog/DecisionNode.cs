using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecisionNode", menuName = "Dialog/DecisionNode")]
public class DecisionNode : BaseDialogNode
{
    public List<string> options = new List<string>();
    public string decisionName;

    //public override void TriggerEvent(PlayerCharacter player, GameStateData gameState)
    //{
    //}
    private void OnValidate()
    {
        UpdateNodeName();
    }

    public void UpdateNodeName()
    {
        name = string.IsNullOrEmpty(decisionName) ? "Decision" : $"Decision: {decisionName}";
    }
}
