using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DoActionsNodeView : BaseNodeView
{
    public DoActionsNode DoActionsNode { get; private set; }

    public DoActionsNodeView(DoActionsNode doActionsNode)
    {
        this.DoActionsNode = doActionsNode;
        this.title = "Do Actions Node";
        this.style.width = 200;

        InitializePorts();
        InitializeInputFields();
        DisplayConditions(DoActionsNode);

        Button addConditionButton = new Button(() => AddNewCondition(DoActionsNode)) { text = "Add Condition" };
        mainContainer.Add(addConditionButton);

        RefreshExpandedState();
        RefreshPorts();
    }

    private void InitializeInputFields()
    {
        // Text field to input the name of the action object
        TextField actionObjectNameField = new TextField("Action Object Name")
        {
            value = DoActionsNode.actionObjectName
        };
        actionObjectNameField.RegisterValueChangedCallback(evt =>
        {
            DoActionsNode.actionObjectName = evt.newValue;
            EditorUtility.SetDirty(DoActionsNode);  // Ensure changes are saved
        });
        mainContainer.Add(actionObjectNameField);
    }

    public override void InitializePorts()
    {
        // Create input port
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        InputPort.portName = "Input";
        inputContainer.Add(InputPort);

        outputContainer.style.flexDirection = FlexDirection.Column;
        outputContainer.style.alignItems = Align.FlexEnd;  // Aligns ports to the right

        OutputPorts = new List<Port>();

        // Add output ports for existing nextNodes
        for (int i = 0; i < DoActionsNode.nextNodes.Count; i++)
        {
            CreateOutputPort(i);
        }
    }
    // Remove an output port and update nextNodes list
    public void RemoveOutputPort(int index)
    {
        OutputPorts[index].RemoveFromHierarchy();
        OutputPorts.RemoveAt(index);
        DoActionsNode.nextNodes.RemoveAt(index);

        // Rebuild output ports
        outputContainer.Clear();
        OutputPorts.Clear();
        for (int i = 0; i < DoActionsNode.nextNodes.Count; i++)
        {
            CreateOutputPort(i);
        }

        RefreshExpandedState();
        RefreshPorts();
    }
    public void CreateOutputPort(int index)
    {
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = (index + 1).ToString();

        // Create a remove button next to the port
        Button removeButton = new Button(() => RemoveOutputPort(index)) { text = "X" };

        // Add the button and port to a horizontal container
        VisualElement container = new VisualElement { style = { flexDirection = FlexDirection.Row } };
        container.Add(removeButton);
        container.Add(outputPort);
        outputContainer.Add(container);

        OutputPorts.Add(outputPort);
    }
    // Add a new output port
    public void AddNewOutputPort()
    {
        DoActionsNode.nextNodes.Add(null);  // Placeholder for new node
        CreateOutputPort(DoActionsNode.nextNodes.Count - 1);
        RefreshExpandedState();
        RefreshPorts();
    }
    public override void SetPositionAndMarkDirty(BaseDialogNode node, Rect newPos)
    {
        base.SetPositionAndMarkDirty(DoActionsNode, newPos);
    }
    public override BaseDialogNode GetNodeData()
    {
        return DoActionsNode;
    }
}
