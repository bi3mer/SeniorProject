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

    /// <summary>
    /// Set max height for the sun and the moon. Also moves the position of
    /// the GameObject if it is not centered at 0,0,0
    /// </summary>
    void Awake()
    {
    	// get max values for height of sun and moon for percentage calculation
        this.moonMaxHeight = Mathf.Abs(moon.transform.position.y);
        this.sunMaxHeight  = Mathf.Abs(sun.transform.position.y);

        // if the weather effects isn't centered, center it. Also if this is the
        // editor, provide a nice messsage for debugging
        #if UNITY_EDITOR
        if(this.transform.position != Vector3.zero)
        {
        	Debug.Log("Weather effects is not centered, centering it.");
        }
        #endif
        this.transform.position = Vector3.zero;
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
    	// if the time is not starting at a time where it is not night
    	// then update reverse the positions of the sun and the moon
    	if(Game.Instance.ClockInstance.CurrentTime >= Game.Instance.ClockInstance.TwelveHours)
    	{
    		SystemLogger.Write("Flipping position of sun and moon at start of game");

    		// store temp variables
    		Vector3 tempPosition    = this.sun.transform.position;
    		Quaternion tempRotation = this.sun.transform.rotation;

    		// swap sun information with the moon
    		this.sun.transform.position  = this.moon.transform.position;
    		this.sun.transform.rotation  = this.moon.transform.rotation;

    		// use temp variables to set the moon's new rotation and position
    		this.moon.transform.position = tempPosition;
    		this.moon.transform.rotation = tempRotation;
    	}
    }

    /// <summary>
    /// Updates the planetary object transform as it rotates around
    /// our world.
    /// </summary>
    private void updatePlanetaryObject(Light planetaryObject, float angle, bool isSun)
    {
        // rotate the lights. They're directional lights so the position literally doesn't matter colan.
        planetaryObject.transform.Rotate(new Vector3(angle, 0f, 0f));

        // set intensity of values for son or the moon. if below the water
        // then this should not be affecting the world
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
		float angle = (Game.Instance.ClockInstance.CurrentTime % Game.Instance.ClockInstance.TwentyFourHours) / Game.Instance.ClockInstance.TwentyFourHours;

		this.updatePlanetaryObject(this.sun,  angle, true);
		this.updatePlanetaryObject(this.moon, angle, false);
	}
}