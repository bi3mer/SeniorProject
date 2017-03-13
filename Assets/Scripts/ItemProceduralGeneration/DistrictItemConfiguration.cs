using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistrictItemConfiguration
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DistrictItemInfo"/> class.
	/// </summary>
	public DistrictItemConfiguration()
	{
		ItemNames = new List<string>();
		ItemExtents = new List<float>();
	}

	/// <summary>
	/// Gets or sets the item templates that will be duplicated to create objects.
	/// </summary>
	/// <value>The item templates.</value>
	public List<string> ItemNames
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the item extents.
	/// </summary>
	/// <value>The item extents.</value>
	public List<float> ItemExtents
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the Items that can be generated.
	/// </summary>
	/// <value>The item templates.</value>
	public List<GameObject> ItemTemplates
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the door extents.
	/// </summary>
	/// <value>The door extents.</value>
	public List<float> DoorExtents
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the door templates.
	/// </summary>
	/// <value>The door templates.</value>
	public List<GameObject> DoorTemplates
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the shelter extents.
	/// </summary>
	/// <value>The shelter extents.</value>
	public List<float> ShelterExtents
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the shelter templates.
	/// </summary>
	/// <value>The shelter templates.</value>
	public List<GameObject> ShelterTemplates
	{
		get;
		set;
	}
}