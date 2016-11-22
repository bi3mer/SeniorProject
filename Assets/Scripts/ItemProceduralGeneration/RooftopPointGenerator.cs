using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RooftopPointGenerator 
{
	// The maximum angle a surface can be to still be considered a viable surface for object placement
	private const float maxAngle = 0.35f;

	// Maximum attempts to place a point
	private const int maximumAttempts = 10;

	// Maxmimum items to be placed
	private const int maximumItems = 8;

	// The maximum distance away from the doorway in which the door being there will affect whether or not objects may be placed there
	// This will be multiplied against the short side of the surface
	private const float maxDoorwayEffectPercent = 0.5f;

	// Minimum distance objects must be from each other by default, not accounting for the doorway which will increase it
	private const float defaultMinDistanceAway = 1f;

	// the number of new points that will be created around an existing sampling point
	private const int newPointsPerSamplingPoint = 3;

	// the amount of area considered valid on the surface of a building
	// from 0 to 1, where 0 means none of the surface is valid, and 1 means that there is no area of the building that is valid
	// if 0.1, there is a border that is 10% the width and 10% the depth run around the edge of the surface
	// no sampling points may be placed within that border
	private const float borderPercent = 0.1f;

	private float cellSize;

	private ItemPlacementSamplePoint[,] grid;

	private List<float> generatableObjectExtents;

	private List<float> generatableDoorExtents;

	/// <summary>
	/// Takes the sampling points and checks to see if they are on locations that are not at too steep an incline on the building's surface.
	/// Then converts the sampling points to their world coordinates.
	/// </summary>
	/// <returns>The valid points.</returns>
	/// <param name="target">Target.</param>
	public List<ItemPlacementSamplePoint> GetValidPoints(GameObject target, List<float> doorExtents, List<float> itemExtents)
	{
		generatableDoorExtents = doorExtents;
		generatableObjectExtents = itemExtents;

		Mesh targetMesh = target.GetComponent<MeshFilter> ().mesh;
		Vector3 max = targetMesh.bounds.max;
		Vector3 min = targetMesh.bounds.min;

		float width = (target.transform.localScale.x * max.x) - (target.transform.localScale.x * min.x);
		float depth = (target.transform.localScale.z * max.z) - (target.transform.localScale.z * min.z);
		float height = (target.transform.localScale.y * max.y) - (target.transform.localScale.y * min.y);

		// the defaultMinDistanceAway is used as the diagonal of the square cell
		// thus, the size of the cell--that is its sides
		// is defined by the diagonal/sqrt(2)
		cellSize = defaultMinDistanceAway/Mathf.Sqrt(2);

		//Create the grid
		grid = new ItemPlacementSamplePoint[Mathf.CeilToInt (width / cellSize), Mathf.CeilToInt (depth / cellSize)];   

		// ray needs to start a little above the object, so this is the offset distance that will be added
		// to the height of the object to make the ray start above the top of the object
		float rayOffset = 1f;

		// Generates points to place objects, as well a door object.
		// TODO: Function that generates points without taking a door into consideration
		List<ItemPlacementSamplePoint> samplingPoints = generatePointsWithDoor (target, targetMesh, width, depth, height);
		float initialRayStartHeight = height + rayOffset;

		RaycastHit hit;
		List<ItemPlacementSamplePoint> validPoints = new List<ItemPlacementSamplePoint> ();

		for (int i = 0; i < samplingPoints.Count; ++i) 
		{
			// the points recieved in the sampling point are all relative to the birds eye view surface area of the target building
			// where Vector2(0, 0) is the bottom left corner, and Vector2(width, depth) is the upper right corner
			// it is converted into a global position here
			// validPoints contains global positions
			Vector2 globalPosition = convertToWorldSpace (samplingPoints[i].PointOnTargetSurface, target.transform.position, width, depth);

			if (Physics.Raycast (new Vector3(globalPosition.x, initialRayStartHeight, globalPosition.y), Vector3.down, out hit, initialRayStartHeight)) 
			{
				if (hit.transform.gameObject.Equals (target)) 
				{
					if (Mathf.Abs (Mathf.Acos (Vector3.Dot (hit.normal, Vector3.up))) < maxAngle) 
					{
						// the point in which the ray comes into contact with the surface of the building is where the object should be placed
						samplingPoints[i].WorldSpaceLocation = hit.point;
						validPoints.Add(samplingPoints[i]);
					}
				}
			}
		}

		return validPoints;
	}

	/// <summary>
	/// Generates sampling points for object while taking into account a door object.
	/// Items should avoid being placed near the door.
	/// </summary>
	/// <returns>The points with door.</returns>
	/// <param name="target">Target.</param>
	/// <param name="targetMesh">Target mesh.</param>
	/// <param name="width">Width.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="height">Height.</param>
	private List<ItemPlacementSamplePoint> generatePointsWithDoor(GameObject target, Mesh targetMesh, float width, float depth, float height)
	{
		float shorterLength = width;
		float maxDoorwayEffectArea = maxDoorwayEffectPercent * shorterLength;

		if (width > depth) 
		{
			shorterLength = depth;
		}

		// RandomQueue works like a queue, except that it
		//pops a random element from the queue instead of
		//the element at the head of the queue
		List<ItemPlacementSamplePoint> processList = new List<ItemPlacementSamplePoint>();

		List<ItemPlacementSamplePoint> samplePoints = new List<ItemPlacementSamplePoint>();

		// generate the first point randomly away from the doorway
		ItemPlacementSamplePoint firstPoint = new ItemPlacementSamplePoint();
		firstPoint.ItemIndex = Random.Range(0, generatableDoorExtents.Count);
		firstPoint.PointOnTargetSurface = createDoor (width, depth, height, target, firstPoint.ItemIndex);
		firstPoint.GridPoint = PointToGrid(firstPoint.PointOnTargetSurface);
		firstPoint.MinDistance = getMinDistance(firstPoint.PointOnTargetSurface, firstPoint.PointOnTargetSurface, shorterLength/2f, maxDoorwayEffectArea);
		firstPoint.Size = generatableDoorExtents[firstPoint.ItemIndex];

		// Building failed to have any sample points after attempting the maximum attempts
		// not a good building, skip
		if (firstPoint.PointOnTargetSurface.Equals (Vector2.zero)) 
		{
			return samplePoints;
		}

		grid[(int) firstPoint.GridPoint.x, (int) firstPoint.GridPoint.y] = firstPoint;

		// the door acts as an origin point, however no other sampling points should be placed around it
		// thus the door's point itself should not be returned as part of the sampling points
		processList.Add(firstPoint);
		samplePoints.Add(firstPoint);

		// generate other points from points in queue.
		while (processList.Count != 0)
		{
			int index = Random.Range (0, processList.Count);
			ItemPlacementSamplePoint point = processList [index]; 

			processList.RemoveAt (index);
			ItemPlacementSamplePoint newPoint;

			int tries;
			for (int i = 0; i < newPointsPerSamplingPoint; ++i)
			{
				newPoint = new ItemPlacementSamplePoint ();
				newPoint.ItemIndex = Random.Range(0, generatableObjectExtents.Count);

				newPoint.PointOnTargetSurface = generateRandomPointAround(point.PointOnTargetSurface, point.MinDistance + point.Size);
				newPoint.MinDistance = getMinDistance(firstPoint.PointOnTargetSurface, newPoint.PointOnTargetSurface, shorterLength/2f, maxDoorwayEffectArea);
				newPoint.GridPoint = PointToGrid(newPoint.PointOnTargetSurface);
				newPoint.Size = generatableObjectExtents[newPoint.ItemIndex];

				tries = 0;

				//check that the point is in the building's region and whether a neighboring point is too close
				while (tries < maximumAttempts && (!inRangeOfBuilding (newPoint.PointOnTargetSurface, width, depth, newPoint.Size) || HasOverlappingNeighbors(newPoint)))
				{
					newPoint.PointOnTargetSurface = generateRandomPointAround (point.PointOnTargetSurface, point.MinDistance + point.Size);
					newPoint.MinDistance = getMinDistance(firstPoint.PointOnTargetSurface, newPoint.PointOnTargetSurface, shorterLength/2f, maxDoorwayEffectArea);
					newPoint.GridPoint = PointToGrid(newPoint.PointOnTargetSurface);
					++tries;
				}

				if(tries < maximumAttempts)
				{
					//update processList and samplePoints with new point
					newPoint.GridPoint = PointToGrid(newPoint.PointOnTargetSurface);
					grid[(int) newPoint.GridPoint.x, (int) newPoint.GridPoint.y] = newPoint;

					processList.Add(newPoint);
					samplePoints.Add(newPoint);
					// if number of points to generate objects on exceeds maximumItems specified, exit the process entirely
					if (samplePoints.Count > maximumItems) 
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
	/// Generates the random point around an existing targetPoint.
	/// </summary>
	/// <returns>Random point relative to the building.</returns>
	/// <param name="targetPoint">Point to generate around.</param>
	/// <param name="minDistance">Minimum distance from the current point that the new point must be.</param>
	private Vector2 generateRandomPointAround(Vector2 targetPoint, float minDistance)
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
	/// Checks if the sampling point is within the area of the surface of the building.
	/// Range is checked by checking against width and depth of the building, meaning that the samplingPoint passed
	/// in is not in the world position yet.
	/// </summary>
	/// <returns><c>true</c>, if sampling point is in range, <c>false</c> otherwise.</returns>
	/// <param name="samplingPoint">Sampling point that contains a location relative to the building to be checked against.</param>
	/// <param name="width">Width of building.</param>
	/// <param name="depth">Depth of building.</param>
	private bool inRangeOfBuilding(Vector2 samplingPoint, float width, float depth, float size)
	{
		float widthBorder = width * borderPercent;
		float depthBorder = depth * borderPercent;

		bool result = ((samplingPoint.x + widthBorder + size) < width && (samplingPoint.x - size) > widthBorder && (samplingPoint.y + depthBorder + size) < depth && (samplingPoint.y - size) > depthBorder);
		return result;
	}
		
	/// <summary>
	/// Gets the minimum distance away that the next sample point should be. The further from the door, the smaller
	/// the minimum distance needs to be. This will help the objects trend away from the door.
	/// </summary>
	/// <returns>The minimum distance.</returns>
	/// <param name="door">Door.</param>
	/// <param name="currentPosition">Current position.</param>
	private float getMinDistance(Vector2 door, Vector2 currentPosition, float maxDistanceAway, float maxDoorwayEffectArea)
	{
		return Mathf.Clamp (defaultMinDistanceAway + maxDistanceAway * (1 - Vector2.Distance (door, currentPosition) / maxDoorwayEffectArea), defaultMinDistanceAway, maxDistanceAway);
	}

	/// <summary>
	/// Creates a sample point for the door. The door must be placed properly, so this does a slope check.
	/// If a door cannot be created, then the building will not have any items placed on it, as that
	/// means that the item has too little valid areas to place items.
	/// </summary>
	/// <returns>The relative location of the door.</returns>
	/// <param name="width">Width of building.</param>
	/// <param name="depth">Depth of building.</param>
	/// <param name="height">Height of building.</param>
	/// <param name="target">Target building.</param>
	private Vector2 createDoor(float width, float depth, float height, GameObject target, int doorIndex)
	{
		float size = generatableDoorExtents[doorIndex];

		// Generates a point within the bounds and converts it to a global location
		Vector2 doorLocation = new Vector2(Random.Range(width * borderPercent + size, width - (width * borderPercent + size)), Random.Range (depth * borderPercent + size, depth - (depth * borderPercent + size)));
		Vector2 doorGlobalLocation = convertToWorldSpace(doorLocation, target.transform.position, width, depth);
		float rayOffset = 1f;
		float initialRayStartHeight = height + rayOffset;

		RaycastHit hit;
		Physics.Raycast (new Vector3 (doorGlobalLocation.x, initialRayStartHeight, doorGlobalLocation.y), Vector3.down, out hit, initialRayStartHeight);
		int tries = 0;

		// It will continue trying to place a point for the door so long as
		// 1) the number of maximum attempts has not been reached
		// 2) the raycast is not hitting anything
		// 3) the raycast is not hitting the target gamebobject
		// 4) the angle of the surface that the raycast hits exceeds the maxAngle
		while (tries < maximumAttempts && (hit.transform == null || !hit.transform.gameObject.Equals (target) || (Mathf.Abs (Mathf.Acos (Vector3.Dot (hit.normal, Vector3.up))) > maxAngle))) 
		{
			doorLocation = new Vector2(Random.Range(width * borderPercent + size, 1 - (width * borderPercent + size)), Random.Range (depth * borderPercent + size, 1 - (depth * borderPercent + size)));

			doorGlobalLocation = convertToWorldSpace(doorLocation, target.transform.position, width, depth);

			Physics.Raycast (new Vector3 (doorGlobalLocation.x, initialRayStartHeight, doorGlobalLocation.y), Vector3.down, out hit, height);
		
			++tries;
		}

		if (tries == maximumAttempts)
		{
			doorLocation = Vector3.zero;
		}

		return doorLocation;
	}

	/// <summary>
	/// Converts the relative sample point location to world space. The relative point passed in will be between Vector2(0, 0) and Vector2(width, depth) of 
	/// of the building, where (0, 0) is the bottom left corner of the building, and (width, depth) is the upper right corner of the building.
	/// </summary>
	/// <returns>The to global.</returns>
	/// <param name="surfacePosition">Surface position.</param>
	/// <param name="targetPosition">Target position.</param>
	/// <param name="targetWidth">Target width.</param>
	/// <param name="targetDepth">Target depth.</param>
	private Vector2 convertToWorldSpace(Vector2 surfacePosition, Vector3 targetPosition, float targetWidth, float targetDepth)
	{
		// to convert to worldSpace, add the target object's position, then subtract half the width so that "0" is the left bound rather than the center
		// and subtract half of the depth so that "0" is the bottom bound rather than the center
		return new Vector2(surfacePosition.x + targetPosition.x - targetWidth/2f, surfacePosition.y + targetPosition.z - targetDepth/2f);
	}

	/// <summary>
	/// Determines whether this point has neighbors that are too close.
	/// </summary>
	/// <returns><c>true</c> if this point has neighbors that are too close; otherwise, <c>false</c>.</returns>
	/// <param name="point">Point.</param>
	///  <param name="cellSize">Size of cells in gird.</param>
	private bool HasOverlappingNeighbors(ItemPlacementSamplePoint samplePoint)
	{
		Vector2 point = samplePoint.PointOnTargetSurface;
		Vector2 gridPoint = samplePoint.GridPoint;
		
		float minDistance = samplePoint.MinDistance;

		// how many neighboring cells should be checked against in the grid
		int neighborCellCheck = Mathf.CeilToInt((minDistance + samplePoint.Size)/cellSize);

		// gets the start and  end grid location to be checked, and clamps them to be within the size of the grid
		int minX = (int) gridPoint.x - neighborCellCheck;
		int maxX = (int) gridPoint.x + neighborCellCheck;

		int minY = (int) gridPoint.y - neighborCellCheck;
		int maxY = (int) gridPoint.y + neighborCellCheck;

		if(minX < 0)
		{
			minX = 0;
		}

		if(maxX >= grid.GetLength(0))
		{
			maxX = grid.GetLength(0) - 1;
		}

		if(minY < 0)
		{
			minY = 0;
		}

		if(maxY >= grid.GetLength(1))
		{
			maxY = grid.GetLength(1) - 1;
		}

		// goes through grid points
		// if there is a point in the grid location specified, then check too see if the distance between point in the grid and the sample point 
		// is too small, which means that there is an overlap
		// if there is already a point in the grid in the sample point's grid location, then that point is automatically considered an overlap
		for (int i = minX; i <= maxX; ++i) 
		{
			for (int j = minY; j <= maxY; ++j) 
			{
				if (j != gridPoint.y || i != gridPoint.x) 
				{
					ItemPlacementSamplePoint gridSample = grid [i, j];

					if (gridSample != null && 
						Vector2.Distance(gridSample.PointOnTargetSurface, point) < (Mathf.Max(minDistance, gridSample.MinDistance) 
																					+ samplePoint.Size + gridSample.Size)) 
					{
						return true;
					}
				}
				else
				{
					ItemPlacementSamplePoint gridSample = grid [i, j];
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
	/// Gets the grid coordinates for a sampling point
	/// </summary>
	/// <returns>The to grid.</returns>
	/// <param name="samplingPoint">Sampling point.</param>
	/// <param name="cellSize">Cell size.</param>
	private Vector2 PointToGrid(Vector2 samplingPoint)
	{
		return new Vector2 ((int)(samplingPoint.x / cellSize), (int)(samplingPoint.y / cellSize));
	}
}
