using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class EdgeManipulator : ContextualMenuManipulator
{
    private Edge edge;

    public EdgeManipulator(Edge edge) : base(evt =>
    {
        evt.menu.AppendAction("Delete Link", action => DeleteEdge(edge));

        BaseNodeView outputNodeView = edge.output.node as BaseNodeView;
        BaseNodeView inputNodeView = edge.input.node as BaseNodeView;

        // Ensure we are working with valid input/output
        if (outputNodeView is DialogNodeView dialogNodeView && inputNodeView != null)
        {
            BaseDialogNode dialogNode = dialogNodeView.GetNodeData();
            BaseDialogNode connectedNode = inputNodeView.GetNodeData();
            int index = dialogNode.nextNodes.IndexOf(connectedNode);

            // Add options to move node up or down
            if (index > 0)
            {
                evt.menu.AppendAction("Move Up", action => MoveNodeUp(dialogNodeView, index));
            }
            if (index < dialogNode.nextNodes.Count - 1)
            {
                evt.menu.AppendAction("Move Down", action => MoveNodeDown(dialogNodeView, index));
            }
        }
    })
    {
        this.edge = edge;
    }

    public static void DeleteEdge(Edge edge)
    {
        BaseNodeView outputNodeView = edge.output.node as BaseNodeView;
        BaseNodeView inputNodeView = edge.input.node as BaseNodeView;

        if (outputNodeView is DialogNodeView dialogNodeView && inputNodeView != null)
        {
            BaseDialogNode dialogNode = dialogNodeView.GetNodeData();
            BaseDialogNode connectedNode = inputNodeView.GetNodeData();

            // Remove the connection from nextNodes
            if (dialogNode.nextNodes.Contains(connectedNode))
            {
                dialogNode.nextNodes.Remove(connectedNode);
                EditorUtility.SetDirty(dialogNode);
            }
        }

        edge.RemoveFromHierarchy();  // Remove the edge from the graph visually
    }

    public static void MoveNodeUp(DialogNodeView dialogNodeView, int index)
    {
        BaseDialogNode dialogNode = dialogNodeView.GetNodeData();

        // Swap with the node above
        if (index > 0)
        {
            var temp = dialogNode.nextNodes[index - 1];
            dialogNode.nextNodes[index - 1] = dialogNode.nextNodes[index];
            dialogNode.nextNodes[index] = temp;

            EditorUtility.SetDirty(dialogNode);  // Mark the node as dirty so Unity saves the changes
            UpdateEdgeLabels(dialogNodeView);
        }
    }

    public static void MoveNodeDown(DialogNodeView dialogNodeView, int index)
    {
        BaseDialogNode dialogNode = dialogNodeView.GetNodeData();

        // Swap with the node below
        if (index < dialogNode.nextNodes.Count - 1)
        {
            var temp = dialogNode.nextNodes[index + 1];
            dialogNode.nextNodes[index + 1] = dialogNode.nextNodes[index];
            dialogNode.nextNodes[index] = temp;

            EditorUtility.SetDirty(dialogNode);  // Mark the node as dirty so Unity saves the changes
            UpdateEdgeLabels(dialogNodeView);
        }
    }

    // Update edge labels after reordering
    public static void UpdateEdgeLabels(DialogNodeView dialogNodeView)
    {
        BaseDialogNode dialogNode = dialogNodeView.GetNodeData();

        // Iterate over all edges of the dialog node and update the order numbers
        foreach (var edge in dialogNodeView.outputContainer.Query<Edge>().ToList())
        {
            BaseNodeView inputNodeView = edge.input.node as BaseNodeView;
            if (inputNodeView != null)
            {
                var connectedNode = inputNodeView.GetNodeData();
                int newIndex = dialogNode.nextNodes.IndexOf(connectedNode);

                // Update the edge label to reflect the new order
                Label edgeLabel = edge.Q<Label>();
                if (edgeLabel != null)
                {
                    edgeLabel.text = (newIndex + 1).ToString();
                }
            }
        }
    }
}
