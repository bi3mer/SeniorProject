using System.Collections;
using System.Collections.Generic;

public class ItemYAMLMap
{
	/// <summary>
	/// Gets or sets the name of the note.
	/// </summary>
	/// <value>The name of the note.</value>
	public string NoteName
	{ 
		get; 
		set; 
	}

    /// <summary>
	/// Gets or sets the text of the note.
	/// </summary>
	/// <value>The text of the note.</value>
	public string Text
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the sprite representation of the item in the inventory.
	/// </summary>
	/// <value>The inventory sprite.</value>
	public string InventorySprite
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the model that represents the item in the world.
	/// </summary>
	/// <value>The world model.</value>
    public string WorldModel
    {
    	get;
    	set;
    }
}
