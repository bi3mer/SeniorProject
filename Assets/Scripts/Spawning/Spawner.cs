using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Spawner : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] creatures;

	[SerializeField]
	private int minCreatureCount;

	[SerializeField]
	private int maxCreatureCount;

	/// <summary>
	/// The pool of creatures that have been killed and will need
	/// to be reinstantiated later on.
	/// </summary>
	private Stack<GameObject> creaturePool;

	/// <summary>
	/// Count of how many creatures are in the scene
	/// </summary>
	/// <value>The creature count.</value>
	public int CreatureCount
	{
		get;
		private set;
	}

	/// <summary>
	/// Find a valid spawn location
	/// </summary>
	/// <returns>The spawn location.</returns>
	protected virtual Vector3 findSpawnLocation()
	{
		// TODO: This will be turned into real code without magic numbers in the future
		//       but right now basic functionality is for the best.
		return new Vector3(Game.Instance.PlayerInstance.WorldTransform.position.x - Random.Range(-25f, 25f), 
		                   Game.Instance.WaterLevelHeight - 2f,
			               Game.Instance.PlayerInstance.WorldTransform.position.z - Random.Range(-25f, 25f));
	}

	/// <summary>
	/// Puts creature into the pool. 
	/// </summary>
	/// <param name="creature">Creature.</param>
	private void putCreatureInPool(GameObject creature)
	{
		// turn game object off
		creature.SetActive(false);

		// place into pool
		this.creaturePool.Push(creature);
	}

	/// <summary>
	/// Event where creature has been killed. Place creature into pool
	/// and spawn another.
	/// </summary>
	/// <param name="creature">Creature.</param>
	public void CreatureKilled(GameObject creature)
	{
		// place into pool
		this.putCreatureInPool(creature);

		// reduce current count
		--this.CreatureCount;

		this.spawn();
	}

	/// <summary>
	/// Spawn a creature
	/// </summary>
	private void spawn()
	{
		GameObject newCreature;

		// check if stack is empty
		if(this.creaturePool.Count == 0)
		{
			// get random index in creature array
			int randomIndex = Random.Range(0, this.creatures.Length);
			Debug.Log(randomIndex);

			// spawn creature
			newCreature = GameObject.Instantiate(this.creatures[randomIndex]);
		}
		else
		{
			// stack is not empty so we can get the most recent game object
			// and put it back in the scene
			newCreature = this.creaturePool.Pop();
			newCreature.SetActive(true);
		}

		// set position of new creature
		newCreature.transform.position = this.findSpawnLocation();

		// increase count of creatures
		++this.CreatureCount;
	}

	/// <summary>
	/// Spawn Creatures
	/// </summary>
	IEnumerator Start()
	{
		// wait till game class has instantiated
		yield return new WaitForEndOfFrame();

		this.creaturePool = new Stack<GameObject>();

		// instiate semi-random number of creatures
		this.CreatureCount = 0;
		for(int i = 0; i < Random.Range(this.minCreatureCount, this.maxCreatureCount); ++i)
		{
			this.spawn();
		}
	}
}
