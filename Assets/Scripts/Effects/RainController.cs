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
/// 
/// </summary>

[ExecuteInEditMode]
public class RainController : MonoBehaviour
{
    //Turn this on for testing. Ideally in the future other scripts will call functions in this script to change the weather
    public bool UpdateEveryFrame;


    //I'm setting these systems as public so that the script can run in Edit Mode.
    //The main rain drops.
    public ParticleSystem MainRain;
    //The fog that falls to simulate extra rain.
    public ParticleSystem RainFog;

    //The RainMaterials to switch between.
    public Material LightRain;
    public Material MediumRain;
    public Material HeavyRain;

    //Threshold before medium rain kicks in.
    public float MediumRainThreshold;
    //Threshold before heavy rain kicks in, Has to be higher than Medium Rain.
    public float HeavyRainThreshold;

    [Range(0, 100)]
    public float RainLevel;

    //When should rainFog kick in?
    [Range(0, 100)]
    public float FogStartThreshold = 50f;

    //How much fog to rain there should be
    public float FogMod;

    //The wind vector in XZ (considering Y is Up, but we're only concerned with 2d wind)
    public Vector2 WindVectorXZ;

    //How Much wind should effect the raind fog (Probably set this to a low number since it falls slowe than the main rain. So we want it to move less. .4 seems close)
    public float FogWindMod = .4f;

    private ParticleSystemRenderer MainRainRenderer;

    // Use this for initialization
    void Start()
    {
        MainRainRenderer = MainRain.GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateEveryFrame)
        {
            UpdateParticleSystem();
        }
    }


    /// <summary>
    /// Sets values in the associated particle systems based off of public values on the script.
    /// </summary>
    public void UpdateParticleSystem()
    {
        //Set RainMain Emission to double rain level. At 200 emission we almost max out particles. (Depending on the height the rain spawns at this may change, but it'll be based on the camera in the future)
        ParticleSystem.EmissionModule RainEmission = MainRain.emission;
        RainEmission.rate = RainLevel * 2f;

        //set the rainfog emission rate, if it's less than the threshold turn off emission.
        RainEmission = RainFog.emission;
        if (RainLevel < FogStartThreshold)
        {
            RainEmission.rate = 0f;
        }
        else
        {
            RainEmission.rate = RainLevel * FogMod;
        }

        //Set the RainMain material based on the thresholds.

        //If in editor, chance the script is running in edit mode and we won't have the renderer set.   
        #if (UNITY_EDITOR)
        if (MainRainRenderer == null)
        {
            MainRainRenderer = GetComponent<ParticleSystemRenderer>();
        }
        #endif

        //Check for the rain thresholds
        if (RainLevel < MediumRainThreshold)
        {
            //Lowest
            MainRainRenderer.material = LightRain;

        }
        else if (RainLevel >= HeavyRainThreshold)
        {
            //Highest
            MainRainRenderer.material = HeavyRain;
        }
        else
        {
            //Medium
            MainRainRenderer.material = MediumRain;
        }

        //Set the wind speed via velocity over lifetime
        ParticleSystem.VelocityOverLifetimeModule ParticleVelocity = MainRain.velocityOverLifetime;
        ParticleVelocity.x = WindVectorXZ.x;
        ParticleVelocity.y = -WindVectorXZ.y;
        ParticleVelocity = RainFog.velocityOverLifetime;
        ParticleVelocity.x = WindVectorXZ.x * FogWindMod;
        ParticleVelocity.z = -WindVectorXZ.y * FogWindMod;

        //Set the rotation of the rain based on the wind.
        MainRain.startRotation3D = new Vector3(Mathf.Deg2Rad*-WindVectorXZ.y, Mathf.Deg2Rad*-WindVectorXZ.x, 0f);
    }

    /// <summary>
    /// Sets the rain level to a new value and updates the particle system.
    /// </summary>
    /// <param name="rainAmmount">A value for the rain between 0 and 100</param>
    public void SetRain(float rainAmmount)
    {
        if(0f <= rainAmmount && rainAmmount <= 100f)
        {
            RainLevel = rainAmmount;
            UpdateParticleSystem();
        }
        else
        {
            Debug.LogError("Rain Ammount set to a bad value");
        }
    }

    /// <summary>
    /// Sets the wind to a new value and updates the particle system.
    /// </summary>
    /// <param name="windVector">A vector2 for the wind using x as the global X axis and Y as the global Z axis</param>
    public void SetRain(Vector2 windVector)
    {
        WindVectorXZ = windVector;
        UpdateParticleSystem();
    }

    /// <summary>
    /// Sets the rain level to a new value and the wind to a new value and updates the particle system.
    /// </summary>
    /// <param name="rainAmmount">A value for the rain between 0 and 100</param>
    /// <param name="windVector">A vector2 for the wind using x as the global X axis and Y as the global Z axis</param>
    public void SetRain(float rainAmmount, Vector2 windVector)
    {
        if (0f <= rainAmmount && rainAmmount <= 100f)
        {
            RainLevel = rainAmmount;
            WindVectorXZ = windVector;

            UpdateParticleSystem();
        }
        else
        {
            Debug.LogError("Rain Ammount set to a bad value");
        }
    }


}
