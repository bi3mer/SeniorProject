using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class WindMovement : MonoBehaviour 
{
	[SerializeField]
	private float windMitigation = 200f;

	private Rigidbody rb;

	/// <summary>
	/// gets rigid body
	/// </summary>
	void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
	}

	/// <summary>
	/// Unity physics update for moving the jelly fish
	/// </summary>
	void FixedUpdate()
	{
		// reduce intensity of wind vector on jelly fish
		this.rb.velocity = Game.Instance.WeatherInstance.WindDirection3d / this.windMitigation;
	}
}