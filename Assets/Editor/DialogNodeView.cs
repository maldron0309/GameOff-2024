using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNodeView : BaseNodeView
{
    public string speakerName;
    public string dialogText;
    public DialogNode DataNode { get; private set; }
    //public List<Port> OutputPorts { get; private set; }

    public DialogNodeView(DialogNode dialogNode)
    {
        this.DataNode = dialogNode;
        this.title = "Dialog Node";
        this.style.width = 300;

        InitializePorts();  // Initialize input and output ports
        DisplayConditions(dialogNode);

        Button addConditionButton = new Button(() => AddNewCondition(dialogNode)) { text = "Add Condition" };
        mainContainer.Add(addConditionButton);

        // Text field for editing speakerName
        TextField speakerNameField = new TextField("Speaker Name");
        speakerNameField.value = dialogNode.speakerName;
        speakerNameField.RegisterValueChangedCallback(evt =>
        {
            dialogNode.speakerName = evt.newValue;
            title = dialogNode.speakerName + ": " + dialogNode.dialogText.Substring(0, Mathf.Min(dialogNode.dialogText.Length, 10)) + "...";
        });
        mainContainer.Add(speakerNameField);

        // Text area for editing dialogText
        TextField dialogTextField = new TextField("Dialog Text") { multiline = true };
        dialogTextField.value = dialogNode.dialogText;
        dialogTextField.RegisterValueChangedCallback(evt =>
        {
            dialogNode.dialogText = evt.newValue;
        });
        mainContainer.Add(dialogTextField);

        // ObjectField for selecting speakerImage sprite
        ObjectField spriteField = new ObjectField("Speaker Image")
        {
            objectType = typeof(Sprite),
            value = DataNode.speakerImage  // Load the current sprite
        };
        spriteField.RegisterValueChangedCallback(evt =>
        {
            DataNode.speakerImage = (Sprite)evt.newValue;
            EditorUtility.SetDirty(DataNode);  // Mark the DataNode as dirty to save changes
        });
        mainContainer.Add(spriteField);

        // Add button to add new output port
        Button addOutputButton = new Button(() => AddNewOutputPort()) { text = "Add Output" };
        extensionContainer.Add(addOutputButton);

        RefreshExpandedState();
        RefreshPorts();
    }

    // Reusable method to create an output port with a removal button
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
        DataNode.nextNodes.Add(null);  // Placeholder for new node
        CreateOutputPort(DataNode.nextNodes.Count - 1);
        RefreshExpandedState();
        RefreshPorts();
    }

    // Remove an output port and update nextNodes list
    public void RemoveOutputPort(int index)
    {
        OutputPorts[index].RemoveFromHierarchy();
        OutputPorts.RemoveAt(index);
        DataNode.nextNodes.RemoveAt(index);

        // Rebuild output ports
        outputContainer.Clear();
        OutputPorts.Clear();
        for (int i = 0; i < DataNode.nextNodes.Count; i++)
        {
            CreateOutputPort(i);
        }

        RefreshExpandedState();
        RefreshPorts();
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
        for (int i = 0; i < DataNode.nextNodes.Count; i++)
        {
            CreateOutputPort(i);
        }
    }
    public override BaseDialogNode GetNodeData()
    {
        return DataNode;
    }
}