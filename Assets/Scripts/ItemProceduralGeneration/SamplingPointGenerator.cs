using UnityEngine;
using System.Collections;

public abstract class SamplingPointGenerator
{
	/// <summary>
	/// The size of the grid cells.
	/// </summary>
	public float CellSize
	{
		get;
		protected set;
	}

	/// <summary>
	/// The grid that contains information about what space is occupied in the city.
	/// </summary>
	public ItemPlacementSamplePoint[,] Grid
	{
		get;
		protected set;
	}

	/// <summary>
	/// The water tag.
	/// </summary>
	protected const string waterTag = "Water";

	/// <summary>
	/// Minimum distance objects must be from each other by default, not accounting for the doorway which will increase it
	/// </summary>
	protected const float defaultMinDistanceAway = 0.5f;

	/// <summary>
	/// The maximum angle a surface can be to still be considered a viable surface for object placement
	/// </summary>
	private const float maxAngle = 0.35f;

	/// <summary>
	/// Generates the random point around an existing targetPoint.
	/// </summary>
	/// <returns>Random point relative to the building.</returns>
	/// <param name="targetPoint">Point to generate around.</param>
	/// <param name="minDistance">Minimum distance from the current point that the new point must be.</param>
	protected Vector2 generateRandomPointAround(Vector2 targetPoint, float minDistance)
	{
		// random radius between mindist and 2 * mindist
		// The random number will generate from 0 to 1, so to make
		// it 1 to 2, 1 is added to it
		float radius = minDistance * (Random.Range(0f, 1f)  + 1);

		//random angle in radians
		float angle = 2 * Mathf.PI * Random.Range(0f, 1f); 

		//the new point is generated around the point (x, y)
		Vector2 point = new Vector2(targetPoint.x + radius * Mathf.Cos(angle), targetPoint.y + radius * Mathf.Sin(angle));
		return point;
	}

	/// <summary>
	/// Determines whether this point has neighbors that are too close.
	/// </summary>
	/// <returns><c>true</c> if this instance has overlapping neighbors the specified samplePoint; otherwise, <c>false</c>.</returns>
	/// <param name="samplePoint">Sample point.</param>
	protected bool HasOverlappingNeighbors(ItemPlacementSamplePoint samplePoint)
	{
		Vector2 point = samplePoint.LocalTargetSurfaceLocation;
		Tuple<int, int> gridPoint = samplePoint.GridPoint;
		
		float minDistance = samplePoint.MinDistance;

		// how many neighboring cells should be checked against in the grid
		int neighborCellCheck = Mathf.CeilToInt((minDistance + samplePoint.Size)/CellSize);

		// gets the start and  end grid location to be checked, and clamps them to be within the size of the grid
		int minX = (int) gridPoint.X - neighborCellCheck;
		int maxX = (int) gridPoint.X + neighborCellCheck;

		int minY = (int) gridPoint.Y - neighborCellCheck;
		int maxY = (int) gridPoint.Y + neighborCellCheck;

		if(minX < 0)
		{
			minX = 0;
		}

		if(maxX >= Grid.GetLength(0))
		{
			maxX = Grid.GetLength(0) - 1;
		}

		if(minY < 0)
		{
			minY = 0;
		}

		if(maxY >= Grid.GetLength(1))
		{
			maxY = Grid.GetLength(1) - 1;
		}

		// goes through grid points
		// if there is a point in the grid location specified, then check too see if the distance between point in the grid and the sample point 
		// is too small, which means that there is an overlap
		// if there is already a point in the grid in the sample point's grid location, then that point is automatically considered an overlap
		for (int i = minX; i <= maxX; ++i) 
		{
			for (int j = minY; j <= maxY; ++j) 
			{
				if (j != gridPoint.Y || i != gridPoint.X) 
				{
					ItemPlacementSamplePoint gridSample = Grid [i, j];

					if (gridSample != null && 
						Vector2.Distance(gridSample.LocalTargetSurfaceLocation, point) < (Mathf.Max(minDistance, gridSample.MinDistance) 
																					+ samplePoint.Size + gridSample.Size)) 
					{
						return true;
					}
				}
				else
				{
					ItemPlacementSamplePoint gridSample = Grid [i, j];
					if(gridSample != null)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Verifies that the raycastHit hits a point that is not water and not at an inclination greater than the maxAngle.
	/// </summary>
	/// <returns><c>true</c>, if raycast hits a point that meets specifications <c>false</c> otherwise.</returns>
	/// <param name="hit">Hit.</param>
	/// <param name="generateInWater">Whether or not it should be considered valid if generating in water.</param>
	protected bool verifyRaycastHit(RaycastHit hit, bool generateInWater)
	{
		if(hit.collider.CompareTag(waterTag) == generateInWater)
		{
			float dotProduct = Mathf.Clamp(Vector3.Dot(hit.normal, Vector3.up), -1f, 1f);

			if(Mathf.Abs (Mathf.Acos (dotProduct)) <= maxAngle)
			{
				return true;
			}
		}

		return false;
	}
}
