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
    //Turn this on for testing. Ideally in the future other scripts will call functions in this script to change the weather
    public bool UpdateEveryFrame;


    //I'm setting these systems as public so that the script can run in Edit Mode.
    //The main rain drops.
    public ParticleSystem MainRain;
    //The fog that falls to simulate extra rain.
    public ParticleSystem RainFog;

    [Range(0, 100)]
    [SerializeField]
    private float _rainLevel;

    //When should rainFog kick in?
    [Range(0, 100)]
    [SerializeField]
    public float FogStartThreshold = 50f;

    //How much fog to rain there should be
    [SerializeField]
    private float _fogMod;

    //The wind vector in XZ (considering Y is Up, but we're only concerned with 2d wind)
    [SerializeField]
    private Vector2 _windVectorXZ;

    //How Much wind should effect the raind fog (Probably set this to a low number since it falls slower than the main rain. So we want it to move less. .4 seems close)
    [SerializeField]
    private float _fogWindMod = .4f;

    private ParticleSystemRenderer MainRainRenderer;

    /// <summary>
    /// Grab the renderer to set the material's blend amount.
    /// </summary>
    void Start()
    {
        //Grab the renderer.
        MainRainRenderer = MainRain.GetComponent<ParticleSystemRenderer>();
    }

    /// <summary>
    /// In editor we want to check for updates every frame, everywhere else that's no good.
    /// </summary>
    #if (UNITY_EDITOR)
    void Update()
    {
        if (UpdateEveryFrame)
        {
            UpdateParticleSystem();
        }
    }
     #endif

    /// <summary>
    /// Sets values in the associated particle systems based off of public values on the script.
    /// </summary>
    public void UpdateParticleSystem()
    {
        //Set RainMain Emission to double rain level. At 200 emission we almost max out particles. (Depending on the height the rain spawns at this may change, but it'll be based on the camera in the future)
        ParticleSystem.EmissionModule RainEmission = MainRain.emission;
        RainEmission.rate = _rainLevel * 2f;

        //set the RainFog emission rate, if it's less than the threshold turn off emission.
        RainEmission = RainFog.emission;
        if (_rainLevel < FogStartThreshold)
        {
            RainEmission.rate = 0f;
        }
        else
        {
            RainEmission.rate = _rainLevel * _fogMod;
        }

        //If in editor, there's a chance the script is running in edit mode and we won't have the renderer set.   
        #if (UNITY_EDITOR)
        if (MainRainRenderer == null)
        {
            MainRainRenderer = GetComponent<ParticleSystemRenderer>();
        }
#endif

        //Set the blend to be equal to the rain ammount
        MainRainRenderer.sharedMaterial.SetFloat("_Opacity", _rainLevel);

        //Set the wind speed via velocity over lifetime
        ParticleSystem.VelocityOverLifetimeModule ParticleVelocity = MainRain.velocityOverLifetime;
        ParticleVelocity.x = _windVectorXZ.x;
        ParticleVelocity.y = -_windVectorXZ.y;
        ParticleVelocity = RainFog.velocityOverLifetime;
        ParticleVelocity.x = _windVectorXZ.x * _fogWindMod;
        ParticleVelocity.z = -_windVectorXZ.y * _fogWindMod;

        //Set the rotation of the rain based on the wind.
        MainRain.startRotation3D = new Vector3(Mathf.Deg2Rad*-_windVectorXZ.y, Mathf.Deg2Rad*-_windVectorXZ.x, 0f);
    }

    /// <summary>
    /// Sets the rain level to a new value and updates the particle system.
    /// </summary>
    /// <param name="rainAmmount">A value for the rain between 0 and 100</param>
    public void SetRain(float rainAmmount)
    {
        if(0f <= rainAmmount && rainAmmount <= 100f)
        {
            _rainLevel = rainAmmount;
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
        _windVectorXZ = windVector;
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
            _rainLevel = rainAmmount;
            _windVectorXZ = windVector;

            UpdateParticleSystem();
        }
        else
        {
            Debug.LogError("Rain Ammount set to a bad value");
        }
    }
}
