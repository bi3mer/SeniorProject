using UnityEngine;
using System.Collections;

public class WeatherSoundSystem : MonoBehaviour
{
	/// <summary>
	/// The default weather event path.
	/// </summary>
	public string DefaultWeatherEventPath = "event:/Ambient/Weather/Basic_Rain";
	 
	/// <summary>
	/// The wind sound event path.
	/// </summary>
	public string WindPath = "event:/Ambient/Weather/Basic_Wind";

	/// <summary>
	/// The weather sounds event.
	/// </summary>
	private FMOD.Studio.EventInstance weatherSounds;

	/// <summary>
	/// The wind sounds event.
	/// </summary>
	private FMOD.Studio.EventInstance windSounds;

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
	/// Flag triggered by storm subscription 
	/// </summary>
	private bool stormFlag = false;

	/// <summary>
	/// Starts the weather sounds.
	/// </summary>
	void Start()
	{
		// Create relevant sound paths
		weatherSounds = FMODUnity.RuntimeManager.CreateInstance (DefaultWeatherEventPath);
		weatherSounds.setVolume (weatherVolume);
		weatherSounds.start ();

		windSounds = FMODUnity.RuntimeManager.CreateInstance (WindPath);

		// subscribe to event manager for weather updates
		Game.Instance.EventManager.WeatherUpdatedSubscription += updateWeatherIntensity;
		Game.Instance.EventManager.StormStartedSubscription += startStormSounds;
		Game.Instance.EventManager.StormStoppedSubscription += stopStormSounds;
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

			if (stormFlag) 
			{
				windSounds.setVolume (weatherVolume);
			}
		}
	}

	/// <summary>
	/// Start storm sounds.
	/// </summary>
	private void startStormSounds()
	{
		FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.STOPPED;
		windSounds.getPlaybackState (out state);

		if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
		{
			windSounds.setVolume (weatherVolume);
			windSounds.start ();
		}

		stormFlag = true;
	}

	/// <summary>
	/// Stop storm sounds.
	/// </summary>
	private void stopStormSounds()
	{
		FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.STOPPED;
		windSounds.getPlaybackState (out state);

		if (state != FMOD.Studio.PLAYBACK_STATE.STOPPED) 
		{
			windSounds.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
		}

		stormFlag = false;
	}
}
