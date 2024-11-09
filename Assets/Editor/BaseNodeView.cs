using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseNodeView : Node
{
    public Port InputPort { get; protected set; }
    public List<Port> OutputPorts { get; protected set; } = new List<Port>();
    protected Foldout conditionsFoldout;  // Store a reference to the conditions foldout

    public abstract void InitializePorts();
    public BaseNodeView()
    {
        // Set the background color of the entire node content
        this.style.backgroundColor = new StyleColor(new Color(0.1f, 0.1f, 0.1f, 1.0f));  // Dark gray background

        // Add padding and border styling if necessary
        this.style.paddingLeft = 5;
        this.style.paddingRight = 5;
        this.style.paddingTop = 5;
        this.style.paddingBottom = 5;

        this.style.borderTopWidth = 1;
        this.style.borderBottomWidth = 1;
        this.style.borderLeftWidth = 1;
        this.style.borderRightWidth = 1;

        this.style.borderTopColor = Color.black;
        this.style.borderBottomColor = Color.black;
        this.style.borderLeftColor = Color.black;
        this.style.borderRightColor = Color.black;
    }

    // Base method for setting position and marking the ScriptableObject as dirty
    public virtual void SetPositionAndMarkDirty(BaseDialogNode node, Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
        EditorUtility.SetDirty(node);  // Mark as dirty to save the changes
    }
    public virtual BaseDialogNode GetNodeData()
    {
        return null;
    }
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        // This should be called in each subclass with the correct node type (e.g., DialogNode, ActionNode, etc.)
        if (this is DialogNodeView dialogNodeView)
        {
            SetPositionAndMarkDirty(dialogNodeView.DataNode, newPos);
        }
        else if (this is ModifyStateNodeView actionNodeView)
        {
            SetPositionAndMarkDirty(actionNodeView.ModifyStateNode, newPos);
        }
        else if (this is ChoiceNodeView choiceNodeView)
        {
            SetPositionAndMarkDirty(choiceNodeView.DecisionNode, newPos);
        }
    }
    // Displays conditions (State and Attribute) for a given node
    protected void DisplayConditions(BaseDialogNode node)
    {
        if (conditionsFoldout != null)
        {
            mainContainer.Remove(conditionsFoldout);
        }

        conditionsFoldout = new Foldout
        {
            text = "Conditions",
            value = !node.isConditionsFolded
        };

        conditionsFoldout.RegisterValueChangedCallback(evt =>
        {
            node.isConditionsFolded = !evt.newValue;
            EditorUtility.SetDirty(node);  // Mark the node as dirty to save the foldout state
        });

        mainContainer.Add(conditionsFoldout);
        conditionsFoldout.Clear();

        // Loop through conditions and display each one with a remove button
        for (int i = 0; i < node.conditions.Count; i++)
        {
            BaseCondition condition = node.conditions[i];

            // Create a horizontal container to hold the remove button and condition fields
            VisualElement conditionRow = new VisualElement();
            conditionRow.style.flexDirection = FlexDirection.Row;  // Set horizontal layout
            conditionRow.style.marginBottom = 5;  // Add margin at the bottom

            // Add the remove button ("x")
            Button removeButton = new Button(() => RemoveCondition(node, condition))
            {
                text = "x",
                style =
            {
                marginRight = 5,  // Add a little spacing between the "x" button and the condition fields
                width = 20,       // Set button width to make it compact
                height = 20
            }
            };
            conditionRow.Add(removeButton);

            // Add condition fields (either StateCondition or AttributeCondition)
            if (condition is StateCondition stateCondition)
            {
                // Compact Key field
                VisualElement keyFieldRow = new VisualElement();
                keyFieldRow.style.flexDirection = FlexDirection.Row;

                Label keyLabel = new Label("Key:");
                keyLabel.style.minWidth = 40;  // Compact the label width
                keyFieldRow.Add(keyLabel);

                TextField keyField = new TextField { value = stateCondition.key };
                keyField.style.flexGrow = 1;  // Allow the input field to take up more space
                keyField.style.minWidth = 40;  // Compact the label width
                keyField.RegisterValueChangedCallback(evt => stateCondition.key = evt.newValue);
                keyFieldRow.Add(keyField);

                conditionRow.Add(keyFieldRow);

                // Compact Value field
                VisualElement valueFieldRow = new VisualElement();
                valueFieldRow.style.flexDirection = FlexDirection.Row;

                Label valueLabel = new Label("Value:");
                valueLabel.style.minWidth = 40;  // Compact the label width
                valueFieldRow.Add(valueLabel);

                TextField valueField = new TextField { value = stateCondition.value };
                valueField.style.flexGrow = 1;  // Allow the input field to take up more space
                valueField.style.minWidth = 40;  // Compact the label width
                valueField.RegisterValueChangedCallback(evt => stateCondition.value = evt.newValue);
                valueFieldRow.Add(valueField);

                conditionRow.Add(valueFieldRow);
            }
            if (condition is ItemCondition itemCondition)
            {
                ObjectField itemField = new ObjectField("Required Item")
                {
                    objectType = typeof(BaseItem),
                    value = itemCondition.item  // Set current value from the condition
                };

                itemField.RegisterValueChangedCallback(evt =>
                {
                    itemCondition.item = (BaseItem)evt.newValue;
                    EditorUtility.SetDirty(node);
                });

                conditionRow.Add(itemField);
            }
            // Add the horizontal condition row to the foldout
            conditionsFoldout.Add(conditionRow);
        }

        RefreshExpandedState();
        RefreshPorts();
    }

    // Adds a new condition to the node (default to StateCondition, can be extended)
    protected void AddNewCondition(BaseDialogNode node)
    {
        // Create a context menu with options to add either StateCondition or AttributeCondition
        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("State Condition"), false, () =>
        {
            StateCondition newStateCondition = new StateCondition();
            node.conditions.Add(newStateCondition);
            EditorUtility.SetDirty(node);  // Mark the node as dirty so the condition is saved
            DisplayConditions(node);  // Refresh the conditions foldout to include the new condition
        });
        menu.AddItem(new GUIContent("Item Condition"), false, () =>
        {
            ItemCondition newItemCondition = new ItemCondition();
            node.conditions.Add(newItemCondition);
            EditorUtility.SetDirty(node);
            DisplayConditions(node);
        });

        // Show the menu at the mouse position
        menu.ShowAsContext();
    }
    protected void RemoveCondition(BaseDialogNode node, BaseCondition condition)
    {
        // Remove the condition by reference, instead of relying on an index
        node.conditions.Remove(condition);

        // Mark the node as dirty so Unity saves the changes
        EditorUtility.SetDirty(node);

        // Refresh the conditions UI
        DisplayConditions(node);
    }
}
