using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class WindMovement : MonoBehaviour 
{
	[SerializeField]
	private float windMitigation = 200f;

	private Rigidbody rb;
	private Vector3 previousVelocity;

	/// <summary>
	/// gets rigid body
	/// </summary>
	void Start()
	{
		this.rb = this.GetComponent<Rigidbody>();
		this.previousVelocity = Vector3.zero;
	}

	/// <summary>
	/// Unity physics update for moving the jelly fish
	/// </summary>
	void FixedUpdate()
	{
		// reduce intensity of wind vector on jelly fish
		Vector3 velocity = Game.Instance.WeatherInstance.WindDirection3d / this.windMitigation;
		this.rb.velocity += velocity - this.previousVelocity;
		this.previousVelocity = velocity;
	}
}