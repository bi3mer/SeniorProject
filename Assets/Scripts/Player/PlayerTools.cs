using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Used for keeping track of tools the player has, 
/// what they currently have equipped, and switching tools.
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
        AvailableTools = new List<Tool>();
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
        set
        {
            // Unequip and equip the tools.
            switchTool(equippedTool, value);

            equippedTool = value;
        }
    }

    /// <summary>
    /// List of tools available to the player to equip.
    /// </summary>
    public List<Tool> AvailableTools
    {
        get;
        private set;
    }

    /// <summary>
    /// Load a list of tool types, and equips the current tool.
    /// </summary>
    /// <param name="toolTypes">List of tool types to load.</param>
    public void LoadTools (string currentlyEquippedTool, List<string> toolTypes)
    {
        for (int i = 0; i < toolTypes.Count; ++i)
        {
            Tool tool = AddTool(toolTypes[i]);
            if (tool.ToolType.Equals(currentlyEquippedTool))
            {
                EquippedTool = tool;
            }
        }
    }

    /// <summary>
    /// Add a tool of the given type to the list of tools 
    /// available to the player.
    /// </summary>
    /// <param name="toolType">The type of tool.</param>
    /// <returns>The tool functionality.</returns>
    public Tool AddTool (string toolType)
    {
        Tool tool = null;

        // Get the correponding script containing the tool functionality
        // tool == null will terminate the loop early if we find the tool.
        for (int i = 0; i < possibleTools.Length && tool == null; ++i)
        {
            Tool potentialTool = possibleTools[i];
            if (potentialTool.ToolType.Equals(toolType))
            {
                tool = potentialTool;
            }
        }
  
        if (tool != null)
        {
            // Add to the list of available tools
            AvailableTools.Add(tool);
        }
        else
        {
            // Error if we didin't yet account for this tooltype
            Debug.LogError("Tool type " + toolType + " not implemented.");
        }

        return tool;
    }

    /// <summary>
    /// Remove instance of tool type from the list of available tools.
    /// </summary>
    /// <param name="toolType">The tool type to remove.</param>
    public void RemoveTool (string toolType)
    {
        for (int i = 0; i < AvailableTools.Count; ++i)
        {
            Tool tool = AvailableTools[i];

            if (tool.ToolType.Equals(toolType))
            {
                AvailableTools.Remove(tool);
            }
        }
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
            // TODO: Wait for unequip to finish
        }
        
        if (next != null)
        {
            next.Equip();
            // TODO: Wait for equip to finish
        }
    }
}
