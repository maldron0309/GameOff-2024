using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : ScriptableObject
{
    public string ingameName;
    [TextArea]
    public string description;
    public Sprite icon;
}
