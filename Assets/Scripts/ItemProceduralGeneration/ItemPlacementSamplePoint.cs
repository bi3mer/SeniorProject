using UnityEngine;
using System.Collections;

public struct ItemPlacementSamplePoint
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
}
