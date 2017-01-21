using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour 
{
	[SerializeField]
	private Transform sun;

	[SerializeField] 
	private Transform moon;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		Game.Instance.ClockInstance.SecondUpdate += this.updateTransforms;
	}

	/// <summary>
	/// Updates the planetary object transform as it rotates around
	/// our world.
	/// </summary>
	private void updatePlanetaryObject(Transform planetaryObject)
	{
		// TODO: should the be able to rotate around something other than the origin?
		planetaryObject.RotateAround(Vector3.zero, Vector3.right, Game.Instance.ClockInstance.AnglePerSecond);
		planetaryObject.LookAt(Vector3.zero);
	}

	/// <summary>
	/// Updates the transforms of the sun and the moon
	/// </summary>
	/// <returns>The transforms.</returns>
	private void updateTransforms()
	{
		this.updatePlanetaryObject(this.sun);
		this.updatePlanetaryObject(this.moon);
	}
}