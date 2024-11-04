using System;
using UnityEngine;

[System.Serializable]
public class StateCondition : BaseCondition
{
    public enum ConditionType
    {
        EqualTo,
        NotEqualTo
    }

    public string key;
    public string value;
    public ConditionType conditionType;

    public override bool Evaluate(GameStateData gameState)
    {
        string stateValue;
        if (gameState.TryGetValue(key, out stateValue))
        {
            switch (conditionType)
            {
                case ConditionType.EqualTo: return stateValue == value;
                case ConditionType.NotEqualTo: return stateValue != value;
            }
        }
        return false;
    }
}