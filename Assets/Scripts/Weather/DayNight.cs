using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour 
{
	[SerializeField]
	private Transform sun;

	[SerializeField] 
	private Transform moon;

	private float previousTime = 0f;

	/// <summary>
	/// Updates the planetary object transform as it rotates around
	/// our world.
	/// </summary>
	private void updatePlanetaryObject(Transform planetaryObject, float angle)
	{
		// TODO: should the be able to rotate around something other than the origin?
		planetaryObject.RotateAround(Vector3.zero, Vector3.right, angle);
		planetaryObject.LookAt(Vector3.zero);
	}

	/// <summary>
	/// Updates the transforms of the sun and the moon
	/// </summary>
	/// <returns>The transforms.</returns>
	void Update()
	{
		float angle = (Game.Instance.ClockInstance.CurrentTime - this.previousTime) / Game.Instance.ClockInstance.TwelveHours;

		this.updatePlanetaryObject(this.sun,  angle);
		this.updatePlanetaryObject(this.moon, angle);
	}
}