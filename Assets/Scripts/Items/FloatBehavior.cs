using UnityEngine;
using System.Collections;

public class FloatBehavior : MonoBehaviour 
{
	private const string waterLayer = "Water";
	private float floatHeight = 1f;
	private float bounceDamp = 0.5f;

	private Rigidbody rigidBody;

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
	/// Updates the floating physics.
	/// </summary>
	IEnumerator applyFloatPhysics () 
	{
		while(true)
		{
			float forceFactor = 1f - ((transform.position.y - Game.Instance.WaterLevelHeight) / floatHeight);
			
			if (forceFactor > 0f) 
			{
				Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * bounceDamp);
				rigidBody.AddForceAtPosition(uplift, transform.position);
			}

			yield return null;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer(waterLayer))
		{
			StartCoroutine(applyFloatPhysics());
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer(waterLayer))
		{
			StopCoroutine(applyFloatPhysics());
		}
	}
}
