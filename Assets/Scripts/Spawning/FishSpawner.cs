using UnityEngine;
using System.Collections;

public class FishSpawner : Spawner 
{
	protected override Vector3 findSpawnLocation()
	{
		return Vector3.zero;
	}
}
