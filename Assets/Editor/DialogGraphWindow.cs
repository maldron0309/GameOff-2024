using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public enum ConditionType
{
    Attribute,
    State
}
public class DialogGraphWindow : EditorWindow
{
    private DialogGraph _dialogGraph;
    private Vector2 scrollPosition;
    private List<BaseDialogNode> nodes = new List<BaseDialogNode>();
    private Dictionary<BaseDialogNode, Rect> nodeRects = new Dictionary<BaseDialogNode, Rect>();
    private int nodeWidth = 200;
    private int nodeHeight = 100;
    private DialogGraphView _graphView;

    private BaseDialogNode selectedNode;
    private BaseDialogNode nodeToLink;

    private Vector2 panOffset;
    private float zoom = 1.0f;
    private const float zoomMin = 0.2f;
    private const float zoomMax = 3.0f;
    private Vector2 lastMousePosition;

    [MenuItem("Window/Dialog Graph")]
    public static void OpenWindow(DialogGraph dialogGraph)
    {
        DialogGraphWindow window = GetWindow<DialogGraphWindow>("Dialog Graph Editor");
        window._dialogGraph = dialogGraph;
        window.AddStyles();
        window.AddGraphWindow();
        window.LoadNodes();
        window.Show();
        window.titleContent = new GUIContent("Dialog Graph");
        window.LoadGraph();
    }
    public void AddGraphWindow()
    {
        _graphView = new DialogGraphView();

        _graphView.StretchToParentSize();
        var toolbar = new Toolbar();

        rootVisualElement.Add(_graphView);

        //var createButton = new Button(() => CreateNewDialogGraph()) { text = "New Graph" };
        //toolbar.Add(createButton);
        var loadButton = new Button(() => LoadGraph()) { text = "Load Graph" };
        toolbar.Add(loadButton);
        rootVisualElement.Add(toolbar);
    }
    public void AddStyles()
    {
        StyleSheet ss = (StyleSheet)EditorGUIUtility.Load("DialogSystem/DialogVariables.uss");
        //StyleSheet ss = (StyleSheet)EditorGUIUtility.Load("DialogSystem/DialogGraphViewStyles.uss");
        rootVisualElement.styleSheets.Add(ss);
    }
    private void OnDisable()
    {
        // Clean up the graph view
        rootVisualElement.Remove(_graphView);
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void LoadNodes()
    {
        nodes.Clear();
        nodeRects.Clear();
        var graphNodes = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_dialogGraph));
        foreach (var node in graphNodes)
        {
            if (node is BaseDialogNode dialogNode)
            {
                nodes.Add(dialogNode);
                nodeRects[dialogNode] = new Rect(dialogNode.position.x, dialogNode.position.y, nodeWidth, nodeHeight); // Default position
            }
        }
    }
    private void OnSelectionChanged()
    {
        if (Selection.activeObject is DialogGraph newGraph)
        {
            _dialogGraph = newGraph;
            LoadNodes();
            Repaint();
        }
    }
    private void LoadGraph()
    {
        //string path = EditorUtility.OpenFilePanel("Load Dialog Graph", "Assets", "asset");
        //if (!string.IsNullOrEmpty(path))
        //{
        //    path = path.Replace(Application.dataPath, "Assets");
        //    _dialogGraph = AssetDatabase.LoadAssetAtPath<DialogGraph>(path);
        //    _graphView.LoadGraph(_dialogGraph);
        //}
        if(_dialogGraph != null)
            _graphView.LoadGraph(_dialogGraph);
    }
}
