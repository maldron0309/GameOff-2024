using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class StartNodeView : Node
{
    private DialogGraph dialogGraph;
    public Port outputPort;

    public StartNodeView(DialogGraph graph)
    {
        dialogGraph = graph;

        title = "Start Node";

        // Set the background color to dark green
        this.style.backgroundColor = new StyleColor(new Color(0.1f, 0.5f, 0.1f, 1.0f));

        // Create the output port
        outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(StartNodeView));
        outputPort.portName = "Next";
        outputContainer.Add(outputPort);

        RefreshExpandedState();
        RefreshPorts();
    }

    // Set the start node on connection
    public void SetStartNode(BaseNodeView targetNodeView)
    {
        dialogGraph.startNode = targetNodeView.GetNodeData();
        EditorUtility.SetDirty(dialogGraph);
    }

    // Save position to DialogGraph when moved
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        dialogGraph.startNodePosition = newPos.position;  // Save the position in the graph
        EditorUtility.SetDirty(dialogGraph);
    }
}
