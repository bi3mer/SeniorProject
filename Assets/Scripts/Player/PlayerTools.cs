using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Used for keeping track of implementation of tools in the game scene.
/// </summary>
public class PlayerTools
{
    private Tool equippedTool;
    private Tool[] possibleTools;

    /// <summary>
    /// Create a PlayerTools object.
    /// </summary>
    /// <param name="possibleToolsList">The list of Tools in the scene.</param>
	public PlayerTools (Tool[] possibleToolsList)
    {
        equippedTool = null;
        possibleTools = possibleToolsList;

        // Subscribe to equipped item events
        PlayerInventory inventory = Game.Instance.PlayerInstance.Inventory;
        inventory.ItemEquippedSubscription += (item) =>
        {
            EquippedTool = GetToolByBaseItem(item);
        };
        inventory.ItemUnequippedSubscription += () =>
        {
            EquippedTool = null;
        };
    }

    /// <summary>
    /// Returns true if the player has a tool 
    /// equipped (EquippedTool is not null). 
    /// </summary>
    public bool HasEquipped 
    {
        get
        {
            return (EquippedTool != null);
        }
    }

    /// <summary>
    /// The tool equipped by the player.
    /// </summary>
    public Tool EquippedTool
    {
        get
        {
            return equippedTool;
        }
        
        private set
        {
            // Unequip and equip the tools.
            switchTool(equippedTool, value);

            equippedTool = value;
        }
    }

    /// <summary>
    /// Iterates through tools and checks for an implementation of the BaseItem.
    /// </summary>
    /// <param name="item">The BaseItem to find.</param>
    /// <returns>The implemenation, or null if the item has not been implemented.</returns>
    public Tool GetToolByBaseItem(BaseItem item)
    {
        Tool tool = null;

        // Get the correponding script containing the tool functionality
        // tool == null will terminate the loop early if we find the tool.
        for (int i = 0; i < possibleTools.Length && tool == null; ++i)
        {
            Tool potentialTool = possibleTools[i];
            if (potentialTool.ToolName.Equals(item.ItemName))
            {
                tool = potentialTool;
				tool.SetUpTool(item);
            }
        }

        // Error if we didin't yet account for this tooltype
        if (tool == null)
        {
            Debug.LogError("Tool type " + item.ItemName + " not implemented.");
        }

        return tool;
    } 

    /// <summary>
    /// Run sequence of unequipped previous tool and equipping current tool.
    /// 
    /// TODO: Once we have equip/unequip animations, make sure we wait 
    /// for one to finish before triggering the next animation.
    /// </summary>
    /// <param name="previous">The tool being unequipped.</param>
    /// <param name="next">The tool being equipped.</param>
    private void switchTool (Tool previous, Tool next)
    {
        if (previous != null)
        {
            previous.Unequip();
            GuiInstanceManager.EquippedItemGuiInstance.Unequipped();
            // TODO: Wait for unequip to finish
        }
        
        if (next != null)
        {
            next.Equip();
            // TODO: Wait for equip to finish
        }
    }
}
