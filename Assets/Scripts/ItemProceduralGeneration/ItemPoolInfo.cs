using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPoolInfo
{
	/// <summary>
	/// Gets or sets the locations of items in the cell.
	/// </summary>
	/// <value>The locations.</value>
	public List<Vector3> Locations
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="ItemPoolInfo"/> is activated.
	/// </summary>
	/// <value><c>true</c> if activated; otherwise, <c>false</c>.</value>
	public bool Activated
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the item names in this cell.
	/// </summary>
	/// <value>The item names.</value>
	public List<string> ItemNames
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the gameobjects used to represent items in this cell.
	/// </summary>
	/// <value>The items.</value>
	public List<GameObject> Items
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the grid location.
	/// </summary>
	/// <value>The grid location.</value>
	public Tuple<int, int> GridLocation
	{
		get;
		set;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ItemPoolInfo"/> class.
	/// </summary>
	/// <param name="location">Location.</param>
	/// <param name="itemName">Item name.</param>
	/// <param name="coord">Grid coordinate.</param>
	/// <param name="activated">If set to <c>true</c> activated.</param>
	public ItemPoolInfo(Vector3 location, string itemName, Tuple<int, int> coord, bool activated)
	{
		Locations = new List<Vector3>();
		ItemNames = new List<string>();
		Items = new List<GameObject>();
		Activated = activated;
		GridLocation = coord;

		Locations.Add(location);
		ItemNames.Add(itemName);
	}

	/// <summary>
	/// Removes the item info from the pool info.
	/// </summary>
	/// <param name="index">Index.</param>
	public void RemoveItemInfo(int index)
	{
		ItemNames.RemoveAt(index);
		Items.RemoveAt(index);
		Locations.RemoveAt(index);
	}

	/// <summary>
	/// Adds the item info from the pool information.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="name">Name of item.</param>
	public void AddItemInfo(GameObject item, string name)
	{
		ItemNames.Add(name);
		Items.Add(item);
		Locations.Add(item.transform.position);
	}
}
