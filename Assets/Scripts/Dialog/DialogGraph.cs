using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogGraph", menuName = "Dialog/DialogGraph")]
public class DialogGraph : ScriptableObject
{
    public BaseDialogNode startNode;
    public List<BaseDialogNode> nodes = new List<BaseDialogNode>();
    public Vector2 startNodePosition = new Vector2(100, 100);
}
