using UnityEngine;
using System.Collections;

public class WhaleSpawner : CreatureSpawner 
{
	/// <summary>
	/// Get a location where the unit can be spawned
	/// </summary>
	/// <returns>The location.</returns>
	protected override Vector3 findSpawnLocation()
	{
		if(RandomUtility.RandomBool == true)
		{
			return Game.Instance.CityBounds.CityBounds.max;
		}

		return Game.Instance.CityBounds.CityBounds.min;
	}
}
