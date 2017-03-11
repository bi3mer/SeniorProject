using System;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    // TODO: When we figure out what we're doing with items, we should have Use return an item
    // for cases like fishing, were you can cath an item.
    public abstract void Use();
    public abstract void Equip();
    public abstract void Unequip();

    /// <summary>
    /// The name of the tool, eg. "fishing rod"
    /// </summary>
    public string ToolName
    {
        get;
        protected set;
    }

    /// <summary>
    /// Return true if the tools is being used.
    /// </summary>
    public bool InUse
    {
        get;
        protected set;
    }
}
