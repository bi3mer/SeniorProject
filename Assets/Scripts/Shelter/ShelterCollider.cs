using UnityEngine;
using System.Collections;

public class ShelterCollider : MonoBehaviour 
{
	private PlayerController player;

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="coll">Collider entered</param>
	public void OnTriggerEnter(Collider coll)
	{
		if (player = coll.gameObject.GetComponent<PlayerController>()) 
		{
			player.IsInShelter = true;
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="coll">Collider exited</param>
	public void OnTriggerExit(Collider coll)
	{
		if (coll.gameObject.GetComponent<PlayerController>())
		{
			player.IsInShelter = false;
		}
	}
}
