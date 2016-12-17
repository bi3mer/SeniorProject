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
    /// The name of the tool type, eg. "fishing rod"
    /// </summary>
    public string ToolType
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

    /// <summary>
    /// Compares to another Tool to check equality. 
    /// Tools are equal if they have the same type.
    /// </summary>
    /// <param name="tool">Tool to compare to.</param>
    /// <returns>True if the tools arre equal.</returns>
    public bool Equals (Tool tool)
    {
        return (this.ToolType.Equals(tool.ToolType));
    }
}
