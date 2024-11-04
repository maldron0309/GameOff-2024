using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(DialogGraph))]
public class DialogGraphEditor : Editor
{
    private DialogGraph dialogGraph;
    private Vector2 scrollPosition;
    private List<BaseDialogNode> nodes = new List<BaseDialogNode>();
    private Dictionary<BaseDialogNode, Rect> nodeRects = new Dictionary<BaseDialogNode, Rect>();
    private int nodeWidth = 200;
    private int nodeHeight = 100;

    void OnEnable()
    {
        dialogGraph = (DialogGraph)target;
        nodes.Clear();
        nodeRects.Clear();
        CollectNodes(dialogGraph.startNode);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Dialog Graph Editor"))
        {
            DialogGraphWindow.OpenWindow(dialogGraph);
        }
    }

    private void CollectNodes(BaseDialogNode node)
    {
        if (node == null || nodes.Contains(node)) return;
        nodes.Add(node);
        nodeRects[node] = new Rect(0, 0, nodeWidth, nodeHeight);
        foreach (var nextNode in node.nextNodes)
        {
            CollectNodes(nextNode);
        }
    }
}