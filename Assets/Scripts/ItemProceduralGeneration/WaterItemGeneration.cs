﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterItemGeneration : ItemGenerator 
{
	[SerializeField]
    [Tooltip("Max number of items to be generated in the water")]
	private int maxNumberOfItems;

	[SerializeField]
	[Tooltip("Max number of items  generated by a single additional item increase event")]
	private int maxNewNumberOfItems;

	[SerializeField]
	[Tooltip("Generally, the more initial points generated, the more evenly spread out the items will be")]
	private int numberOfInitialPoints;

	[SerializeField]
	[Tooltip("Number of initial points when generating during a item increase event.")]
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

	public delegate void StepCallback (List<ItemPlacementSamplePoint> message);

	/// <summary>
	/// The point generator.
	/// </summary>
	private WaterPointGenerator pointGenerator;

	private bool addImmediateToWorld;

	private GameLoaderTask loadingTask;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () 
	{
		Dictionary<string, List<GameObject>> itemTemplates = Game.Instance.WorldItemFactoryInstance.GetAllInteractableItemsByDistrict(false, true);
		districtItemInfo = new Dictionary<string, DistrictItemConfiguration>();

		// get district name here
		foreach(string key in itemTemplates.Keys)
		{
			districtItemInfo.Add(key, new DistrictItemConfiguration());
			districtItemInfo[key].ItemTemplates = itemTemplates[key];
			districtItemInfo[key].ItemNames = Game.Instance.ItemFactoryInstance.WaterItemsByDistrict[key];
			districtItemInfo[key].ItemExtents = GetItemExtents(itemTemplates[key]);
		}

		Game.Instance.EventManager.StormStartedSubscription += this.GenerateInWaterAroundPlayer;
	}

	/// <summary>
	/// Unsubscribes on destroy
	/// </summary>
	void OnDestroy()
	{
		Game.Instance.EventManager.StormStartedSubscription -= this.GenerateInWaterAroundPlayer;
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
		pointGenerator.NumberOfInitialPoints = numberOfInitialPoints;
		pointGenerator.NewPointsPerSamplingPoint = newPointsPerSamplingPoint;
		pointGenerator.MaxAttempts = maxAttempts;
		pointGenerator.DistrictCheck = districtCheck;
		pointGenerator.Districts = districts;
	}

	/// <summary>
	/// Generates the items in water.
	/// </summary>
	public void GenerateInWater(ref GameLoaderTask task)
	{
		float minX = pointGenerator.CityCenter.x - pointGenerator.CityWidth/2f;
		float maxX = pointGenerator.CityCenter.x + pointGenerator.CityWidth/2f;
		float minY = pointGenerator.CityCenter.z - pointGenerator.CityDepth/2f;
		float maxY = pointGenerator.CityCenter.z + pointGenerator.CityDepth/2f;
		pointGenerator.MaxNumberOfItems = maxNumberOfItems;

		loadingTask = task;
		addImmediateToWorld = false;
		StepCallback finishingAction = SetInitialGenerationPoints;
		StartCoroutine(pointGenerator.GenerateRandomPointsInBounds(minX, maxX, minY, maxY, pointGenerator.NumberOfInitialPoints, districtItemInfo, finishingAction));
	}

	/// <summary>
	/// Generates the in water around player.
	/// </summary>
	public void GenerateInWaterAroundPlayer()
	{
		Vector2 playerPos = new Vector2(Game.Player.WorldPosition.x, Game.Player.WorldPosition.z);

		Tuple<int, int> playerLocation = Game.Instance.ItemPoolInstance.PointToGrid(playerPos);

		if(playerLocation != null)
		{
			pointGenerator.MaxNumberOfItems = maxNewNumberOfItems;
			int maxX = Mathf.Clamp(playerLocation.X + itemIncreaseChunks, 0, (int)pointGenerator.Grid.GetLength(0));
			int minX = Mathf.Clamp(playerLocation.X - itemIncreaseChunks, 0, (int)pointGenerator.Grid.GetLength(0));
			int maxY = Mathf.Clamp(playerLocation.Y + itemIncreaseChunks, 0, (int)pointGenerator.Grid.GetLength(1));
			int minY = Mathf.Clamp(playerLocation.Y - itemIncreaseChunks, 0, (int)pointGenerator.Grid.GetLength(1));

			float cityMinX = pointGenerator.CityCenter.x - pointGenerator.CityWidth/2f;
			float cityMinY = pointGenerator.CityCenter.z - pointGenerator.CityDepth/2f;

			Vector3 minBounds = new Vector3(cityMinX + pointGenerator.CellSize * minX, 0, cityMinY + pointGenerator.CellSize * minY);
			Vector3 maxBounds = new Vector3(cityMinX + pointGenerator.CellSize * (maxX + 1), 0, cityMinY + pointGenerator.CellSize * (maxY + 1));

			addImmediateToWorld = true;
			StepCallback finishingAction = SetInitialGenerationPoints;
			StartCoroutine(pointGenerator.GenerateRandomPointsInBounds(minBounds.x, maxBounds.x, minBounds.z, maxBounds.z,
																	   pointGenerator.NumberOfInitialPoints, districtItemInfo, finishingAction));
		}
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
			}
		}

		if(loadingTask != null)
		{
			loadingTask.PercentageComplete = 1f;
			loadingTask = null;
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
	/// Starts Coroutine that verifies the sampling points.
	/// </summary>
	/// <param name="possiblePoints">Possible points.</param>
	public void VerifySamplingPoints(List<ItemPlacementSamplePoint> possiblePoints)
	{
		if(loadingTask != null)
		{
			loadingTask.PercentageComplete = 0.6f;
		}

		StepCallback nextAction = SetGenerationPoints;
		StartCoroutine(pointGenerator.SetPointsInWater(possiblePoints, nextAction));
	}

	/// <summary>
	/// Sets the initial generation points.
	/// </summary>
	/// <param name="initialPoints">Initial points.</param>
	public void SetInitialGenerationPoints(List<ItemPlacementSamplePoint> initialPoints)
	{
		// TODO: this is temporary and will be changed to not use magic numbers in the pull request to do the loading screen
		if(loadingTask != null)
		{
			loadingTask.PercentageComplete = 0.3f;
		}

		StepCallback nextAction = VerifySamplingPoints;
		StartCoroutine(pointGenerator.GeneratePoints(initialPoints, districtItemInfo, nextAction));
	}

	/// <summary>
	/// Sets the generation points and adds the items accompanying them to the world.
	/// </summary>
	/// <param name="samplingPoints">Sampling points.</param>
	public void SetGenerationPoints(List<ItemPlacementSamplePoint> samplingPoints)
	{
		// TODO: this is temporary and will be changed to not use magic numbers in the pull request to do the loading screen
		if(loadingTask != null)
		{
			loadingTask.PercentageComplete = 0.9f;
		}

		if (samplingPoints.Count > 0) 
		{
			if(addImmediateToWorld)
			{
				addObjectsToWorld(samplingPoints);
			}
			else
			{
				addObjectsToPoolManager(samplingPoints);
			}
		}
	}
}
