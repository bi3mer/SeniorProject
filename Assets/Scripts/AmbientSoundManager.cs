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

	void Start()
	{
		ambientMusic = FMODUnity.RuntimeManager.CreateInstance (AmbientMusicPath);
		ambientMusic.start ();
		Game.Instance.EventManager.RadioMusicSubscription += handleRadioMusicChange;
	}

	/// <summary>
	/// Lowers or raises the ambient music; saves the radio music channel state.
	/// </summary>
	private void handleRadioMusicChange(bool radioMusicOn)
	{
		// this check prevents the volume from being constantly set
		if (radioMusicOn != radioOn) 
		{
			radioOn = radioMusicOn;

			float currentVolume = 0.0f;
			ambientMusic.getVolume (out currentVolume);

			float volumeChange = (maxAmbientMusicVolume - minAmbientMusicVolume) / changeFactor;

			if (radioMusicOn) 
			{
				for (int i = 1; i < changeFactor; ++i) 
				{
					ambientMusic.setVolume (currentVolume - (i*volumeChange));
				}
			} 
			else 
			{
				for (int i = 1; i < changeFactor; ++i) 
				{
					ambientMusic.setVolume (currentVolume + (i * volumeChange));
				}
			}
		}
	}

}

