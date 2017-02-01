using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterPointGenerator : SamplingPointGenerator 
{
	public int MaxNumberOfItems;

	// Generally, the more initial points generated, the more evenly spread out the items will be
	public int NumberOfInitialPoints;

	// Generally, the more points per previous point and less number of initial points, the more clustered item generation will be
	public int NewPointsPerSamplingPoint;

	// The number of attempts to place a point. Generally, the higher the number, the closer to the maximum number of items will be generated
	public int MaxAttempts;

	// How many times it will generate an item for a point without checking for district before checking for district again
	public int DistrictCheck;

	public District[] districts;

	private float width;
	private float depth;

	public WaterPointGenerator(float cityWidth, float cityDepth, District[] cityDistricts)
	{
		grid = new ItemPlacementSamplePoint[Mathf.CeilToInt (cityWidth / cellSize), Mathf.CeilToInt (cityDepth / cellSize)];  
		width = cityWidth;
		depth = cityDepth;
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

	public List<ItemPlacementSamplePoint> GetPointsInWater(Dictionary<string, List<float>> generatableItemExtents)
	{
		List<ItemPlacementSamplePoint> samplingPoints = generatePoints(generatableItemExtents);

		// ray needs to start a little above the object, so this is the offset distance that will be added
		// to the height of the object to make the ray start above the top of the object
		float rayOffset = 1f;

		float initialRayStartHeight = Game.Instance.WaterLevelHeight + rayOffset;

		RaycastHit hit;
		List<ItemPlacementSamplePoint> validPoints = new List<ItemPlacementSamplePoint>();

		for (int i = 0; i < samplingPoints.Count; ++i) 
		{
			// the points recieved in the sampling point are all relative to the birds eye view surface area of the target building
			// where Vector2(0, 0) is the bottom left corner, and Vector2(width, depth) is the upper right corner
			// it is converted into a global position here
			// validPoints contains global positions

			if (Physics.Raycast (new Vector3(samplingPoints[i].LocalTargetSurfaceLocation.x, samplingPoints[i].LocalTargetSurfaceLocation.y), Vector3.down, out hit, initialRayStartHeight)) 
			{
				if (verifyRaycastHit(hit, false)) 
				{
					// the point in which the ray comes into contact with the surface of the building is where the object should be placed
					samplingPoints[i].WorldSpaceLocation = hit.point;
					validPoints.Add(samplingPoints[i]);
				}
			}
		}

		return validPoints;
	}

	private List<ItemPlacementSamplePoint> generatePoints(Dictionary<string, List<float>> generatableObjectExtents)
	{
		// RandomQueue works like a queue, except that it
		//pops a random element from the queue instead of
		//the element at the head of the queue
		List<ItemPlacementSamplePoint> processList = new List<ItemPlacementSamplePoint>();
		List<ItemPlacementSamplePoint> samplePoints = new List<ItemPlacementSamplePoint>();

		Vector3 cityCenter = Game.Instance.CityInstance.Center;
		WorldItemFactory itemFactory = Game.Instance.WorldItemFactoryInstance;

		int tries = 0;

		for(int i = 0; i < NumberOfInitialPoints; i++)
		{
			ItemPlacementSamplePoint initPoint = new ItemPlacementSamplePoint();

			// points should be generated around city center
			// so minimum is half the width/depth from the city center
			// and maximum is half the width/depth from the city center
			initPoint.LocalTargetSurfaceLocation = new Vector2(Random.Range(cityCenter.x - width/2f, cityCenter.x + width/2f), 
																Random.Range (cityCenter.z - depth/2f, cityCenter.z + depth/2f));

			initPoint.District = getDistrict(initPoint.LocalTargetSurfaceLocation);
			initPoint.ItemIndex = itemFactory.GetRandomItemIndex(initPoint.District);
			initPoint.MinDistance = defaultMinDistanceAway;
			initPoint.Size = generatableObjectExtents[initPoint.District][initPoint.ItemIndex];
			tries = 1;

			while(HasOverlappingNeighbors(initPoint) && tries < MaxAttempts)
			{
				initPoint.LocalTargetSurfaceLocation = new Vector2(Random.Range(cityCenter.x - width/2f, cityCenter.x + width/2f), 
																Random.Range (cityCenter.z - depth/2f, cityCenter.z + depth/2f));
				++tries;
			}

			if(tries < MaxAttempts)
			{
				initPoint.GridPoint = PointToGrid(initPoint.LocalTargetSurfaceLocation);
				grid[initPoint.GridPoint.X, initPoint.GridPoint.Y] = initPoint;
				samplePoints.Add(initPoint);
				processList.Add(initPoint);
			}
		}

		int pointsWithoutCheckingDistrict = 0;

		// generate other points from points in queue.
		while (processList.Count != 0)
		{
			int index = Random.Range (0, processList.Count);
			ItemPlacementSamplePoint point = processList [index]; 

			processList.RemoveAt (index);
			ItemPlacementSamplePoint newPoint;

			for (int i = 0; i < NewPointsPerSamplingPoint; ++i)
			{
				newPoint = new ItemPlacementSamplePoint ();

				newPoint.LocalTargetSurfaceLocation = generateRandomPointAround(point.LocalTargetSurfaceLocation, point.MinDistance + point.Size);

				if(pointsWithoutCheckingDistrict < DistrictCheck)
				{
					newPoint.District = point.District;
				}
				else
				{
					newPoint.District = getDistrict(newPoint.LocalTargetSurfaceLocation);
				}

				newPoint.ItemIndex = itemFactory.GetRandomItemIndex(newPoint.District);
				newPoint.MinDistance = defaultMinDistanceAway;
				newPoint.GridPoint = PointToGrid(newPoint.LocalTargetSurfaceLocation);
				newPoint.Size = generatableObjectExtents[newPoint.District][newPoint.ItemIndex];

				tries = 0;

				//check that the point is in the building's region and whether a neighboring point is too close
				while (tries < MaxAttempts && HasOverlappingNeighbors(newPoint))
				{
					newPoint.LocalTargetSurfaceLocation = generateRandomPointAround (point.LocalTargetSurfaceLocation, point.MinDistance + point.Size);
					newPoint.GridPoint = PointToGrid(newPoint.LocalTargetSurfaceLocation);
					++tries;
				}

				if(tries < MaxAttempts)
				{
					grid[newPoint.GridPoint.X, newPoint.GridPoint.Y] = newPoint;

					processList.Add(newPoint);
					samplePoints.Add(newPoint);

					// if number of points to generate objects on exceeds maximumItems specified, exit the process entirely
					if (samplePoints.Count >= MaxNumberOfItems) 
					{
						processList.Clear ();
						break;
					}
				}
			}
		}

		return samplePoints;
	}

	/// <summary>
	/// Gets the district.
	/// </summary>
	/// <returns>The district.</returns>
	/// <param name="point">Point.</param>
	private string getDistrict(Vector2 point)
	{
		for(int i = 0; i < districts.Length; ++i)
		{
			if(districts[i].ContainsPoint(point))
			{
				return districts[i].Name;
			}
		}

		return null;
	}
}
