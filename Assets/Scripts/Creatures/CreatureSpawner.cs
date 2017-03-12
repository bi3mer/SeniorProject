using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CreatureSpawner : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] creaturesPrefabs;

	[SerializeField]
	protected int minCreatureCount;

	protected int meanCreatureCount;

	[SerializeField]
	protected int maxCreatureCount;

	[SerializeField]
	private float maxSpawnRadius = 25f;

	[SerializeField]
	private float minSpawnRadius = 10f;

	[SerializeField]
	private float waterLevelOffset = -2f;

	protected CreatureManager creatureManager;

	/// <summary>
	/// The pool of creatures that have been killed and will need
	/// to be reinstantiated later on.
	/// </summary>
	private Stack<GameObject> creaturePool;

	/// <summary>
	/// Get the count of the number of creatures alive
	/// </summary>
	/// <value>The count.</value>
	public int Count
	{
		get
		{
			return this.creatureManager.CreatureCount;
		}
	}

	/// <summary>
	/// Gets the float inside of the spawn radius but outside
	/// of the minimum spawning radius
	/// </summary>
	/// <value>The float in radius.</value>
	private float floatInRadius
	{
		get
		{
			if(RandomUtility.RandomBool)
			{
				// return a random positive value
				return Random.Range(this.minSpawnRadius, this.maxSpawnRadius);
			}
			else
			{
				// return a random negative value
				return Random.Range(-this.maxSpawnRadius, -this.minSpawnRadius);
			}
		}
	}

	/// <summary>
	/// Find a valid spawn location
	/// </summary>
	/// <returns>The spawn location.</returns>
	protected virtual Vector3 findSpawnLocation()
	{
		return new Vector3(Game.Instance.PlayerInstance.WorldPosition.x - this.floatInRadius, 
		                   Game.Instance.WaterLevelHeight + this.waterLevelOffset,
			               Game.Instance.PlayerInstance.WorldPosition.z - this.floatInRadius);
	}

	/// <summary>
	/// Event where creature has been killed. Place creature into pool
	/// and spawn another.
	/// </summary>
	/// <param name="creature">Creature.</param>
	public void CreatureKilled(CreatureTracker creature)
	{
		// place into pool
		this.creatureManager.PutCreatureInPool(creature.Index);

		this.spawn();
	}

	/// <summary>
	/// Gets the random creature.
	/// </summary>
	/// <returns>The random creature.</returns>
	private GameObject getRandomCreature()
	{
		// get random index in creature array and return corresponding game object
		return this.creaturesPrefabs[Random.Range(0, this.creaturesPrefabs.Length)];
	}

	/// <summary>
	/// Initialize anything class specific
	/// </summary>
	protected virtual void Init()
	{
		// do nothing
	}

	/// <summary>
	/// Spawn a creature
	/// </summary>
	private void spawn()
	{
		this.creatureManager.Spawn(this.findSpawnLocation(), this.getRandomCreature());
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate()
	{
		if(this.creatureManager != null)
		{
			this.creatureManager.UpdateCreatureInfo(this.maxSpawnRadius, this.waterLevelOffset);
			this.creatureManager.UpdateCreatureCount(this.findSpawnLocation(), this.getRandomCreature());
		}
	}

	/// <summary>
	/// Spawn Creatures
	/// </summary>
	IEnumerator Start()
	{
		if(this.minSpawnRadius > this.maxSpawnRadius)
		{
			Debug.LogError("Min spawn radius must be smaller than the spawn radius");
		}

		// break out since nothing can be done with 0 prefabs
		// TODO: load from resources folder instead
		if(this.creaturesPrefabs.Length == 0)
		{
			Debug.LogError("Error, no prefabs in array to be spawned");
		}
		else
		{
			// wait till game class has instantiated
			yield return new WaitForEndOfFrame();

			// get mean of the min and max
			this.meanCreatureCount = (this.maxCreatureCount + this.minCreatureCount) / 2;

			// instantiate creature manager
			this.creatureManager = new CreatureManager(this.maxCreatureCount, this.meanCreatureCount);

			// instiate semi-random number of creatures
			for(int i = 0; i < Random.Range(this.minCreatureCount, this.maxCreatureCount); ++i)
			{
				this.spawn();
			}

			// run anything class specific
			this.Init();
		}
	}
}
