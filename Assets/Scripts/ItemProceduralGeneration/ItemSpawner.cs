using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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

    [Tooltip("The roof's door, if left null, the door won't open.")]
    [SerializeField]
    private GameObject door;

    [Tooltip("How far the door should rotate to open.")]
    [SerializeField]
    private float doorRotationAngles;

    [Tooltip("Time in seconds for the door to open.")]
    [SerializeField]
    private float doorOpenTime;

    private bool doorIsOpen = false;

    [Tooltip("Custom Set up for prefabs set in by hand.")]
    [SerializeField]
    private bool CustomSetUp;
    [SerializeField]
    private string customDistrictName;
    [SerializeField]
    private float customSize;

    [SerializeField]
    private Transform[] itemLocations;

    [Tooltip("Time in seconds for items to move to position.")]
    [SerializeField]
    private float itemMoveTime;

    [Tooltip("possible positions for posters to appear on the door")]
    [SerializeField]
    private Transform[] posterPositions;
    public Transform[] PosterPositions
    {
        get
        {
            return posterPositions;
        }
    }
    
    [SerializeField]
    private float posterRotationModMax;
    public float PosterRotationModMax
    {
        get
        {
            return posterRotationModMax;
        }
    }


    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
	{
		SetUp();

        if(CustomSetUp)
        {
            SetUpSpawner(customSize, customDistrictName);
        }
	}

	/// <summary>
	/// Sets up spawner. Sets the action as the interactable object action, and gets the size of the item around which items should be spawned. 
	/// </summary>
	/// <param name="centralItem">The bounds of the gameobject this script is attached to..</param>
	public void SetUpSpawner(float size, string districtName)
	{
		// the radius of the item is half the size
		centralItemRadius = size;
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

            if(door != null && !doorIsOpen)
            {
                door.transform.DOLocalRotate(new Vector3(0f, doorRotationAngles, 0f), doorOpenTime);
                doorIsOpen = true;
            }
		}
	}

	/// <summary>
	/// Spawns the items around the central object
	/// </summary>
	private void spawnItems()
	{
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		int numberToSpawn = (int) Random.Range(MinSpawnNumber, MaxSpawnNumber);

		for (int i = 0; i < numberToSpawn  ; ++i)
		{
			GameObject item = factory.CreateRandomPickupInteractableItem(district);           

            Vector3 spawnposition;
            if(door == null)
            {
                spawnposition = transform.position;
            }
            else
            {
                spawnposition = door.transform.position;
            }
            item.transform.position = spawnposition;
            //Code to move the item to its proper location. Currently has a bug associated with moving items once they are spawned.
            //TO BE UNCOMMENTED AFTER FIXED
            //item.transform.DOMove(itemLocations[Random.Range(0, itemLocations.Length)].position, itemMoveTime);
			item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);

            item.transform.position = itemLocations[Random.Range(0, itemLocations.Length)].position;

            Game.Instance.ItemPoolInstance.AddItemFromWorld(item);
        }
	}
}

