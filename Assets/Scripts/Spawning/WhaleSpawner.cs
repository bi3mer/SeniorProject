using UnityEngine;
using System.Collections;

public class WhaleSpawner : Spawner 
{
	/// <summary>
	/// Get a location where the unit can be spawned
	/// </summary>
	/// <returns>The location.</returns>
	protected override Vector3 findSpawnLocation()
	{
		return Vector3.zero;
	}
}
