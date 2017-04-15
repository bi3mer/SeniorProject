using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterPointGenerator : SamplingPointGenerator 
{
	/// <summary>
	/// Gets or sets the max number of items.
	/// </summary>
	/// <value>The max number of items.</value>
	public int MaxNumberOfItems
	{
		get;
		set;
	}

	/// <summary>
	/// Generally, the more initial points generated, the more evenly spread out the items will be
	/// </summary>
	/// <value>The number of initial points.</value>
	public int NumberOfInitialPoints
	{
		get;
		set;
	}

	/// <summary>
	/// Generally, the more points per previous point and less number of initial points, the more clustered item generation will be
	/// </summary>
	/// <value>The new points per sampling point.</value>
	public int NewPointsPerSamplingPoint
	{
		get;
		set;
	}

	/// <summary>
	/// The number of attempts to place a point. Generally, the higher the number, the closer to the maximum number of items will be generated
	/// </summary>
	public int MaxAttempts
	{
		get;
		set;
	}

	/// <summary>
	/// How many times it will generate an item for a point without checking for district before checking for district again
	/// </summary>
	public int DistrictCheck
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the districts.
	/// </summary>
	/// <value>The districts.</value>
	public District[] Districts
	{
		get;
		set;
	}

	/// <summary>
	/// The width of the city.
	/// </summary>
	private float cityWidth;

	/// <summary>
	/// The city depth.
	/// </summary>
	private float cityDepth;

	/// <summary>
	/// The city center.
	/// </summary>
	private Vector3 cityCenter;

	// ray needs to start a little above the object, so this is the offset distance that will be added
	// to the height of the object to make the ray start above the top of the object
	private const float rayOffset = 1f;

	/// <summary>
	/// Initializes a new instance of the <see cref="WaterPointGenerator"/> class.
	/// </summary>
	/// <param name="width">City width.</param>
	/// <param name="depth">City depth.</param>
	/// <param name="center">City center.</param>
	public WaterPointGenerator(float width, float depth, Vector3 center)
	{
		// the diagonal of the cell is the defaultMinDistanceAway
		// so the dimensions of the cellSize square should be divided by Mathf.Sqrt(2)
		// Since the diagonal of a square is Mathf.Sqrt(2) * outsideDimension
		// ex: If the dimensions of a square is 3, then the diagonal is Mathf.Sqrt(9 + 9) = 3 * Mathf.Sqrt(2)
		cellSize = defaultMinDistanceAway/Mathf.Sqrt(2);
		grid = new ItemPlacementSamplePoint[Mathf.CeilToInt (width / cellSize), Mathf.CeilToInt (depth / cellSize)]; 
		cityWidth = width;
		cityDepth = depth;
		cityCenter = center;
	}

	/// <summary>
	/// Adds the building bounds to the grid of occupied space.
	/// </summary>
	/// <param name="bound">Bound.</param>
	public void AddBuildingBounds(Bounds bound)
	{
		ItemPlacementSamplePoint buildingSamplingPoint;
		Tuple<int, int> gridPoint;

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

	/// <summary>
	/// Gets the sampling points in water.
	/// </summary>
	/// <returns>The points in water.</returns>
	/// <param name="generatableItemExtents">Generatable item extents.</param>
	public List<ItemPlacementSamplePoint> GetPointsInWater(Dictionary<string, List<float>> generatableItemExtents)
	{
		List<ItemPlacementSamplePoint> samplingPoints = generatePoints(generatableItemExtents);

		float initialRayStartHeight = Game.Instance.WaterLevelHeight + rayOffset;

		RaycastHit hit;
		List<ItemPlacementSamplePoint> validPoints = new List<ItemPlacementSamplePoint>();

		for (int i = 0; i < samplingPoints.Count; ++i) 
		{
			// the points recieved in the sampling point are all relative to the birds eye view surface area of the target building
			// where Vector2(0, 0) is the bottom left corner, and Vector2(width, depth) is the upper right corner
			// it is converted into a global position here
			// validPoints contains global positions
			if (Physics.Raycast (new Vector3(samplingPoints[i].LocalTargetSurfaceLocation.x, initialRayStartHeight, samplingPoints[i].LocalTargetSurfaceLocation.y), Vector3.down, out hit, initialRayStartHeight)) 
			{
				if (verifyRaycastHit(hit, true)) 
				{
					// the point in which the ray comes into contact with the surface of the building is where the object should be placed
					samplingPoints[i].WorldSpaceLocation = hit.point;
					validPoints.Add(samplingPoints[i]);
				}
			}
		}

		return validPoints;
	}

	/// <summary>
	/// Generates the points.
	/// </summary>
	/// <returns>The points.</returns>
	/// <param name="generatableObjectExtents">Generatable object extents.</param>
	private List<ItemPlacementSamplePoint> generatePoints(Dictionary<string, List<float>> generatableObjectExtents)
	{
		// RandomQueue works like a queue, except that it
		//pops a random element from the queue instead of
		//the element at the head of the queue
		List<ItemPlacementSamplePoint> processList = generateInitialPoints(generatableObjectExtents);
		List<ItemPlacementSamplePoint> samplePoints = new List<ItemPlacementSamplePoint>();

		for(int i = 0; i < processList.Count; ++i)
		{
			samplePoints.Add(processList[i]);
		}

		WorldItemFactory itemFactory = Game.Instance.WorldItemFactoryInstance;

		int tries = 0;
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

				if(newPoint.District != null)
				{
					newPoint.ItemIndex = itemFactory.GetRandomItemIndex(newPoint.District, true);
					newPoint.MinDistance = defaultMinDistanceAway;
					newPoint.GridPoint = PointToGrid(newPoint.LocalTargetSurfaceLocation);
					newPoint.Size = generatableObjectExtents[newPoint.District][newPoint.ItemIndex];

					tries = 0;

					//check that the point is in the building's region and whether a neighboring point is too close
					while (tries < MaxAttempts && (HasOverlappingNeighbors(newPoint) || !inCityBounds(newPoint.LocalTargetSurfaceLocation)))
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
		}

		return samplePoints;
	}

	/// <summary>
	/// Generates the initial sampling points.
	/// </summary>
	/// <returns>The initial points.</returns>
	/// <param name="generatableObjectExtents">Generatable object extents.</param>
	public List<ItemPlacementSamplePoint> generateInitialPoints(Dictionary<string, List<float>>  generatableObjectExtents)
	{
		List<ItemPlacementSamplePoint> initialPoints = new List<ItemPlacementSamplePoint>();
		WorldItemFactory itemFactory = Game.Instance.WorldItemFactoryInstance;

		int tries = 0;

		for(int i = 0; i < NumberOfInitialPoints; ++i)
		{
			ItemPlacementSamplePoint initPoint = new ItemPlacementSamplePoint();

			// points should be generated around city center
			// so minimum is half the width/depth from the city center
			// and maximum is half the width/depth from the city center
			initPoint.LocalTargetSurfaceLocation = new Vector2(Random.Range(cityCenter.x - cityWidth/2f, cityCenter.x + cityWidth/2f), 
																Random.Range (cityCenter.z - cityDepth/2f, cityCenter.z + cityDepth/2f));
			tries = 1;
			initPoint.District = getDistrict(initPoint.LocalTargetSurfaceLocation);

			while(tries < MaxAttempts && (initPoint.District == null || !inCityBounds(initPoint.LocalTargetSurfaceLocation)))
			{
				initPoint.LocalTargetSurfaceLocation = new Vector2(Random.Range(cityCenter.x - cityWidth/2f, cityCenter.x + cityWidth/2f), 
																Random.Range (cityCenter.z - cityDepth/2f, cityCenter.z + cityDepth/2f));
				initPoint.District = getDistrict(initPoint.LocalTargetSurfaceLocation);

				++tries;
			}

			if(initPoint.District != null)
			{
				initPoint.ItemIndex = itemFactory.GetRandomItemIndex(initPoint.District, true);
				initPoint.MinDistance = defaultMinDistanceAway;
				initPoint.Size = generatableObjectExtents[initPoint.District][initPoint.ItemIndex];
				initPoint.GridPoint = PointToGrid(initPoint.LocalTargetSurfaceLocation);

				while( tries < MaxAttempts && (HasOverlappingNeighbors(initPoint) || !inCityBounds(initPoint.LocalTargetSurfaceLocation)))
				{
					initPoint.LocalTargetSurfaceLocation = new Vector2(Random.Range(cityCenter.x - cityWidth/2f, cityCenter.x + cityWidth/2f), 
																	Random.Range (cityCenter.z - cityDepth/2f, cityCenter.z + cityDepth/2f));
					initPoint.GridPoint = PointToGrid(initPoint.LocalTargetSurfaceLocation);
					++tries;
				}

				if(tries < MaxAttempts)
				{	
					grid[initPoint.GridPoint.X, initPoint.GridPoint.Y] = initPoint;
					initialPoints.Add(initPoint);
				}
			}
		}

		return initialPoints;
	}

	/// <summary>
	/// Gets the district.
	/// </summary>
	/// <returns>The district.</returns>
	/// <param name="point">Point.</param>
	private string getDistrict(Vector2 point)
	{
		for(int i = 0; i < Districts.Length; ++i)
		{
			if(Districts[i].ContainsPoint(point))
			{
				return Districts[i].Name;
			}
		}

		return null;
	}

	/// <summary>
	/// Checks whether or not a point is within the city bounds.
	/// </summary>
	/// <returns><c>true</c>, if point is in city bounds, <c>false</c> otherwise.</returns>
	/// <param name="location">Location.</param>
	private bool inCityBounds(Vector2 location)
	{
		if(location.x > (cityCenter.x - cityWidth/2f) && location.x < (cityCenter.x + cityWidth/2f))
		{
			if(location.y > (cityCenter.y - cityDepth/2f) && location.y < (cityCenter.y + cityDepth/2f))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Gets the grid coordinates for a sampling point
	/// Since the point locations are not created with the minimum bounds being (0, 0) in mind
	/// it needs to be offset such that the minimum bound is at (0, 0)
	/// Which can be done by adding half the dimension of the city and the center of the city
	/// </summary>
	/// <returns>The to grid.</returns>
	/// <param name="samplingPoint">Sampling point.</param>
	protected Tuple<int, int> PointToGrid(Vector2 samplingPoint)
	{
		return new Tuple<int, int> ((int)((samplingPoint.x - cityCenter.x + cityWidth/2f) / cellSize), 
									(int)((samplingPoint.y - cityCenter.y + cityDepth/2f) / cellSize));
	}
}
