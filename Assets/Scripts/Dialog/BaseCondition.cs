using UnityEngine;

[System.Serializable]
public abstract class BaseCondition
{
    // Abstract method for evaluating conditions
    public abstract bool Evaluate(GameStateData gameState);
}
