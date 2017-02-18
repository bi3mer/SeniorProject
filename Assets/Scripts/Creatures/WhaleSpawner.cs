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
		// TODO: update with 4 corners
		return Vector3.zero;
	}
}
