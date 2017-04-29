using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterItemGeneration : ItemGenerator 
{
	[SerializeField]
    [Tooltip("Max number of items to be generated in the water")]
	private int maxNumberOfItems;

	[SerializeField]
	[Tooltip("Generally, the more initial points generated, the more evenly spread out the items will be")]
	private int numberOfInitialPoints;

	[SerializeField]
	[Tooltip("Number of initial points when generating within a single cell only.")]
	private int numberOfInitialPointsInSingleCell;

	[SerializeField]
	[Tooltip("Generally, the more points per previous point and less number of initial points, the more clustered item generation will be")]
	private int newPointsPerSamplingPoint;

	[SerializeField]
	[Tooltip("The number of attempts to place a point. Generally, the higher the number, the closer to the maximum number of items will be generated")]
	private int maxAttempts;

	[SerializeField]
	[Tooltip("How many times it will generate an item for a point without checking for district before checking for district again")]
	private int districtCheck;

	[SerializeField]
	[Tooltip("How many grid chunks away the storm event will generate more items for the player.")]
	private int itemIncreaseChunks;

	private Dictionary<string, List<float>> generatableItemExtents;

	public delegate void StepCallback (List<ItemPlacementSamplePoint> message);

	/// <summary>
	/// The point generator.
	/// </summary>
	private WaterPointGenerator pointGenerator;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () 
	{
		Dictionary<string, List<GameObject>> itemTemplates = Game.Instance.WorldItemFactoryInstance.GetAllInteractableItemsByDistrict(false, true);
		districtItemInfo = new Dictionary<string, DistrictItemConfiguration>();
		generatableItemExtents = new Dictionary<string, List<float>>();

		// get district name here
		foreach(string key in itemTemplates.Keys)
		{
			districtItemInfo.Add(key, new DistrictItemConfiguration());
			districtItemInfo[key].ItemTemplates = itemTemplates[key];
			districtItemInfo[key].ItemNames = Game.Instance.ItemFactoryInstance.WaterItemsByDistrict[key];
			districtItemInfo[key].ItemExtents = GetItemExtents(itemTemplates[key]);
			generatableItemExtents.Add(key, districtItemInfo[key].ItemExtents);
		}

		Game.Instance.EventManager.StormStoppedSubscription += this.ItemIncreaseEvent;
	}

	/// <summary>
	/// Sets the city information. Must be called before points are generated.
	/// </summary>
	/// <param name="cityWidth">City width.</param>
	/// <param name="cityDepth">City depth.</param>
	/// <param name="districts">Districts.</param>
	public void SetCityInformation(float cityWidth, float cityDepth, Vector3 cityCenter, District[] districts)
	{
		pointGenerator = new WaterPointGenerator(cityWidth, cityDepth, cityCenter);
		pointGenerator.MaxNumberOfItems = maxNumberOfItems;
		pointGenerator.NumberOfInitialPoints = numberOfInitialPoints;
		pointGenerator.NewPointsPerSamplingPoint = newPointsPerSamplingPoint;
		pointGenerator.MaxAttempts = maxAttempts;
		pointGenerator.DistrictCheck = districtCheck;
		pointGenerator.Districts = districts;
	}

	/// <summary>
	/// Generates the items in water.
	/// </summary>
	public void GenerateInWater()
	{
		float minX = pointGenerator.CityCenter.x - pointGenerator.CityWidth/2f;
		float maxX = pointGenerator.CityCenter.x + pointGenerator.CityWidth/2f;
		float minY = pointGenerator.CityCenter.z - pointGenerator.CityDepth/2f;
		float maxY = pointGenerator.CityCenter.z + pointGenerator.CityDepth/2f;

		StepCallback finishingAction = SetInitialGenerationPoints;
		StartCoroutine(pointGenerator.GenerateRandomPointsInBounds(minX, maxX, minY, maxY, pointGenerator.NumberOfInitialPoints, generatableItemExtents, finishingAction));
	}

	/// <summary>
	/// Generates the in water around player.
	/// </summary>
	public void GenerateInWaterAroundPlayer()
	{
		Tuple<int, int> playerLocation = Game.Instance.ItemPoolInstance.PointToGrid(new Vector2(Game.Instance.PlayerInstance.WorldTransform.position.x, 
																 Game.Instance.PlayerInstance.WorldTransform.position.z));

		int maxX = Mathf.Clamp(playerLocation.X + itemIncreaseChunks, 0, (int)Game.Instance.CityBounds.CityBounds.max.x);
		int minX = Mathf.Clamp(playerLocation.X - itemIncreaseChunks, 0, (int)Game.Instance.CityBounds.CityBounds.max.x);
		int maxY = Mathf.Clamp(playerLocation.Y + itemIncreaseChunks, 0, (int)Game.Instance.CityBounds.CityBounds.max.y);
		int minY = Mathf.Clamp(playerLocation.Y - itemIncreaseChunks, 0, (int)Game.Instance.CityBounds.CityBounds.max.y);

		Bounds totalBounds = new Bounds();
		Bounds cellBounds;

		for(int i = minX; i < maxX; ++i)
		{
			for(int j = minY; j < maxY; ++j)
			{
				cellBounds = pointGenerator.GetGridCellBounds(i, j);
				if(cellBounds.size != null && cellBounds.size.x > 0f)
				{
					totalBounds.Encapsulate(cellBounds);
				}
			}
		}

		StepCallback finishingAction = SetInitialGenerationPoints;
		StartCoroutine(pointGenerator.GenerateRandomPointsInBounds(totalBounds.min.x, totalBounds.max.x, totalBounds.min.y, totalBounds.max.y, 
																   pointGenerator.NumberOfInitialPoints, generatableItemExtents, finishingAction));
	}

	/// <summary>
	/// Adds the building to water point generation's map of blocked off space.
	/// </summary>
	/// <param name="buildingBound">Building bound.</param>
	public void AddBuildingToWaterGenerationMap(Bounds buildingBound)
	{
		pointGenerator.AddBuildingBounds(buildingBound);
	}

	/// <summary>
	/// Generates the objects that needs to be placed on the building's surface.
	/// </summary>
	/// <param name="points">Points.</param>
	private void addObjectsToPoolManager(List<ItemPlacementSamplePoint> points)
	{
		for (int i = 0; i < points.Count; ++i) 
		{
			if(points[i].ItemIndex < districtItemInfo[points[i].District].ItemNames.Count)
			{
				poolManager.AddToGrid(points[i].WorldSpaceLocation, districtItemInfo[points[i].District].ItemNames[points[i].ItemIndex], false);

				GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = points[i].WorldSpaceLocation;
			}
		}
	}

	/// <summary>
	/// Adds the objects to world. Adds to pool manager.
	/// </summary>
	/// <param name="points">Points that need to be added.</param>
	private void addObjectsToWorld(List<ItemPlacementSamplePoint> points)
	{
		for (int i = 0; i < points.Count; ++i) 
		{
			if(points[i].ItemIndex < districtItemInfo[points[i].District].ItemNames.Count)
			{
				poolManager.AddItemImmediate(points[i].WorldSpaceLocation, districtItemInfo[points[i].District].ItemNames[points[i].ItemIndex]);
			}
		}
	}

	/// <summary>
	/// Items the increase event.
	/// </summary>
	private void ItemIncreaseEvent()
	{
		Game.Instance.EventManager.StormStoppedSubscription += GenerateInWaterAroundPlayer;
	}

	public void VerifySamplingPoints(List<ItemPlacementSamplePoint> possiblePoints)
	{
		StepCallback nextAction = SetGenerationPoints;
		StartCoroutine(pointGenerator.SetPointsInWater(possiblePoints, nextAction));
	}

	public void SetInitialGenerationPoints(List<ItemPlacementSamplePoint> initialPoints)
	{
		StepCallback nextAction = VerifySamplingPoints;
		StartCoroutine(pointGenerator.GeneratePoints(generatableItemExtents, initialPoints, nextAction));
	}

	public void SetGenerationPoints(List<ItemPlacementSamplePoint> samplingPoints)
	{
		if (samplingPoints.Count > 0) 
		{
			addObjectsToPoolManager(samplingPoints);
		}
	}
}
