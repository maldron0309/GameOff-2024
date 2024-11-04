using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class ChoiceNodeView : BaseNodeView
{
    public DecisionNode DecisionNode { get; private set; }

    // List to hold output ports for each option
    public List<TextField> OutputTexts { get; private set; } = new List<TextField>();  // List of TextFields for each option text

    public ChoiceNodeView(DecisionNode decisionNode)
    {
        this.style.width = 300;
        DecisionNode = decisionNode;
        title = "Choice Node";
        SetPosition(new Rect(decisionNode.position.x, decisionNode.position.y, 200, 150));

        InitializePorts();  // Initialize input and custom output ports

        // Display existing conditions
        DisplayConditions(DecisionNode);

        // Button to add new conditions
        Button addConditionButton = new Button(() => AddNewCondition(DecisionNode)) { text = "Add Condition" };
        mainContainer.Add(addConditionButton);

        // Add fields for each decision option and create a port for it
        for (int i = 0; i < DecisionNode.options.Count; i++)
        {
            AddOption(i);
        }

        // Add button to add more options
        Button addOptionButton = new Button(() => AddNewOption()) { text = "Add Option" };
        mainContainer.Add(addOptionButton);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void InitializePorts()
    {
        // Only add the input port, since output ports are custom
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(ChoiceNodeView));
        InputPort.portName = "Input";
        inputContainer.Add(InputPort);
    }

    private void AddOption(int index)
    {
        Port optionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        optionPort.portName = "";  // Hide label on port

        // Text field for option with a fixed width
        TextField optionField = new TextField { value = DecisionNode.options[index] };
        optionField.style.flexGrow = 1;
        optionField.RegisterValueChangedCallback(evt =>
        {
            DecisionNode.options[index] = evt.newValue;
        });

        // Container to hold the text field and port
        VisualElement container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        container.Add(optionField);
        container.Add(optionPort);
        outputContainer.Add(container);

        OutputPorts.Add(optionPort);
    }

    // Adds a new option (both UI field and port)
    private void AddNewOption()
    {
        // Add a new empty option to the DecisionNode data
        DecisionNode.options.Add("New Option");
        DecisionNode.nextNodes.Add(null);
        int newIndex = DecisionNode.options.Count - 1;

        // Add the corresponding field and output port
        AddOption(newIndex);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void SetPositionAndMarkDirty(BaseDialogNode node, Rect newPos)
    {
        base.SetPositionAndMarkDirty(DecisionNode, newPos);
    }
    public override BaseDialogNode GetNodeData()
    {
        return DecisionNode;
    }
}
