using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ModifyStateNodeView : BaseNodeView
{
    public ModifyStateNode ModifyStateNode { get; private set; }
    //public List<Port> OutputPorts { get; private set; }

    public ModifyStateNodeView(ModifyStateNode modifyStateNode)
    {
        ModifyStateNode = modifyStateNode;
        title = "Modify State Node";
        SetPosition(new Rect(modifyStateNode.position.x, modifyStateNode.position.y, 200, 150));

        InitializePorts();  // Initialize input and output ports

        // Display the game state changes
        DisplayGameStateChanges();

        // Add a button to add new game state modifications
        Button addStateModButton = new Button(() => AddNewStateMod()) { text = "Add State Mod" };
        mainContainer.Add(addStateModButton);

        Button addOutputButton = new Button(() => AddNewOutputPort()) { text = "Add Output" };
        extensionContainer.Add(addOutputButton);

        RefreshExpandedState();
        RefreshPorts();
    }

    public override void InitializePorts()
    {
        InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(ModifyStateNodeView));
        InputPort.portName = "Input";
        inputContainer.Add(InputPort);

        //OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(ModifyStateNodeView));
        //OutputPort.portName = "Output";
        //outputContainer.Add(OutputPort);
        OutputPorts = new List<Port>();

        for (int i = 0; i < ModifyStateNode.nextNodes.Count; i++)
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            outputPort.portName = (i + 1).ToString();  // Use incremental numbers as labels
            outputContainer.Add(outputPort);
            OutputPorts.Add(outputPort);
        }
    }

    // Display the gameStateChanges list
    private Foldout gameStateFoldout;

    private void DisplayGameStateChanges()
    {
        // Remove existing foldout if it's already created
        if (gameStateFoldout != null)
        {
            mainContainer.Remove(gameStateFoldout);
        }

        // Create a new foldout to display gameStateChanges
        gameStateFoldout = new Foldout
        {
            text = "State Modifications",
            value = true  // Default to expanded
        };

        mainContainer.Add(gameStateFoldout);
        gameStateFoldout.Clear();  // Clear foldout before re-populating

        // Loop through each GameStateEntry and add fields
        for (int i = 0; i < ModifyStateNode.gameStateChanges.Count; i++)
        {
            var entry = ModifyStateNode.gameStateChanges[i];

            // Create a container for the entry row (remove button + fields)
            VisualElement entryRow = new VisualElement();
            entryRow.style.flexDirection = FlexDirection.Row;
            entryRow.style.marginBottom = 5;

            // Add the remove button ("x")
            Button removeButton = new Button(() => RemoveStateMod(i))
            {
                text = "x",
                style = { width = 20, height = 20, marginRight = 5 }
            };
            entryRow.Add(removeButton);

            // Create fields for key and value
            TextField keyField = new TextField("Key") { value = entry.key };
            keyField.style.flexGrow = 1;
            keyField.RegisterValueChangedCallback(evt => entry.key = evt.newValue);
            entryRow.Add(keyField);

            TextField valueField = new TextField("Value") { value = entry.value };
            valueField.style.flexGrow = 1;
            valueField.RegisterValueChangedCallback(evt => entry.value = evt.newValue);
            entryRow.Add(valueField);

            // Add the entry row to the foldout
            gameStateFoldout.Add(entryRow);
        }

        RefreshExpandedState();
        RefreshPorts();
    }

    // Add a new GameStateEntry to the list
    private void AddNewStateMod()
    {
        ModifyStateNode.gameStateChanges.Add(new GameStateEntry { key = "NewKey", value = "NewValue" });
        EditorUtility.SetDirty(ModifyStateNode);  // Mark the node as dirty to save the changes
        DisplayGameStateChanges();  // Refresh the list
    }

    // Remove a GameStateEntry by index
    private void RemoveStateMod(int index)
    {
        ModifyStateNode.gameStateChanges.RemoveAt(index);
        EditorUtility.SetDirty(ModifyStateNode);  // Mark the node as dirty to save the changes
        DisplayGameStateChanges();  // Refresh the list
    }

    public override void SetPositionAndMarkDirty(BaseDialogNode node, Rect newPos)
    {
        base.SetPositionAndMarkDirty(ModifyStateNode, newPos);
    }
    public override BaseDialogNode GetNodeData()
    {
        return ModifyStateNode;
    }
    // Reusable method to create an output port with a removal button
    public void CreateOutputPort(int index)
    {
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = (index + 1).ToString();

        Button removeButton = new Button(() => RemoveOutputPort(index)) { text = "X" };

        VisualElement container = new VisualElement { style = { flexDirection = FlexDirection.Row } };
        container.Add(removeButton);
        container.Add(outputPort);
        outputContainer.Add(container);

        OutputPorts.Add(outputPort);
    }
    private void AddOutputPort(int index)
    {
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = (index + 1).ToString();  // Incremental number as label

        // Add the remove button to the port's visual
        Button removeButton = new Button(() => RemoveOutputPort(index)) { text = "X" };
        VisualElement container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        container.Add(removeButton);
        container.Add(outputPort);
        outputContainer.Add(container);

        OutputPorts.Add(outputPort);
    }

    private void AddNewOutputPort()
    {
        ModifyStateNode.nextNodes.Add(null);  // Add a placeholder for the new node
        AddOutputPort(ModifyStateNode.nextNodes.Count - 1);
        RefreshExpandedState();
        RefreshPorts();
    }

    private void RemoveOutputPort(int index)
    {
        OutputPorts[index].RemoveFromHierarchy();
        OutputPorts.RemoveAt(index);

        // Remove the corresponding node from the nextNodes list
        ModifyStateNode.nextNodes.RemoveAt(index);

        // Rebuild output ports with updated labels
        outputContainer.Clear();
        OutputPorts.Clear();
        for (int i = 0; i < ModifyStateNode.nextNodes.Count; i++)
        {
            AddOutputPort(i);
        }

        RefreshExpandedState();
        RefreshPorts();
    }
}
