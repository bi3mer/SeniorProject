/// <summary>
/// Base class for all notes.
/// </summary>
public class NoteData
{
	/// <summary>
	/// Gets or sets the interactable Note object.
	/// </summary>
	/// <value>The interactable.</value>
	public Note interactable 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the title of the note
	/// </summary>
	/// <value>The title of the note</value>
	public string Title 
	{
		get; 
		set;
	}

	/// <summary>
	/// Gets or sets the type of the note
	/// </summary>
	/// <value>The type of the note</value>
	public string Type 
	{
		get; 
		set;
	}

	/// <summary>
	/// Gets or sets the body text of the note
	/// </summary>
	/// <value>The body text of the note</value>
	public string Text 
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

	/// <summary>
	/// Gets or sets the model that represents the item in the inventory.
	/// </summary>
	/// <value>The inventory sprite model.</value>
	public string InventorySprite
	{
		get;
		set;
	}
}