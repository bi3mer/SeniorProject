using UnityEngine;
using System.Collections;

/// <summary>
/// This class handles particle effects for miscellaneous debris, such as sand, leaves, dirt, and paper, in relation to them blowing around in the wind during the game.
/// This is structured very similarly to the RainController class, except that this only handles one particle system at a time, without blending.
/// </summary>

public class DebrisController : MonoBehaviour 
{
	// This is for testing.
	[Tooltip("Set to true to use your own values for testing")]
	public bool UseCustomValues;

	[Tooltip("The debris particle system")]
	public ParticleSystem Debris;

	[SerializeField]
	[Range(0,20)]
	private float windMitigation = 8f;

	[SerializeField]
	[Range(0,10)]
	private float debrisMultiplier = 2f;

	[SerializeField]
	[Range(0,100)]
	[Tooltip("The amount of debris getting kicked around")]
	private float debrisLevel;

	/// <summary>
	/// At what point should we start kicking up debris?
	/// </summary>
	[Range(0,100)]
	[Tooltip("The threshold at which debris begins appearing")]
	public float DebrisStartThreshold = 40f;

	private ParticleSystemRenderer MainDebrisRenderer;

	[SerializeField]
	private Vector2 windVectorXZ;

	/// <summary>
	/// The wind vector in XZ (considering Y is Up, but we're only concerned with 2D wind)
	/// </summary>
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

	/// <summary>
	/// Gets or sets the amount of debris we should be kicking up, in a value between 0 and 100.
	/// </summary>
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



	/// <summary>
	/// Grab the renderer.
	/// </summary>
	void Start()
	{
		// Grab the renderer.
		MainDebrisRenderer = Debris.GetComponent<ParticleSystemRenderer>();

		Debris = GetComponent<ParticleSystem>();
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
		ParticleSystem.EmissionModule DebrisEmission = Debris.emission;

		// Set the Debris emission rate, if it's less than the threshold turn off emission. If not, multiply it by a given value.
		if (DebrisLevel < DebrisStartThreshold)
		{
			DebrisEmission.rate = 0f;
		}
		else
		{
			DebrisEmission.rate = debrisLevel * debrisMultiplier;
		}

		// Set the wind speed via velocity over lifetime
		ParticleSystem.VelocityOverLifetimeModule ParticleVelocity = Debris.velocityOverLifetime;
		ParticleVelocity.x = WindVectorXZ.x;
		ParticleVelocity.y = -WindVectorXZ.y;
		ParticleVelocity.z = -WindVectorXZ.y;

		// Set the rotation of the rain based on the wind.
		Debris.startRotation3D = new Vector3(Mathf.Deg2Rad* -WindVectorXZ.y, Mathf.Deg2Rad* -WindVectorXZ.x, 0f);
	}
}