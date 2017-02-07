using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RooftopPointGenerator: SamplingPointGenerator
{
	// Maximum attempts to place a point
	private const int maximumAttempts = 10;

	// Maxmimum items to be placed
	private const int maximumItems = 5;

	// The maximum distance away from the doorway in which the door being there will affect whether or not objects may be placed there
	// This will be multiplied against the short side of the surface
	private const float maxDoorwayEffectPercent = 0.5f;

	// the number of new points that will be created around an existing sampling point
	private const int newPointsPerSamplingPoint = 3;

	// the amount of area considered valid on the surface of a building
	// from 0 to 1, where 0 means none of the surface is valid, and 1 means that there is no area of the building that is valid
	// if 0.1, there is a border that is 10% the width and 10% the depth run around the edge of the surface
	// no sampling points may be placed within that border
	private const float borderPercent = 0.1f;

	private List<float> generatableObjectExtents;

	private List<float> generatableDoorExtents;

	/// <summary>
	/// Takes the sampling points and checks to see if they are on locations that are not at too steep an incline on the building's surface.
	/// Then converts the sampling points to their world coordinates.
	/// </summary>
	/// <returns>The valid points.</returns>
	/// <param name="targetBound">Target bounds.</param>
	/// <param name="targetCenter">Target center.</param>
	/// <param name="itemInfo">Item info based on district.</param>
	/// <param name="district">Distric this belongs tot.</param>
	/// <param name="doorExtents">Door extents.</param>
	/// <param name="doorTemplates">Door templates.</param>
	/// <param name="hasDoor">If set to <c>true</c> has door.</param>
	public List<ItemPlacementSamplePoint> GetValidPoints(Bounds targetBound, Vector3 targetCenter, DistrictItemConfiguration itemInfo, 
														 string district, List<float> doorExtents, List<GameObject> doorTemplates, bool hasDoor = false)
	{
		generatableDoorExtents = doorExtents;
		generatableObjectExtents = itemInfo.ItemExtents;

		Vector3 max = targetBound.max;
		Vector3 min = targetBound.min;

		float width = max.x - min.x;
		float depth = max.z - min.z;
		float height = max.y - min.y;

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
		List<ItemPlacementSamplePoint> samplingPoints;

		if(hasDoor)
		{
			samplingPoints = generatePoints (targetCenter, generatableDoorExtents, itemInfo, district, hasDoor, width, depth, height);
		}
		else
		{
			samplingPoints = generatePoints (targetCenter, generatableObjectExtents,itemInfo, district, hasDoor, width, depth, height);
		}

		float initialRayStartHeight = height + rayOffset;

		RaycastHit hit;
		List<ItemPlacementSamplePoint> validPoints = new List<ItemPlacementSamplePoint> ();

		for (int i = 0; i < samplingPoints.Count; ++i) 
		{
			// the points recieved in the sampling point are all relative to the birds eye view surface area of the target building
			// where Vector2(0, 0) is the bottom left corner, and Vector2(width, depth) is the upper right corner
			// it is converted into a global position here
			// validPoints contains global positions
			Vector2 globalPosition = convertToWorldSpace (samplingPoints[i].LocalTargetSurfaceLocation, targetCenter, width, depth);

			if (Physics.Raycast (new Vector3(globalPosition.x, initialRayStartHeight, globalPosition.y), Vector3.down, out hit, initialRayStartHeight)) 
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

	/// <summary>
	/// Generates sampling points for object while taking into account a door object.
	/// Items should avoid being placed near the door.
	/// </summary>
	/// <returns>The points.</returns>
	/// <param name="center">Center.</param>
	/// <param name="firstPointExtents">First point extents.</param>
	/// <param name="itemInfo">The information about the district the rooftop belongs to.</param>
	/// <param name="district">The district this generating for.</param></param>
	/// <param name="hasDoor">If set to <c>true</c> has door.</param>
	/// <param name="width">Width.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="height">Height.</param>
	private List<ItemPlacementSamplePoint> generatePoints(Vector3 center, List<float> firstPointExtents, DistrictItemConfiguration itemInfo, string district, bool hasDoor, float width, float depth, float height)
	{
		float shorterLength = width;
		float maxDoorwayEffectArea = maxDoorwayEffectPercent * shorterLength;

		if (width > depth) 
		{
			shorterLength = depth;
		}

		// generate the first point randomly away from the doorway
		ItemPlacementSamplePoint firstPoint = new ItemPlacementSamplePoint();

		if(hasDoor)
		{
			firstPoint.ItemIndex = Random.Range(0, firstPointExtents.Count);
		}
		else
		{
			firstPoint.ItemIndex = Game.Instance.WorldItemFactoryInstance.GetRandomItemIndex(district);
		}

		firstPoint.LocalTargetSurfaceLocation = createFirstPoint (center, width, depth, height, firstPoint.ItemIndex, firstPointExtents);
		firstPoint.GridPoint = PointToGrid(firstPoint.LocalTargetSurfaceLocation);
		firstPoint.MinDistance = getMinDistance(firstPoint.LocalTargetSurfaceLocation, firstPoint.LocalTargetSurfaceLocation, shorterLength/2f, maxDoorwayEffectArea, hasDoor);
		firstPoint.Size = firstPointExtents[firstPoint.ItemIndex];

		// Building failed to have any sample points after attempting the maximum attempts
		// not a good building, skip
		if (firstPoint.LocalTargetSurfaceLocation.Equals (Vector2.zero)) 
		{
			return new List<ItemPlacementSamplePoint>();
		}

		return generateSubsequentPoints(width, depth, height, firstPoint, itemInfo, district, hasDoor); 
	}

	/// <summary>
	/// Generates sampling points for object while taking into account a door object.
	/// Items should avoid being placed near the door.
	/// </summary>
	/// <returns>The subsequent points.</returns>
	/// <param name="width">Width.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="height">Height.</param>
	/// <param name="firstPoint">First point.</param>
	/// <param name="itemInfo"> District item info. </param> 
	/// <param name="district">District this belongs to.</param>
	/// <param name="useGaussianMinDistance">If set to <c>true</c> use gaussian minimum distance.</param>
	private List<ItemPlacementSamplePoint> generateSubsequentPoints(float width, float depth, float height, 
														  ItemPlacementSamplePoint firstPoint, DistrictItemConfiguration itemInfo, string district, bool useGaussianMinDistance)
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

		grid[firstPoint.GridPoint.X, firstPoint.GridPoint.Y] = firstPoint;

		// the door acts as an origin point, however no other sampling points should be placed around it
		// thus the door's point itself should not be returned as part of the sampling points
		processList.Add(firstPoint);
		samplePoints.Add(firstPoint);

		WorldItemFactory itemFactory = Game.Instance.WorldItemFactoryInstance;

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
				newPoint.ItemIndex = itemFactory.GetRandomItemIndex(district);

				newPoint.LocalTargetSurfaceLocation = generateRandomPointAround(point.LocalTargetSurfaceLocation, point.MinDistance + point.Size);
				newPoint.MinDistance = getMinDistance(firstPoint.LocalTargetSurfaceLocation, newPoint.LocalTargetSurfaceLocation, shorterLength/2f, maxDoorwayEffectArea, useGaussianMinDistance);
				newPoint.GridPoint = PointToGrid(newPoint.LocalTargetSurfaceLocation);
				newPoint.Size = generatableObjectExtents[newPoint.ItemIndex];

				tries = 0;

				//check that the point is in the building's region and whether a neighboring point is too close
				while (tries < maximumAttempts && (!inRangeOfBuilding (newPoint.LocalTargetSurfaceLocation, width, depth, newPoint.Size) || HasOverlappingNeighbors(newPoint)))
				{
					newPoint.LocalTargetSurfaceLocation = generateRandomPointAround (point.LocalTargetSurfaceLocation, point.MinDistance + point.Size);
					newPoint.MinDistance = getMinDistance(firstPoint.LocalTargetSurfaceLocation, newPoint.LocalTargetSurfaceLocation, shorterLength/2f, maxDoorwayEffectArea, useGaussianMinDistance);
					newPoint.GridPoint = PointToGrid(newPoint.LocalTargetSurfaceLocation);
					++tries;
				}

				if(tries < maximumAttempts)
				{
					//update processList and samplePoints with new point
					newPoint.GridPoint = PointToGrid(newPoint.LocalTargetSurfaceLocation);
					grid[newPoint.GridPoint.X, newPoint.GridPoint.Y] = newPoint;

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
	/// Checks if the sampling point is within the area of the surface of the building.
	/// Range is checked by checking against width and depth of the building, meaning that the samplingPoint passed
	/// in is not in the world position yet.
	/// </summary>
	/// <returns><c>true</c>, if sampling point is in range, <c>false</c> otherwise.</returns>
	/// <param name="samplingPoint">Sampling point that contains a location relative to the building to be checked against.</param>
	/// <param name="width">Width of building.</param>
	/// <param name="depth">Depth of building.</param>
	/// <param name="size">Size of object in sampling point.</param> 
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
	/// <param name="maxDistanceAway">Max distance away.</param>
	/// <param name="maxDoorwayEffectArea">Maximum distance at which door's presence will effect the density of points</param>
	/// <param name="useGaussian">If set to <c>true</c> uses gaussian distribution.</param>
	private float getMinDistance(Vector2 door, Vector2 currentPosition, float maxDistanceAway, float maxDoorwayEffectArea, bool useGaussian)
	{
		if(useGaussian)
		{
			return Mathf.Clamp (defaultMinDistanceAway + maxDistanceAway * (1 - Vector2.Distance (door, currentPosition) / maxDoorwayEffectArea), 
								defaultMinDistanceAway, maxDistanceAway);
		}

		return defaultMinDistanceAway;
	}

	/// <summary>
	/// Creates a sample point for the door. The door must be placed properly, so this does a slope check.
	/// If a door cannot be created, then the building will not have any items placed on it, as that
	/// means that the item has too little valid areas to place items.
	/// </summary>
	/// <returns>The first point.</returns>
	/// <param name="center">Center.</param>
	/// <param name="width">Width.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="height">Height.</param>
	/// <param name="itemIndex">Item index.</param>
	/// <param name="generatableExtents">Generatable extents.</param>
	private Vector2 createFirstPoint(Vector3 center, float width, float depth, float height, int itemIndex, List<float> generatableExtents)
	{
		float size = generatableExtents[itemIndex];

		// Generates a point within the bounds and converts it to a global location
		Vector2 objectLocation = new Vector2(Random.Range(width * borderPercent + size, width - (width * borderPercent + size)), Random.Range (depth * borderPercent + size, depth - (depth * borderPercent + size)));
		Vector2 objectGlobalLocation = convertToWorldSpace(objectLocation, center, width, depth);
		float rayOffset = 1f;
		float initialRayStartHeight = height + rayOffset;

		RaycastHit hit;
		Physics.Raycast (new Vector3 (objectGlobalLocation.x, initialRayStartHeight, objectGlobalLocation.y), Vector3.down, out hit, initialRayStartHeight);
		int tries = 0;

		// It will continue trying to place a point for the door so long as
		// 1) the number of maximum attempts has not been reached
		// 2) the raycast is not hitting anything
		// 3) the raycast is not hitting the target gamebobject
		// 4) the angle of the surface that the raycast hits exceeds the maxAngle
		while (tries < maximumAttempts && (hit.transform == null || !verifyRaycastHit(hit, false)))
		{
			objectLocation = new Vector2(Random.Range(width * borderPercent + size, 1 - (width * borderPercent + size)), Random.Range (depth * borderPercent + size, 1 - (depth * borderPercent + size)));

			objectGlobalLocation = convertToWorldSpace(objectLocation, center, width, depth);
			Physics.Raycast (new Vector3 (objectGlobalLocation.x, initialRayStartHeight, objectGlobalLocation.y), Vector3.down, out hit, height);
		
			++tries;
		}

		if (tries == maximumAttempts)
		{
			objectLocation = Vector3.zero;
		}

		return objectLocation;
	}

	/// <summary>
	/// Converts the relative sample point location to world space. The relative point passed in will be between Vector2(0, 0) and Vector2(width, depth) of 
	/// of the building, where (0, 0) is the bottom left corner of the building, and (width, depth) is the upper right corner of the building.
	/// </summary>
	/// <returns>The to world space.</returns>
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
	/// Gets the grid coordinates for a sampling point
	/// </summary>
	/// <returns>The to grid.</returns>
	/// <param name="samplingPoint">Sampling point.</param>
	protected Tuple<int, int> PointToGrid(Vector2 samplingPoint)
	{
		return new Tuple<int, int> ((int)(samplingPoint.x / cellSize), (int)(samplingPoint.y / cellSize));
	}
}
