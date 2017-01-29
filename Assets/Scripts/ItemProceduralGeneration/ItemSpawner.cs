using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : InteractableObject 
{
	[Tooltip("Animations or other actions that should fired off prior to items spawning")]
	[SerializeField]
	private UnityEvent prespawnActions;

	[Tooltip("Can spawn again if interacted with again")]
	[SerializeField]
	private bool spawnsMultipleTimes;

	[Tooltip("Distance away from the outer edge of the central object that items will be generated.")]
	public float SpawnRadius;

	[Tooltip("If true, spawns items after actions are executed. If false, waits for other code to calls the spawn action.")]
	public bool SpawnAfterActions;

	[Tooltip("Items should appear around the object without need for interaction.")]
	public bool SpawnWithoutInteraction;

	[Tooltip("Max number of items to spawn")]
	public int MaxSpawnNumber;

	[Tooltip("Min number of items to spawn")]
	public int MinSpawnNumber;

	private float centralItemRadius;

	private string district;

	private bool spawned;

	private const float angleIncrementations = 40f;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		SetUp();
	}

	/// <summary>
	/// Sets up spawner. Sets the action as the interactable object action, and gets the size of the item around which items should be spawned. 
	/// </summary>
	/// <param name="centralItem">The bounds of the gameobject this script is attached to..</param>
	public void SetUpSpawner(Bounds centralItem, string districtName)
	{
		// the radius of the item is half the size
		centralItemRadius = Mathf.Max(centralItem.extents.x, centralItem.extents.z);
		district = districtName;

		// if items should be spawned around central item without need for interaction, spawn here
		// otherwise set the action upon interaction be spawning
		if(SpawnWithoutInteraction)
		{
			spawnItems();
		}
		else
		{
			SetAction
			(
				delegate 
				{ 
					startSpawn(); 
				}
			);
		}
	}

	/// <summary>
	/// Starts the spawning. First runs through the prespawnActions. If the call to spawn items will not come from the prespawn actions, then
	/// spawning occurs immediately after the prespawn actions are invoked.
	/// </summary>
	private void startSpawn()
	{
		if(!spawned || spawnsMultipleTimes)
		{
			if(prespawnActions != null)
	    	{
				prespawnActions.Invoke();
			}

			if(SpawnAfterActions)
			{
				spawnItems();
			}

			spawned = true;
		}
	}

	/// <summary>
	/// Spawns the items around the central object
	/// </summary>
	private void spawnItems()
	{
		float spawnLocationRadius = centralItemRadius + SpawnRadius;

		float currentSpawnSlot = 0;

		// maxDiscardSlots is how points on the circle there are given the amount in which the angle increments
		// a circle has 360 degrees, so the number of points is equal to 360 divided by the amount in which the angle increments
		// however, at 360, the player has looped back to 0, so subtract the last slot, which will be considered to be 360, which equals 0
		int maxSpawnSlots = Mathf.FloorToInt(360 / angleIncrementations) - 1;
		Vector3 centerPos = transform.position;
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		int numberToSpawn = (int) Random.Range(MinSpawnNumber, MaxSpawnNumber);

		for (int i = 0; i < numberToSpawn  ; ++i)
		{
			GameObject item = factory.CreateRandomPickupInteractableItem(district);
			item.transform.position = new Vector3(centerPos.x + spawnLocationRadius * Mathf.Cos(Mathf.Deg2Rad *(angleIncrementations * currentSpawnSlot)),
												  centerPos.y,
												  centerPos.z + spawnLocationRadius * Mathf.Sin(Mathf.Deg2Rad * (angleIncrementations * currentSpawnSlot)));
			item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);

			++ currentSpawnSlot;

			if(currentSpawnSlot > maxSpawnSlots)
			{
				currentSpawnSlot = 0;
			}
		}
	}
}
