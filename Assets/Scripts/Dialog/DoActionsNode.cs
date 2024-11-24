using UnityEngine;

[System.Serializable]
public class DoActionsNode : BaseDialogNode
{
    public string actionObjectName;

    public void Trigger()
    {
        GameObject triggerActions = GameObject.Find("TriggerActions");

        if (triggerActions != null)
        {
            Transform actionObject = triggerActions.transform.Find(actionObjectName);

            if (actionObject != null)
            {
                ActionsContainer actionsContainer = actionObject.GetComponent<ActionsContainer>();
                if (actionsContainer != null)
                {
                    actionsContainer.ProcessActions();
                }
                else
                {
                    Debug.LogError($"ActionsContainer not found on object: {actionObjectName}");
                }
            }
            else
            {
                Debug.LogError($"Action object '{actionObjectName}' not found under TriggerActions.");
            }
        }
        else
        {
            Debug.LogError("TriggerActions GameObject not found in the scene.");
        }
    }
}
