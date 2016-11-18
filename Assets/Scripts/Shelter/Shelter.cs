using UnityEngine;
using System.Collections;

public class Shelter : MonoBehaviour 
{
	private Collider collider;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		// gets the collider for the shelter
		collider = GetComponent<BoxCollider> ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{

	} 

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="coll">Collider entered</param>
	public void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.GetComponent<PlayerController>()) 
		{
			PlayerController p = coll.gameObject.gameObject.GetComponent <PlayerController>();
			p.IsInShelter = true;
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
			PlayerController p = coll.gameObject.gameObject.GetComponent <PlayerController>();
			p.IsInShelter = false;
		}
	}
}
