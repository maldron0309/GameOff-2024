using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;

public class DialogGraphView : GraphView
{
    private DialogGraph dialogGraph;
    private StartNodeView startNodeView;
    public DialogGraphView()
    {
        AddManipulators();
        AddGridBackground();
        ApplyStyles();
    }
    public void AddGridBackground()
    {
        GridBackground gb = new GridBackground();

        gb.StretchToParentSize();

        Insert(0, gb);
    }
    public void ApplyStyles()
    {
        StyleSheet ss = (StyleSheet)EditorGUIUtility.Load("DialogSystem/DialogGraphViewStyles.uss");
        styleSheets.Add(ss);
    }
    public void AddManipulators()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        // Allow connections between nodes
        this.graphViewChanged = OnGraphViewChanged;

        // Add custom contextual menu manipulator (for right-click menu)
        this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
        style.flexGrow = 1;
    }
    private GraphViewChange OnGraphViewChanged(GraphViewChange change)
    {
        if (change.edgesToCreate != null)
        {
            foreach (var edge in change.edgesToCreate)
            {
                var outputNodeView = edge.output.node as BaseNodeView;
                var inputNodeView = edge.input.node as BaseNodeView;
                ChoiceNodeView choiceNodeOut = edge.output.node as ChoiceNodeView;

                // Handle connection from StartNodeView
                if (edge.output.node is StartNodeView startNodeView && edge.input.node is BaseNodeView)
                {
                    // Set the connected node as the starting node in the DialogGraph
                    startNodeView.SetStartNode(inputNodeView);
                }
                else if (choiceNodeOut != null && inputNodeView != null)
                {
                    // Find the index of the output port (matches the option in DecisionNode)
                    int optionIndex = choiceNodeOut.OutputPorts.IndexOf(edge.output);

                    if (optionIndex >= 0 && optionIndex < choiceNodeOut.DecisionNode.options.Count)
                    {
                        // Set the next node for the specific option in DecisionNode
                        choiceNodeOut.DecisionNode.nextNodes[optionIndex] = inputNodeView.GetNodeData();
                        EditorUtility.SetDirty(choiceNodeOut.DecisionNode);
                    }
                } 
                else if (outputNodeView != null && inputNodeView != null)
                {
                    int optionIndex = outputNodeView.OutputPorts.IndexOf(edge.output);

                    if (optionIndex >= 0 && optionIndex < outputNodeView.GetNodeData().nextNodes.Count)
                    {
                        // Set the next node for the specific option in DecisionNode
                        outputNodeView.GetNodeData().nextNodes[optionIndex] = inputNodeView.GetNodeData();
                        EditorUtility.SetDirty(outputNodeView.GetNodeData());
                    }
                }
            }
        }

        // Handle node deletion
        if (change.elementsToRemove != null)
        {
            change.elementsToRemove.RemoveAll(element => element == startNodeView);
            foreach (GraphElement element in change.elementsToRemove)
            {
                // Handle node deletion
                if (element is BaseNodeView nodeView)
                {
                    // Remove the node from the dialogGraph data
                    dialogGraph.nodes.Remove(nodeView.GetNodeData());

                    // Clean up any references to this node in other nodes
                    foreach (BaseDialogNode node in dialogGraph.nodes)
                    {
                        // If the node is a DialogNode, remove the deleted node from its nextNodes list
                        if (node is DialogNode dialogNode)
                        {
                            dialogNode.nextNodes.Remove(nodeView.GetNodeData());
                            EditorUtility.SetDirty(dialogNode);  // Mark dirty to save changes
                        }
                        // If the node is a DecisionNode (ChoiceNode), clean up nextNodes for each option
                        else if (node is DecisionNode decisionNode)
                        {
                            for (int i = 0; i < decisionNode.nextNodes.Count; i++)
                            {
                                if (decisionNode.nextNodes[i] == nodeView.GetNodeData())
                                {
                                    decisionNode.nextNodes[i] = null;
                                    EditorUtility.SetDirty(decisionNode);  // Mark dirty to save changes
                                }
                            }
                        }
                    }
                    // Remove the node from the asset database
                    ScriptableObject nodeObject = nodeView.GetNodeData() as ScriptableObject;
                    if (nodeObject != null)
                    {
                        Object.DestroyImmediate(nodeObject, true);  // Destroy the ScriptableObject node
                    }

                    // Mark the graph as dirty so the deletion is saved
                    EditorUtility.SetDirty(dialogGraph);
                }
            }
        }

        return change;
    }

    public void LoadGraph(DialogGraph graph)
    {
        this.dialogGraph = graph;
        ClearGraph();

        // Ensure StartNode is always created and displayed

        Dictionary<BaseDialogNode, BaseNodeView> nodeViewLookup = new Dictionary<BaseDialogNode, BaseNodeView>();

        // Loop through nodes in the saved DialogGraph and create corresponding nodes
        foreach (BaseDialogNode savedNode in graph.nodes)
        {
            BaseNodeView nodeView = null;
            Vector2 position = new Vector2(savedNode.position.x, savedNode.position.y);

            // Check node type and create the appropriate view
            if (savedNode is DialogNode dialogNode)
            {
                nodeView = new DialogNodeView(dialogNode);
                nodeView.SetPositionAndMarkDirty(dialogNode, new Rect(position, new Vector2(200, 150)));
                AddElement(nodeView);
                nodeViewLookup[dialogNode] = nodeView;
            }
            else if (savedNode is ModifyStateNode actionNode)
            {
                nodeView = new ModifyStateNodeView(actionNode);
                nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
                AddElement(nodeView);
            }
            else if (savedNode is DecisionNode decisionNode)
            {
                nodeView = new ChoiceNodeView(decisionNode);
                nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
                AddElement(nodeView);
            }
            else if (savedNode is DoActionsNode doActionsNode)
            {
                nodeView = new DoActionsNodeView(doActionsNode);
                nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
                AddElement(nodeView);
            }

            if (nodeView != null)
            {
                AddElement(nodeView);
                nodeViewLookup[savedNode] = nodeView;  // Add the node to the dictionary
            }
        }

        CreateStartNode(nodeViewLookup);
        EstablishLinks(nodeViewLookup, graph);
    }
    // This method handles establishing links between nodes
    private void EstablishLinks(Dictionary<BaseDialogNode, BaseNodeView> nodeViewLookup, DialogGraph graph)
    {
        // Loop through the nodes to establish connections (edges) between them
        foreach (var nodeViewPair in nodeViewLookup)
        {
            BaseDialogNode node = nodeViewPair.Key;
            BaseNodeView nodeView = nodeViewPair.Value;

            if (node is DialogNode dialogNode)
            {
                DialogNodeView dialogNodeView = nodeView as DialogNodeView;
                if (dialogNodeView != null)
                {
                    for (int i = 0; i < dialogNode.nextNodes.Count; i++)
                    {
                        var nextNode = dialogNode.nextNodes[i];
                        if (nextNode != null && nodeViewLookup.TryGetValue(nextNode, out BaseNodeView nextNodeView))
                        {
                            Edge edge = dialogNodeView.OutputPorts[i].ConnectTo(nextNodeView.InputPort);
                            AddElement(edge);
                        }
                    }
                }
            }
            else if (node is ModifyStateNode modifyStateNode)
            {
                ModifyStateNodeView modifyStateNodeView = nodeView as ModifyStateNodeView;

                if (modifyStateNodeView != null)
                {
                    for (int i = 0; i < modifyStateNode.nextNodes.Count; i++)
                    {
                        var nextNode = modifyStateNode.nextNodes[i];
                        if (nextNode != null && nodeViewLookup.TryGetValue(nextNode, out BaseNodeView nextNodeView))
                        {
                            Edge edge = modifyStateNodeView.OutputPorts[i].ConnectTo(nextNodeView.InputPort);
                            AddElement(edge);
                        }
                    }
                }
            }
            else if (node is DecisionNode decisionNode)
            {
                // For each option in the DecisionNode, connect the output port to the next node
                for (int i = 0; i < decisionNode.nextNodes.Count; i++)
                {
                    var nextNode = decisionNode.nextNodes[i];

                    if (nextNode != null && nodeViewLookup.TryGetValue(nextNode, out BaseNodeView nextNodeView))
                    {
                        ChoiceNodeView choiceNodeView = nodeView as ChoiceNodeView;

                        if (choiceNodeView != null && i < choiceNodeView.OutputPorts.Count)
                        {
                            // Create and connect the edge from the option's output port
                            Edge edge = choiceNodeView.OutputPorts[i].ConnectTo(nextNodeView.InputPort);
                            AddElement(edge);

                            // Color the edge based on its position in the nextNodes list
                            Color edgeColor = CalculateGradientColor(i, decisionNode.nextNodes.Count);
                            ColorEdge(edge, edgeColor);
                        }
                    }
                }
            }
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        // Iterate through all ports in the graph
        ports.ForEach((port) =>
        {
            // Ensure we're not trying to connect the port to itself
            if (startPort != port && startPort.node != port.node)
            {
                // Ensure compatibility: Output port can only connect to input port, and vice versa
                if (startPort.direction == Direction.Output && port.direction == Direction.Input)
                {
                    compatiblePorts.Add(port);
                }
            }
        });

        return compatiblePorts;
    }

    public void ClearGraph()
    {
        graphElements.ForEach(RemoveElement);
    }
    // Build the custom context menu
    private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var target = evt.target;

        if (target is Edge edge)  // Check if right-clicked on an edge
        {
            // Get the output and input node views from the edge
            BaseNodeView outputNodeView = edge.output.node as BaseNodeView;
            BaseNodeView inputNodeView = edge.input.node as BaseNodeView;

            if (outputNodeView is DialogNodeView dialogNodeView && inputNodeView != null)
            {
                BaseDialogNode dialogNode = dialogNodeView.GetNodeData();
                BaseDialogNode connectedNode = inputNodeView.GetNodeData();
                int index = dialogNode.nextNodes.IndexOf(connectedNode);

                // Add options for moving the node in the nextNodes list
                if (index > 0)
                {
                    evt.menu.AppendAction("Move Up", action => EdgeManipulator.MoveNodeUp(dialogNodeView, index));
                }
                if (index < dialogNode.nextNodes.Count - 1)
                {
                    evt.menu.AppendAction("Move Down", action => EdgeManipulator.MoveNodeDown(dialogNodeView, index));
                }

                // Add the option to delete the edge
                evt.menu.AppendAction("Delete Link", action => EdgeManipulator.DeleteEdge(edge));
            }
        }
        else
        {
            Vector2 mousePosition = this.contentViewContainer.WorldToLocal(evt.mousePosition);
            // Add menu items for adding nodes at the click position
            evt.menu.AppendAction("Add Dialog Node", action => AddDialogNode(mousePosition));
            evt.menu.AppendAction("Add Modify State Node", action => AddModifyStateNode(mousePosition));
            evt.menu.AppendAction("Add Choice Node", action => AddChoiceNode(mousePosition));
            evt.menu.AppendAction("Add Do Actions Node", action => AddNode<DoActionsNode>(mousePosition));  // Add DoActionsNode
            evt.menu.AppendAction("Add Do Actions Node2", action => AddDoAcctionsNode(mousePosition));  // Add DoActionsNode
        }
    }

    private void AddNode<T>(Vector2 position) where T : BaseDialogNode, new()
    {
        T newNode = new T { position = position };
        dialogGraph.nodes.Add(newNode);
        EditorUtility.SetDirty(dialogGraph);
        LoadGraph(dialogGraph);  // Refresh graph to display the new node
    }
    private void AddDoAcctionsNode(Vector2 position)
    {
        // Create a new ChoiceNode scriptable object
        DoActionsNode newNode = ScriptableObject.CreateInstance<DoActionsNode>();
        newNode.name = "Do Actions";
        newNode.position = position;
        newNode.nextNodes.Add(null);
        dialogGraph.nodes.Add(newNode);

        // Mark the dialogGraph as dirty so Unity saves the changes
        AssetDatabase.AddObjectToAsset(newNode, dialogGraph);
        EditorUtility.SetDirty(dialogGraph);

        // Create the node view and add it to the graph
        DoActionsNodeView nodeView = new DoActionsNodeView(newNode);
        nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
        AddElement(nodeView);
    }

    // Add Dialog Node
    private void AddDialogNode(Vector2 position)
    {
        // Create a new DialogNode scriptable object
        DialogNode newNode = ScriptableObject.CreateInstance<DialogNode>();
        newNode.name = "New Dialog Node";
        newNode.position = position;
        dialogGraph.nodes.Add(newNode);

        // Mark the dialogGraph as dirty so Unity saves the changes
        AssetDatabase.AddObjectToAsset(newNode, dialogGraph);
        EditorUtility.SetDirty(dialogGraph);

        // Create the node view and add it to the graph
        DialogNodeView nodeView = new DialogNodeView(newNode);
        nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
        AddElement(nodeView);
    }

    // Add Modify State Node
    private void AddModifyStateNode(Vector2 position)
    {
        // Create a new ModifyStateNode scriptable object
        ModifyStateNode newNode = ScriptableObject.CreateInstance<ModifyStateNode>();
        newNode.name = "New Modify State Node";
        newNode.position = position;
        dialogGraph.nodes.Add(newNode);

        // Mark the dialogGraph as dirty so Unity saves the changes
        AssetDatabase.AddObjectToAsset(newNode, dialogGraph);
        EditorUtility.SetDirty(dialogGraph);

        // Create the node view and add it to the graph
        ModifyStateNodeView nodeView = new ModifyStateNodeView(newNode);
        nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
        AddElement(nodeView);
    }

    // Add Choice Node
    private void AddChoiceNode(Vector2 position)
    {
        // Create a new ChoiceNode scriptable object
        DecisionNode newNode = ScriptableObject.CreateInstance<DecisionNode>();
        newNode.name = "New Choice Node";
        newNode.position = position;
        newNode.options.Add("Option 1");  // Add a default option
        newNode.nextNodes.Add(null);
        dialogGraph.nodes.Add(newNode);

        // Mark the dialogGraph as dirty so Unity saves the changes
        AssetDatabase.AddObjectToAsset(newNode, dialogGraph);
        EditorUtility.SetDirty(dialogGraph);

        // Create the node view and add it to the graph
        ChoiceNodeView nodeView = new ChoiceNodeView(newNode);
        nodeView.SetPosition(new Rect(position, new Vector2(200, 150)));
        AddElement(nodeView);
    }
    // Automatically create and display the StartNode
    private void CreateStartNode(Dictionary<BaseDialogNode, BaseNodeView> nodeViewLookup)
    {
        // Create the StartNodeView and position it
        startNodeView = new StartNodeView(dialogGraph);
        startNodeView.SetPosition(new Rect(dialogGraph.startNodePosition, new Vector2(200, 100)));
        AddElement(startNodeView);
        if (dialogGraph.startNode != null)
        {
            Edge edge = startNodeView.outputPort.ConnectTo(nodeViewLookup[dialogGraph.startNode].InputPort);
            AddElement(edge);
        }
    }
    private void ColorEdge(Edge edge, Color color)
    {
        edge.edgeControl.inputColor = color;
        edge.edgeControl.outputColor = color;
        edge.edgeControl.toCapColor = color;
        edge.edgeControl.fromCapColor = color;
        edge.edgeControl.edgeWidth = 8;

        edge.MarkDirtyRepaint();
    }
    private Color CalculateGradientColor(int index, int total)
    {
        // Define light gray for the first node and dark gray for the last node
        //Color lightGray = new Color(0.8f, 0.8f, 0.8f);
        //Color darkGray = new Color(0.2f, 0.2f, 0.2f);
        Color lightGray = new Color(0.8f, 0.0f, 0.0f);
        Color darkGray = new Color(0.0f, 0.8f, 0.0f);

        if (total == 1)
        {
            return new Color(0.8f, 0.0f, 0.0f);
        }

        // Calculate the gradient color for intermediate nodes
        float t = (float)index / (total - 1);  // Normalize index to a 0-1 range
        return Color.Lerp(lightGray, darkGray, t);  // Linearly interpolate between light gray and dark gray
    }
}
