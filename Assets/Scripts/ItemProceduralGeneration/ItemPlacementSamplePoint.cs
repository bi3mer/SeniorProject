using UnityEngine;
using System.Collections;

public class ItemPlacementSamplePoint
{
	/// <summary>
	/// Gets or sets the local position on the target surface.
	/// </summary>
	/// <value>The local target surface location.</value>
	public Vector2 LocalTargetSurfaceLocation 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the minimum distance away that another object needs to be.
	/// </summary>
	/// <value>The minimum distance.</value>
	public float MinDistance 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the grid point.
	/// </summary>
	/// <value>The grid point.</value>
	public Tuple<int, int> GridPoint
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the world space location.
	/// </summary>
	/// <value>The world space location.</value>
	public Vector3 WorldSpaceLocation
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the index of the item.
	/// </summary>
	/// <value>The index of the item.</value>
	public int ItemIndex
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the size.
	/// </summary>
	/// <value>The size.</value>
	public float Size
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the district.
	/// </summary>
	/// <value>The district.</value>
	public string District
	{
		get;
		set;
	}
}
