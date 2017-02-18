using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles particle effects for miscellaneous debris, such as sand, leaves, dirt, and paper, in relation to them blowing around in the wind during the game.
/// This is structured very similarly to the RainController class, except that this only handles one particle system at a time, without blending.
/// </summary>

[ExecuteInEditMode]
public class DebrisController : MonoBehaviour 
{
	//This is for testing.
	public bool UseCustomValues;

	public ParticleSystem Debris;

	[SerializeField]
	[Range(0,20)]
	private float windMitigation = 8f;

	[SerializeField]
	[Range(0,100)]
	private float debrisLevel;

	//When should we start kicking up debris?
	[SerializeField]
	[Range(0,100)]
	public float DebrisStartThreshold = 40f;

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

	public float DebrisLevel
	{
		get 
		{
			return debrisLevel;
		}
		set 
		{
			// keep the value between 0 and 100
			debrisLevel = Mathf.Clamp(value, 0f, 100f);
			UpdateParticleSystem ();
		}
	}

	private ParticleSystemRenderer MainDebrisRenderer;

	private ParticleSystem debrisParticleSystem;

	/// <summary>
	/// Grab the renderer.
	/// </summary>
	void Start()
	{
		// Grab the renderer.
		MainDebrisRenderer = Debris.GetComponent<ParticleSystemRenderer>();

		debrisParticleSystem = GetComponent<ParticleSystem>();
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
			this.DebrisLevel = Game.Instance.WeatherInstance.WeatherInformation [(int)Weather.WindSpeedMagnitude];
		}
	}

	/// <summary>
	/// Sets values in the associated particle systems based off of public values on the script.
	/// </summary>
	private void UpdateParticleSystem()
	{
		//Set Debris Emission to double debris level. At 200 emission we almost max out particles.
		ParticleSystem.EmissionModule DebrisEmission = Debris.emission;
		DebrisEmission.rate = debrisLevel * 2f;

		//set the Debris emission rate, if it's less than the threshold turn off emission.
		if (DebrisLevel < DebrisStartThreshold)
		{
			DebrisEmission.rate = 0f;
		}
		else
		{
			DebrisEmission.rate = DebrisLevel;
		}

		//If in editor, there's a chance the script is running in edit mode and we won't have the renderer set.   
		#if (UNITY_EDITOR)
		if (MainDebrisRenderer == null)
		{
			MainDebrisRenderer = GetComponent<ParticleSystemRenderer>();
		}
		#endif

		// Set the wind speed via velocity over lifetime
		ParticleSystem.VelocityOverLifetimeModule ParticleVelocity = Debris.velocityOverLifetime;
		ParticleVelocity.x = WindVectorXZ.x;
		ParticleVelocity.y = -WindVectorXZ.y;
		ParticleVelocity.x = WindVectorXZ.x;
		ParticleVelocity.z = -WindVectorXZ.y;

		// Set the rotation of the rain based on the wind.
		Debris.startRotation3D = new Vector3(Mathf.Deg2Rad* -WindVectorXZ.y, Mathf.Deg2Rad* -WindVectorXZ.x, 0f);
	}

}
