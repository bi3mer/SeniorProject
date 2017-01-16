using UnityEngine;
using System.Collections;

public class FloatBehavior : MonoBehaviour 
{
	/// <summary>
	/// How high the object will float
	/// </summary>
	private const float floatHeight = 1f;

	/// <summary>
	/// The damp on the bounce.
	/// </summary>
	private float bounceDamp = 0.5f;

	/// <summary>
	/// The rigid body of the object.
	/// </summary>
	private Rigidbody rigidBody;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// Sets the maximum height the object can go until it is completely out of the water
	/// </summary>
	/// <param name="height">Height.</param>
	public void SetFloatHeight(float height)
	{
		floatHeight = height;
	}

	/// <summary>
	/// Update this instance and makes it bob up and down.
	/// </summary>
	void Update()
	{
		// percentage out of water, with 100% being 1f
		float forceFactor = 1f - ((transform.position.y - Game.Instance.WaterLevelHeight) / floatHeight);

		// if below or floating
		if (forceFactor > 0f) 
		{
			Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * bounceDamp);
			rigidBody.AddForceAtPosition(uplift, transform.position);
		}
	}
}
