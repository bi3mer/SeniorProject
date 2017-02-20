using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crosstales.RTVoice;

public class AmbientSoundManager : MonoBehaviour
{
	/// <summary>
	/// The ambient music track base path.
	/// </summary>
	[SerializeField]
	public string AmbientMusicPath = "event:/Ambient/Music/Basic_Music";

	private FMOD.Studio.EventInstance ambientMusic;

	/// <summary>
	/// The max ambient music volume, set when readio music channel is  off
	/// </summary>
	[SerializeField]
	private float maxAmbientMusicVolume = 1.0f;

	/// <summary>
	/// The minimum ambient music volume, set when radio music channel is on.
	/// </summary>
	[SerializeField]
	private float minAmbientMusicVolume = 0.1f;

	/// <summary>
	/// How many steps it takes for the volume to be turned up or down.
	/// Too high and there will be lagging.
	/// </summary>
	[SerializeField]
	private int changeFactor = 10;

	private bool radioOn = false;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		// Set up the music and start it
		ambientMusic = FMODUnity.RuntimeManager.CreateInstance (AmbientMusicPath);
		ambientMusic.setVolume (maxAmbientMusicVolume);
		ambientMusic.start ();

		// Subscribe so we can lower the music when the radio music channel is on
		Game.Instance.EventManager.RadioMusicSubscription += handleRadioMusicChange;
	}

	/// <summary>
	/// Lowers or raises the ambient music; saves the radio music channel state.
	/// TODO: find a better way to do the fading out -- look into FMOD AHDSR usage.
	/// </summary>
	private void handleRadioMusicChange(bool radioMusicOn)
	{
		// this check prevents the volume from being constantly set
		if (radioMusicOn != radioOn) 
		{
			radioOn = radioMusicOn;

			float currentVolume = 0.0f;
			ambientMusic.getVolume (out currentVolume);

			if (radioMusicOn) 
			{
				// we base the volume change off of the current volume vs. the minimum
				float volumeChange = (currentVolume - minAmbientMusicVolume) / changeFactor;

				for (int i = 1; i < changeFactor + 1; ++i) 
				{
					ambientMusic.setVolume (currentVolume - (i*volumeChange));
				}
			} 
			else 
			{
				// we base the volume change off of the current volume vs. the maximum
				float volumeChange = (maxAmbientMusicVolume - currentVolume) / changeFactor;

				for (int i = 1; i < changeFactor + 1; ++i) 
				{
					ambientMusic.setVolume (currentVolume + (i * volumeChange));
				}
			}
		}
	}

}

