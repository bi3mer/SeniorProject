using UnityEngine;
using System.Collections;

public class WeatherSoundSystem : MonoBehaviour
{
	/// <summary>
	/// The default weather event path.
	/// </summary>
	public string DefaultWeatherEventPath = "event:/Ambient/Weather/Basic_Rain";

	/// <summary>
	/// The weather sounds event.
	/// </summary>
	private FMOD.Studio.EventInstance weatherSounds;

	/// <summary>
	/// The lightning sound event path.
	/// </summary>
	[SerializeField]
	private string LightningEventPath = "event:/Ambient/Weather/Basic_Thunder";

	/// <summary>
	/// The lightning sound event.
	/// </summary>
	private FMOD.Studio.EventInstance lightningSound;

	/// <summary>
	/// This divides the windSpeedMagnitude to give us the new volume of the base weather sound
	/// </summary>
	[SerializeField]
	private float weatherIntensityDivisor = 10.0f;

	/// <summary>
	/// The max volume that the weather event will play at.
	/// </summary>
	[SerializeField]
	private float maxWeatherVolume = 3.0f;

	/// <summary>
	/// The current weather volume.
	/// </summary>
	[SerializeField]
	private float weatherVolume = 2.0f;

	/// <summary>
	/// The current lightning volume.
	/// </summary>
	[SerializeField]
	private float lightningVolume = 1.5f;

	/// <summary>
	/// Starts the weather sounds.
	/// </summary>
	void Start()
	{
		weatherSounds = FMODUnity.RuntimeManager.CreateInstance (DefaultWeatherEventPath);
		weatherSounds.setVolume (weatherVolume);
		weatherSounds.start ();

		lightningSound = FMODUnity.RuntimeManager.CreateInstance (LightningEventPath);
		lightningSound.setVolume (lightningVolume);

		// subscribe to event manager for weather updates
		Game.Instance.EventManager.WeatherUpdatedSubscription += updateWeatherIntensity;
		Game.Instance.EventManager.WeatherLightningSubscription += triggerLightning;
	}

	/// <summary>
	/// Updates the weather intensity -- subscribes to WeatherUpdateSubscription
	/// </summary>
	/// <param name="precipitation">Precipitation.</param>
	private void updateWeatherIntensity(float precipitation)
	{
		float newWeatherVolume = precipitation / weatherIntensityDivisor;

		if (newWeatherVolume < maxWeatherVolume) 
		{
			weatherVolume = newWeatherVolume;
			weatherSounds.setVolume (weatherVolume);
		}
	}


	/// <summary>
	/// Plays the lightning strike sound according to the location of the lightning.
	/// </summary>
	/// <param name="lightningPos">Lightning position.</param>
	private void triggerLightning(Vector3 lightningPos)
	{
		lightningSound.set3DAttributes (FMODUnity.RuntimeUtils.To3DAttributes (lightningPos));
		lightningSound.start ();
	}
}
