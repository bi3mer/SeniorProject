using UnityEngine;
using System.Collections;

public class FishSpawner : CreatureSpawner 
{
	[Tooltip("Time till the creature count will be reset after a storm to be a middle value again.")]
	[SerializeField]
	private float updateAfterStormEndTime = 20f;

	private IEnumerator coroutine;

	/// <summary>
	/// Updates the creature count after storm after a given time.
	/// </summary>
	/// <returns>The creature count after storm.</returns>
	private IEnumerator updateCreatureCountAfterStorm()
	{
		yield return new WaitForSeconds(this.updateAfterStormEndTime);

		// set creature count to be the mean again
		this.creatureManager.IdealCreatureCount = this.meanCreatureCount;

		// reset the coroutine class variable
		this.coroutine = null;
	}

	/// <summary>
	/// Event for when a storm has started
	/// </summary>
	private void stormStartEvent()
	{
		this.creatureManager.IdealCreatureCount = this.minCreatureCount;
	}

	/// <summary>
	/// Event for when a storm has ended
	/// </summary>
	private void stormEndEvent()
	{
		this.creatureManager.IdealCreatureCount = this.maxCreatureCount;

		// check if a coroutine is currently running
		if(this.coroutine != null)
		{
			// stop the running coroutine
			StopCoroutine(this.coroutine);
		}

		// create a new coroutine and start it
		this.coroutine = this.updateCreatureCountAfterStorm();
		StartCoroutine(this.coroutine);
	}

	/// <summary>
	/// Ovveride the init function to subscribe to storms, so amount of
	/// fish on screen can be controlled
	/// </summary>
	protected override void Init()
	{
		Game.Instance.EventManager.StormStartedSubscription += this.stormStartEvent;
		Game.Instance.EventManager.StormStoppedSubscription += this.stormEndEvent;
		this.coroutine = null;
	}
}
