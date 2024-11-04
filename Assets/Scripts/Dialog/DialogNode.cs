using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDialogNode : ScriptableObject
{
    public string nodeName;
    public List<BaseDialogNode> nextNodes = new List<BaseDialogNode>();
    [SerializeReference]
    public List<BaseCondition> conditions = new List<BaseCondition>();
    public Vector2 position; // Add this line
    public bool isConditionsFolded = true;  // Default to folded
    //public abstract void TriggerEvent(PlayerCharacter player, GameStateData gameState);

    public BaseDialogNode GetNextNode(GameStateData gameState)
    {
        foreach (var nextNode in nextNodes)
        {
            bool conditionsMet = true;

            foreach (var attributeCondition in nextNode.conditions)
            {
                if (!attributeCondition.Evaluate(gameState))
                {
                    conditionsMet = false;
                    break;
                }
            }

            if (conditionsMet)
            {
                return nextNode;
            }
        }
        return null;
    }
}

[CreateAssetMenu(fileName = "DialogNode", menuName = "Dialog/DialogNode")]
public class DialogNode : BaseDialogNode
{
    public string speakerName;
    [TextArea] public string dialogText;
    public Sprite speakerImage;

    //public override void TriggerEvent(PlayerCharacter player, GameStateData gameState)
    //{

    //}
    private void OnValidate()
    {
        UpdateNodeName();
    }
    public void UpdateNodeName()
    {
        if (string.IsNullOrEmpty(speakerName))
        {
            name = "Dialog";
        }
        else
        {
            string truncatedText = dialogText.Length > 20 ? dialogText.Substring(0, 20) + "..." : dialogText;
            name = $"Dialog: {speakerName} - {truncatedText}";
        }
    }
}