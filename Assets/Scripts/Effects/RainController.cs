using UnityEngine;
using System.Collections;


/// <summary>
/// This class handles the rain effect that is present throughout the entire game.
/// 
/// 
/// CURRENT PROBLEMS
/// Currently there seems to be an issue with Unity running out of memory and crashing when editing the particle system.
/// I have plenty of ram to spare so I think something else is going on. People online have said it may have to do with sub-emitters so I'm going to forgoe them for now.
/// There is still a sub emitter on RainMain you can turn on but it may make the system unstable.
/// </summary>

[ExecuteInEditMode]
public class RainController : MonoBehaviour
{
    // Turn this on for testing. Ideally in the future other scripts will call functions in this script to change the weather
    public bool UseCustomValues;

    // I'm setting these systems as public so that the script can run in Edit Mode.
    // The main rain drops.
    public ParticleSystem MainRain;

    // The fog that falls to simulate extra rain.
    public ParticleSystem RainFog;

    // The ammount of rain that can fall on a range between 0 to 100
    public float RainLevel
    {
        get
        {
            return rainLevel;
        }
        set
        {
            if (0f <= value && value <= 100f)
            {
                rainLevel = value;
                UpdateParticleSystem();
            }
            else
            {
                Debug.LogError("Rain Ammount can only accept values between 0 and 100");
            }
        }
    }

    [SerializeField]
    [Range(0,20)]
    private float windMitigation = 8f;

    [SerializeField]
    [Range(0, 100)]
    private float rainLevel;

    //  When should rainFog kick in?
    [Range(0, 100)]
    [SerializeField]
    public float FogStartThreshold = 50f;

    // How much fog to rain there should be
    [SerializeField]
    private float fogMod;

    // The wind vector in XZ (considering Y is Up, but we're only concerned with 2d wind)
    public Vector2 WindVectorXZ
    {
        get
        {
            return windVectorXZ;
        }
        set
        {
            windVectorXZ = value;
            UpdateParticleSystem();
        }
    }
    [SerializeField]
    private Vector2 windVectorXZ;

    // How Much wind should effect the raind fog (Probably set this to a low number since it falls slower than the main rain. So we want it to move less. .4 seems close)
    [SerializeField]
    private float fogWindMod = .4f;

    private ParticleSystemRenderer MainRainRenderer;

    // The attribute associated with the particle shader that controlls the blend between the rain textures.
    private const string particleMaterialBlendAttribute = "_Opacity";

    /// <summary>
    /// Grab the renderer to set the material's blend amount.
    /// </summary>
    void Start()
    {
        // Grab the renderer.
        MainRainRenderer = MainRain.GetComponent<ParticleSystemRenderer>();
    }

	/// <summary>
	/// Update this instance.
	/// </summary>
    void Update()
    {
		if (UseCustomValues)
        {
            UpdateParticleSystem();
        }
        else
        {
			this.WindVectorXZ = Game.Instance.WeatherInstance.WindDirection2d / this.windMitigation;
        	this.RainLevel    = Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.Precipitation];
        }
    }

    /// <summary>
    /// Sets values in the associated particle systems based off of public values on the script.
    /// </summary>
    private void UpdateParticleSystem()
    {
        //Set RainMain Emission to double rain level. At 200 emission we almost max out particles. (Depending on the height the rain spawns at this may change, but it'll be based on the camera in the future)
        ParticleSystem.EmissionModule RainEmission = MainRain.emission;
        RainEmission.rate = RainLevel * 2f;

        //set the RainFog emission rate, if it's less than the threshold turn off emission.
        RainEmission = RainFog.emission;
        if (RainLevel < FogStartThreshold)
        {
            RainEmission.rate = 0f;
        }
        else
        {
            RainEmission.rate = RainLevel * fogMod;
        }

        //If in editor, there's a chance the script is running in edit mode and we won't have the renderer set.   
        #if (UNITY_EDITOR)
        if (MainRainRenderer == null)
        {
            MainRainRenderer = GetComponent<ParticleSystemRenderer>();
        }
        #endif

        // Set the blend to be equal to the rain ammount
        MainRainRenderer.sharedMaterial.SetFloat(particleMaterialBlendAttribute, RainLevel);

        // Set the wind speed via velocity over lifetime
        ParticleSystem.VelocityOverLifetimeModule ParticleVelocity = MainRain.velocityOverLifetime;
        ParticleVelocity.x = WindVectorXZ.x;
        ParticleVelocity.y = -WindVectorXZ.y;
        ParticleVelocity = RainFog.velocityOverLifetime;
        ParticleVelocity.x = WindVectorXZ.x * fogWindMod;
        ParticleVelocity.z = -WindVectorXZ.y * fogWindMod;

        // Set the rotation of the rain based on the wind.
        MainRain.startRotation3D = new Vector3(Mathf.Deg2Rad*-WindVectorXZ.y, Mathf.Deg2Rad*-WindVectorXZ.x, 0f);
    }
}