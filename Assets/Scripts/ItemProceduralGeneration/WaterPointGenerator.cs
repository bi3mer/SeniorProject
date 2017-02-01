using UnityEngine;
using System.Collections;

public class WaterPointGenerator : SamplingPointGenerator 
{
	public WaterPointGenerator(float cityWidth, float cityDepth)
	{
		grid = new ItemPlacementSamplePoint[Mathf.CeilToInt (cityWidth / cellSize), Mathf.CeilToInt (cityDepth / cellSize)];  
	}

	/// <summary>
	/// Adds the building bounds to the grid of occupied space.
	/// </summary>
	/// <param name="bound">Bound.</param>
	public void AddBuildingBounds(Bounds bound)
	{
		ItemPlacementSamplePoint buildingSamplingPoint;
		IntPair gridPoint;

		// mark the grid as occupied every 1 unit with an item of 1 size within the bounds of the building
		for(int i = (int) bound.min.x; i < (int) bound.max.x; ++i)
		{
			for(int j = (int) bound.min.z; j < (int)bound.max.y; ++j)
			{
				gridPoint = PointToGrid(new Vector2(i, j));

				if(grid[gridPoint.X, gridPoint.Y] == null)
				{
					buildingSamplingPoint = new ItemPlacementSamplePoint();
					buildingSamplingPoint.LocalTargetSurfaceLocation = new Vector2(i, j);
					buildingSamplingPoint.MinDistance = 1;
					buildingSamplingPoint.GridPoint = gridPoint;

					grid[gridPoint.X, gridPoint.Y] = buildingSamplingPoint;
				}
			}
		}
	}

	public void GeneratePoissonOnWater()
	{
		
	}
}
