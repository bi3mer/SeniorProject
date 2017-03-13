using System;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
	[SerializeField]
	[BaseItemPopup]
    protected string toolName;

    protected const string unequipActName = "Unequip";

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

    /// <summary>
    /// Sets up the tool so that it is linked to the proper item in the inventory.
    /// </summary>
    /// <param name="itemForTool">Item for tool.</param>
    public virtual void SetUpTool(BaseItem itemForTool)
    {
    }
}
