using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour 
{
	[SerializeField]
	private Light sun;

	[SerializeField] 
	private Light moon;

    [SerializeField]
    private float moonMinBrightness;
    [SerializeField]
    private float moonMaxBrightness;

    [SerializeField]
    private float sunMinBrightness;
    [SerializeField]
    private float sunMaxBrightness;

    
    // Max heights are based on the starting transform of the sun and moon. (the starting Y value)
    private float moonMaxHeight;
    private float sunMaxHeight;

    private float previousTime = 0f;

    /// <summary>
    /// Get the sun and moon
    /// </summary>
    void Awake()
    {
        moonMaxHeight = Mathf.Abs(moon.transform.position.y);
        sunMaxHeight = Mathf.Abs(sun.transform.position.y);
    }

    /// <summary>
    /// Updates the planetary object transform as it rotates around
    /// our world.
    /// </summary>
    private void updatePlanetaryObject(Light planetaryObject, float angle, bool isSun)
    {
        // TODO: should the be able to rotate around something other than the origin?
        planetaryObject.transform.RotateAround(Vector3.zero, Vector3.right, angle);
        planetaryObject.transform.LookAt(transform.position);
        if (planetaryObject.transform.position.y < Game.Instance.WaterLevelHeight)
        {
            planetaryObject.intensity = 0f;
        }
        else
        {
            if (isSun)
            {
                planetaryObject.intensity = Mathf.Lerp(sunMinBrightness, sunMaxBrightness, (planetaryObject.transform.position.y - transform.position.y) / sunMaxHeight);
            }
            else
            {
                planetaryObject.intensity = Mathf.Lerp(moonMinBrightness, moonMaxBrightness, (planetaryObject.transform.position.y - transform.position.y) / moonMaxHeight);
            }
        }

	}

	/// <summary>
	/// Updates the transforms of the sun and the moon
	/// </summary>
	void Update()
	{
		float angle = (Game.Instance.ClockInstance.CurrentTime - this.previousTime) / Game.Instance.ClockInstance.TwelveHours;

		this.updatePlanetaryObject(this.sun,  angle, true);
		this.updatePlanetaryObject(this.moon, angle, false);
	}
}