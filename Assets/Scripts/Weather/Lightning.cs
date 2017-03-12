using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour 
{
	[SerializeField]
	private GameObject lightningPrefab;

	[SerializeField]
	private float gameMinutesTillFirstLightningStrike = 1.0f;

	[SerializeField]
	private float minGameTimeForLightningStrike = 10.0f;

	[SerializeField]
	private float maxGameTimeForLightningStrike = 30.0f;

	[SerializeField]
	private float lightingYSpawnOffset = 10.0f;

	private GameObject lightningInstance;

	/// <summary>
	/// Waits for a set time and spawns the next lightning strike
	/// </summary>
	/// <returns>The next lightning strike.</returns>
	private IEnumerator spawnNextLightningStrike()
	{
		// wait and spawn next lightning
		yield return new WaitForSeconds(Random.Range(this.minGameTimeForLightningStrike, this.maxGameTimeForLightningStrike));
		this.spawnLightning();
	}

	/// <summary>
	/// Finds the lightning spawn location.
	/// </summary>
	/// <returns>The lightning spawn location.</returns>
	private Vector3 findLightningSpawnLocation()
	{
		// get all of the low pressure systems currently in the game
		List<PressureSystem> lowPressureSystems = Game.Instance.WeatherInstance.WeatherPressureSystems.LowPressureSystems();

		// sort the pressure systems for lowest to highest
		lowPressureSystems.Sort(delegate(PressureSystem x, PressureSystem y) {
			return x.Pressure.CompareTo(y.Pressure);
		});

		// get a random index of the array that is slightly more likely to be lower than higher
		int randomIndex = Mathf.FloorToInt(RandomUtility.RandomPercent * lowPressureSystems.Count);

		return new Vector3(lowPressureSystems[randomIndex].Position.x,
		                   Game.Instance.PlayerInstance.WorldTransform.position.y + this.lightingYSpawnOffset,
		                   lowPressureSystems[randomIndex].Position.y
		);
	}

	/// <summary>
	/// Spawns the lightning.
	/// </summary>
	private void spawnLightning()
	{
		if(this.lightningInstance == null)
		{
			this.lightningInstance = Instantiate(this.lightningPrefab);
			this.lightningInstance.SetActive(false);
		}

		this.lightningInstance.transform.position = this.findLightningSpawnLocation();
		this.lightningInstance.SetActive(true);

		StartCoroutine(this.spawnNextLightningStrike());

		// notify subscribers
		Game.Instance.EventManager.LightningStrike (this.lightningInstance.transform.position);
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	IEnumerator Start () 
	{
		// scale time to fit with game time
		this.minGameTimeForLightningStrike *= Game.Instance.ClockInstance.Tick;
		this.maxGameTimeForLightningStrike *= Game.Instance.ClockInstance.Tick;

		// Wait before first lightning strike
		yield return new WaitForSeconds(this.gameMinutesTillFirstLightningStrike * Game.Instance.ClockInstance.Tick);

		this.spawnLightning();
	}
}
