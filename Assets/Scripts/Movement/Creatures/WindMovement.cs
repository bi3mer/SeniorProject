using UnityEngine;
using System.Collections;

public class WindMovement : MonoBehaviour 
{
	[SerializeField]
	private float windMitigationConstant = 200f;

	private Rigidbody rb;

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
		this.rb.velocity = Game.Instance.WeatherInstance.WindDirection3d / windMitigationConstant;
	}
}
