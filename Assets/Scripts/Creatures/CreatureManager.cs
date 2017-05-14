using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class CreatureInfo
{
	public CreatureTracker creatureTracker;
	public Rigidbody rb;
}

public class CreatureManager 
{
    /// <summary>
    /// This is the parent for where all the creatures will be placed.
    /// </summary>
    private Transform parent;

	/// <summary>
	/// The pool of creatures that have been killed and will need
	/// to be reinstantiated later on.
	/// </summary>
	private Stack<int> creaturePool;

	/// <summary>
	/// List of active creatures in the pool
	/// </summary>
	private CreatureInfo[] creatures;

	/// <summary>
	/// The ideal amount of creatures in the scene
	/// </summary>
	public int IdealCreatureCount
	{
		get;
		set;
	}

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
	/// Spawn the specified position and creature.
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="creature">Creature.</param>
	public void Spawn(Vector3 position, GameObject creature)
	{
		// prevent over spawning of creatures
		if(this.CreatureCount == this.creatures.Length)
		{
			return;
		}

		CreatureTracker newCreature;

		// check if stack is empty
		if(this.creaturePool.Count == 0)
		{
			// spawn creature
			GameObject spawnedCreature = GameObject.Instantiate(creature);

            // set parent
            spawnedCreature.transform.SetParent(this.parent.transform);

            // add empty class
            this.creatures[this.CreatureCount] = new CreatureInfo();

            // set rigid body
            this.creatures[this.CreatureCount].rb = spawnedCreature.GetComponent<Rigidbody>();

            // add creature tracker and cache it
			newCreature                                        = spawnedCreature.AddComponent<CreatureTracker>();
			newCreature.Index                                  = this.CreatureCount;
			this.creatures[this.CreatureCount].creatureTracker = newCreature;
		}
		else
		{
			// stack is not empty so we can get the most recent game object
			// and put it back in the scene
			newCreature = this.creatures[this.creaturePool.Pop()].creatureTracker;
			newCreature.IsDead = false;
			newCreature.gameObject.SetActive(true);
		}

		// reset velocity
		this.creatures[this.CreatureCount].rb.velocity = Vector3.zero;
		this.creatures[this.CreatureCount].rb.rotation = Quaternion.identity;

		// set position of new creature
		newCreature.transform.position = position;

		// increase count of creatures
		++this.CreatureCount;
	}

	/// <summary>
	/// Puts creature into the pool. 
	/// </summary>
	/// <param name="creature">Creature.</param>
	public void PutCreatureInPool(int creatureIndex)
	{
		// turn game object off
		this.creatures[creatureIndex].creatureTracker.gameObject.SetActive(false);

		// place into pool
		this.creaturePool.Push(creatureIndex);

		// update count of creatures alive
		--this.CreatureCount;
	}

	/// <summary>
	/// Checks the creature posiiton and kills them if
	/// they aren't in the radius
	/// </summary>
	/// <param name="radius">Radius.</param>
	/// <param name="waterLevelOffset">Water level offset.</param>
	public void UpdateCreatureInfo(float radius, float waterLevelOffset)
	{
		for(int i = 0; i < this.creatures.Length; ++i)
		{
			// do nothing if the game object is currently not active
			if(this.creatures[i] == null || !this.creatures[i].creatureTracker.gameObject.activeSelf)
			{
				continue;
			}

			// check if the creature recently died
			if(this.creatures[i].creatureTracker.IsDead)
			{
				this.PutCreatureInPool(i);
			}

            // get reference to creature transform
            Transform transform = this.creatures[i].creatureTracker.gameObject.transform;

            // check if creature is outside of the radius. 
            Vector2 creaturePosition = VectorUtility.XZ(transform.position);
			Vector2 playerPosition   = VectorUtility.XZ(Game.Instance.PlayerInstance.WorldPosition);

			if(Vector2.Distance(creaturePosition, playerPosition) > radius)
			{
				// kill the creature if outside of the given radius
				this.PutCreatureInPool(i);
			}

            // set creature height to be at the water level with the offset
			transform.position = new Vector3(transform.position.x,
			                                 Game.Instance.WaterLevelHeight + waterLevelOffset,
			                                 transform.position.z);
		}
	}

	/// <summary>
	/// Updates the creature count by spawning more creatures
	/// if we are below the ideal creature count. If we are
	/// above time will eventually remedy this.
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="creature">Creature.</param>
	public void UpdateCreatureCount(Vector3 position, GameObject creature)
	{
		if(this.CreatureCount < this.IdealCreatureCount)
		{
			this.Spawn(position, creature);
		}	
	}

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatureManager"/> class.
    /// </summary>
    /// <param name="maxCount"></param>
    /// <param name="idealCount"></param>
    /// <param name="parent"></param>
    public CreatureManager(int maxCount, int idealCount, Transform parent)
	{
		this.creaturePool       = new Stack<int>();
        this.creatures          = new CreatureInfo[maxCount];
		this.IdealCreatureCount = idealCount;
		this.CreatureCount      = 0;
        this.parent             = parent;
	}
}
