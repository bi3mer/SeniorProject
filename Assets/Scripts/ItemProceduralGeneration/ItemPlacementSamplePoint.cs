using UnityEngine;
using System.Collections;

public class ItemPlacementSamplePoint
{
	public Vector2 PointOnTargetSurface 
	{
		get;
		set;
	}

	// minDistance away from this point that an object needs to be
	public float MinDistance 
	{
		get;
		set;
	}

	public Vector2 GridPoint
	{
		get;
		set;
	}

	public Vector3 WorldSpaceLocation
	{
		get;
		set;
	}

	public int ItemIndex
	{
		get;
		set;
	}

	public float Size
	{
		get;
		set;
	}
}
