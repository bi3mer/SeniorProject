using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crosstales.RTVoice;
using System.Collections.Generic;

//radio stations
public enum RadioChannel {Music, Weather, Mystery, Null};

public class Radio : MonoBehaviour
{
	// CHANNELS
	[SerializeField]
	private FMOD.Studio.EventInstance musicChannel;
	private List<string> musicCarousel;
	private Dictionary<string, int> musicCarouselClipLengths;
	private int musicClipsTotalLength = 0;

	// Music channel volume during storm and normally
	float musicLowVol = 0.25f;
	float musicRegVol = 1f;

	[SerializeField]
	private FMOD.Studio.EventInstance mysteryChannel;
	private List<string> mysteryCarousel;
	private Dictionary<string, int> mysteryCarouselClipLengths;
	private int mysteryClipsTotalLength = 0;

	// Mystery clips sorted by intensity phase and the current index of where we are at
	private List<List<string>> mysteryPhaseClips;
	private int mysteryPhaseIndex = 0;

	// Static channel right now just plays when current active channel is null
	[SerializeField]
	private FMOD.Studio.EventInstance staticChannel;

	// Weather still has an audio source rather than an FMOD instance for text-to-speech purposes
	[SerializeField]
	private AudioSource weather;

	// Default channel sound event paths
	[SerializeField]
	public string MusicDefaultPath = "event:/Radio/Music/Zero_Rain";
	[SerializeField]
	public string MysteryDefaultPath = "event:/Radio/Mystery/Mystery1";
	[SerializeField]
	public string StaticDefaultPath = "event:/Radio/Static/Basic_Static";

	// Static overlay for storms
	[SerializeField]
	private FMOD.Studio.EventInstance staticOverlay;

	[SerializeField]
	private bool staticOverlayOn;

	[SerializeField]
	private float staticOverlayVolume = 5f;
	 
	[SerializeField]
	private Dial dial;

	private bool isOn;

	private RadioChannel CurrentChannel { get; set; }
	private string announcement;

	private bool weatherStarting = false;

	// Counter for how many times weather played
	private int weatherCounter = 0;
	private WeatherSystem currentWeather;

	// Counter for which sound in carousels we are on. Resets to zero at end of carousel
	private int mysteryCounter = 0;
	private int musicCounter = 0;

	// Millisecond conversion
	private const int millisecond = 1000;

	[Tooltip("The lowest degree of the range of the knob's rotation in which music will play")]
	public float lowMusic;
	[Tooltip("The highest degree of the range of the knob's rotation in which music will play")]
	public float highMusic;
	[Tooltip("The lowest degree of the range of the knob's rotation in which the mystery channel will play")]
	public float lowMystery;
	[Tooltip("The highest degree of the range of the knob's rotation in which the mystery channel will play")]
	public float highMystery;
	[Tooltip("The lowest degree of the range of the knob's rotation in which the weather will play")]
	public float lowWeather;
	[Tooltip("The highest degree of the range of the knob's rotation in which the weather will play")]
	public float highWeather;

	[SerializeField]
	private float volumeDecrease;
	[SerializeField]
	private float volumeIncrease;

    // the lowest is always 0
    private const float lowestVolume = 0;
    // the highest possible volume is 1
    private const float highestVolume = 1f;

    [SerializeField]
    [Tooltip("The rate the voice speaks for weather")]
	private float voiceRate;
	[SerializeField]
	[Tooltip("The volume the voice speaks for weather")]
	private float voiceVolume;
	[SerializeField]
	[Tooltip("The pitch the voice speaks for weather")]
	private float voicePitch;

	[SerializeField]
	[Tooltip("How many time the weather will repeat before updating")]
	private float weatherUpdateThreshold;
	[SerializeField]
	[Tooltip("How long the weather needs to update the file")]
	private float weatherUpdateTime;

	/// <summary>
	/// Sets up radio for usage -- turned off, with no active channel
	/// </summary>
	void Start()
	{
		if (Game.Instance.IsRadioActive)
		{
			// assign instance to this one
			Game.Instance.RadioInstance = this;
		}

		// Update the weather and play if radio on
		StartCoroutine(updateWeather());

		isOn = false;
		staticOverlayOn = false;

		CurrentChannel = RadioChannel.Null;

		// Load in default sounds for all other channels
		musicCarousel = new List<string> ();
		mysteryCarousel = new List<string> ();

		musicCarousel.Add ("event:/Radio/Music/Zero_Rain");
		mysteryCarousel.Add(StaticDefaultPath);

		mysteryChannel = FMODUnity.RuntimeManager.CreateInstance (mysteryCarousel[0]);
		musicChannel = FMODUnity.RuntimeManager.CreateInstance (musicCarousel[0]);
		staticChannel = FMODUnity.RuntimeManager.CreateInstance (StaticDefaultPath);
		staticOverlay = FMODUnity.RuntimeManager.CreateInstance (StaticDefaultPath);
		staticOverlay.setVolume(staticOverlayVolume);

		// save clip lengths for looping later
		musicCarouselClipLengths = new Dictionary<string, int>();
		FMOD.Studio.EventDescription e;
		int clipLength;
		musicChannel.getDescription (out e);
		e.getLength (out clipLength);
		musicCarouselClipLengths.Add(musicCarousel[0], clipLength);
		musicClipsTotalLength += clipLength;

		mysteryCarouselClipLengths = new Dictionary<string, int>();
		mysteryChannel.getDescription (out e);
		e.getLength (out clipLength);
		mysteryCarouselClipLengths.Add(mysteryCarousel[0], clipLength);
		mysteryClipsTotalLength += clipLength;
	
		AddToCarousel (RadioChannel.Music, "event:/Radio/Music/Apocalypse");
		AddToCarousel (RadioChannel.Music, "event:/Radio/Music/Better_Safe");

		Game.Instance.EventManager.StormStartedSubscription += startStatic;
		Game.Instance.EventManager.StormStoppedSubscription += stopStatic;
		Game.Instance.EventManager.PlayerBoardRaftSubscription += addRaftClip;
		Game.Instance.ClockInstance.HourUpdate += addNextMysteryClip;

		// initialize with first two mystery clips
		setUpMysteryClips();
		addNextMysteryClip();
		addNextMysteryClip();

	}

	/// <summary>
	/// Sets up current mystery channel index and lists of mystery channel clips based on phases of story intensity.
	/// </summary>
	private void setUpMysteryClips()
	{
		mysteryPhaseIndex = 0;
		mysteryPhaseClips = new List<List<string>>();
		List<string> phase = new List<string>();

		phase.Add("event:/Radio/Mystery/Phase1/School_Musical");
		phase.Add("event:/Radio/Mystery/Phase1/Jeff");
		phase.Add("event:/Radio/Mystery/Phase1/Evacuate_3_Block");

		mysteryPhaseClips.Add(phase);
		phase = new List<string>();

		phase.Add("event:/Radio/Mystery/Phase2/4_Officials_Missing");
		phase.Add("event:/Radio/Mystery/Phase2/Flooding_Centuries");
		phase.Add("event:/Radio/Mystery/Phase2/Hospital_Generators");

		mysteryPhaseClips.Add(phase);
		phase = new List<string>();

		phase.Add("event:/Radio/Mystery/Phase3/Bodies_Found");
		phase.Add("event:/Radio/Mystery/Phase3/District_5_6_Rations");
		phase.Add("event:/Radio/Mystery/Phase3/Relgious_Fanatic");

		mysteryPhaseClips.Add(phase);
	}

	/// <summary>
	/// Finds the next mystery clip and puts it in the mystery channel carousel
	/// Also checks to update phase to next phase and to unsubscribe from clock updates if we're out of new clips
	/// </summary>
	private void addNextMysteryClip()
	{
		int randIndex = Random.Range(0, mysteryPhaseClips[mysteryPhaseIndex].Count-1);
		string nextClip = mysteryPhaseClips[mysteryPhaseIndex][randIndex];
		mysteryPhaseClips[mysteryPhaseIndex].RemoveAt(randIndex);

		// check to update phase
		if (mysteryPhaseClips[mysteryPhaseIndex].Count == 0)
		{
			mysteryPhaseIndex += 1;

			if (mysteryPhaseIndex >= mysteryPhaseClips.Count){

				// unsubscribe if we're out of clips
				Game.Instance.ClockInstance.HourUpdate -= addNextMysteryClip;
			}
		}

		AddToCarousel (RadioChannel.Mystery, nextClip);
	}

	/// <summary>
	/// Updates music if on.
	/// </summary>
	void Update()
	{
		if (isOn) 
		{
			// Find the current channel and turn it on if it's not already playing 
			FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.STOPPED;

			// get the seconds passed so far
			int millisecondsPassed = (int) (Time.time * millisecond);

			if (CurrentChannel == RadioChannel.Mystery) 
			{
				mysteryChannel.getPlaybackState (out state);

				// check that the current state isn't playing or starting so we don't double up on sounds
				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING && state != FMOD.Studio.PLAYBACK_STATE.STARTING)  
				{
					// figure out which clip we should be on (and at what time) based on how much time has passed
					// use modulus to reduce seconds passed so far down to within the length of the total clips for this channel
					int timeLeft = millisecondsPassed % mysteryClipsTotalLength;

					// iterate over each clip and find the one should play on based on how much time has passed
					for (int i = 0; i < mysteryCarousel.Count; ++i) 
					{
						int currentClipLength = 0;

						mysteryCarouselClipLengths.TryGetValue (mysteryCarousel [i], out currentClipLength);

						// if this the time left is >= this clip's length, reduce time left by the clip length
						if (timeLeft >= currentClipLength) 
						{
							timeLeft -= currentClipLength;
						} 
						// otherwise, this is the clip we want. Keep the time left and set the musicCounter to i
						else 
						{
							mysteryCounter = i;
							break;
						}
					}

					// set the clip and the time left that we found earlier
					mysteryChannel = FMODUnity.RuntimeManager.CreateInstance (mysteryCarousel [mysteryCounter]);
					mysteryChannel.setTimelinePosition (timeLeft);
					mysteryChannel.start ();
				}
			}

			else if (CurrentChannel == RadioChannel.Music) 
			{
				musicChannel.getPlaybackState (out state);

				// check that the current state isn't playing or starting so we don't double up on sounds
				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING && state != FMOD.Studio.PLAYBACK_STATE.STARTING)
				{
					// figure out which clip we should be on (and at what time) based on how much time has passed
					// use modulus to reduce seconds passed so far down to within the length of the total clips for this channel
					int timeLeft = millisecondsPassed % musicClipsTotalLength;

					// iterate over each clip and find the one should play on based on how much time has passed
					for (int i = 0; i < musicCarousel.Count; ++i) 
					{
						int currentClipLength = 0;

						musicCarouselClipLengths.TryGetValue (musicCarousel [i], out currentClipLength);

						// if this the time left is >= this clip's length, reduce time left by the clip length
						if (timeLeft >= currentClipLength) 
						{
							timeLeft -= currentClipLength;
						} 
						// otherwise, this is the clip we want. Keep the time left and set the musicCounter to i
						else 
						{
							musicCounter = i;
							break;
						}
					}

					// set the clip and the time left that we found earlier
					musicChannel = FMODUnity.RuntimeManager.CreateInstance (musicCarousel [musicCounter]);
					musicChannel.setTimelinePosition (timeLeft);

					musicChannel.start ();
				}
			}

			else if (CurrentChannel == RadioChannel.Null) 
			{
				staticChannel.getPlaybackState (out state);

				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
				{
					staticChannel.start ();
				}
			}

			else if (CurrentChannel == RadioChannel.Weather && !weather.isPlaying) 
			{
				weather.Play ();
			}

			// if static overlay is active, play it if it's not already on, otherwise stop it
			staticOverlay.getPlaybackState (out state);

			if (staticOverlayOn && state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
			{
				staticOverlay.start();
			} 

			else if (state != FMOD.Studio.PLAYBACK_STATE.STOPPED)
			{
				staticOverlay.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}

			ChangeChannel(dial.knobDegree);

		} 
	}

	/// <summary>
	/// Turns radio on and off.
	/// </summary>
	public void Power()
	{
		// Turn off the radio and all channels if it's currently on
		if (isOn)
		{
			isOn = false;
			CurrentChannel = RadioChannel.Null;

			musicChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			mysteryChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			staticChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			staticOverlay.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			weather.Stop ();
		}

		// Turn on the radio if it's currently off
		else
		{
			isOn = true;
		}
	}

	/// <summary>
	/// Adds sound event to carousel.
	/// </summary>
	/// <param name="channel">Channel.</param>
	/// <param name="soundEvent">Sound event to be added.</param>
	public void AddToCarousel(RadioChannel channel, string soundEvent)
	{
		if (channel == RadioChannel.Music) 
		{
			musicCarousel.Add (soundEvent);
			FMOD.Studio.EventInstance eventInstance;
			FMOD.Studio.EventDescription eventDescription;
			int clipLength = 0;

			eventInstance = FMODUnity.RuntimeManager.CreateInstance (soundEvent);
			eventInstance.getDescription (out eventDescription);
			eventDescription.getLength (out clipLength);
			musicCarouselClipLengths.Add(soundEvent, clipLength);
			musicClipsTotalLength += clipLength;
		}

		else if (channel == RadioChannel.Mystery) 
		{
			mysteryCarousel.Add (soundEvent);
			FMOD.Studio.EventInstance eventInstance;
			FMOD.Studio.EventDescription eventDescription;
			int clipLength = 0;

			eventInstance = FMODUnity.RuntimeManager.CreateInstance (soundEvent);
			eventInstance.getDescription (out eventDescription);
			eventDescription.getLength (out clipLength);
			mysteryCarouselClipLengths.Add(soundEvent, clipLength);
			mysteryClipsTotalLength += clipLength;
		}
	}

	/// <summary>
	/// Updates the weather, creates a new announcement, and then plays the announcement.
	/// </summary>
	IEnumerator updateWeather()
	{
        while (true)
        {
            if (isOn)
            {
                // Check if the weather is not playing and if the radio is on the weather channel
                if (!weather.isPlaying && CurrentChannel == RadioChannel.Weather)
                {
                    // Play the weather 3 times before restarting to 0 
                    if (weatherCounter == weatherUpdateThreshold)
                    {
                        weatherCounter = 0;
                    }

                    // Update the weather when it has played 3 times or has just turned on
                    if (weatherCounter == 0)
                    {
                        // Update weather
                        currentWeather = Game.Instance.WeatherInstance;

                        // Get new announcment
                        announcement = GetWeatherAnnouncement(currentWeather.WeatherInformation[(int)Weather.WindSpeedMagnitude], currentWeather.WeatherInformation[(int)Weather.Temperature]);

                        // Send the announcement to the wav file
                        Speaker.Speak(announcement, weather, Speaker.VoiceForCulture("en", 0), false, voiceRate, voiceVolume, Application.dataPath + "/Sounds/Weather", voicePitch);

                        // Wait for the wave file to update
                        yield return new WaitForSeconds(weatherUpdateTime);
                    }

                    ++weatherCounter;
                    weather.Play();
                }

                // Don't do anything if the weather is still playing
                else
                {
                    yield return null;
                }
            }

            // Don't do anything if radio is not on
            else
            {
                yield return null;
            }
        }
    }

	/// <summary>
	/// Set new selected channel.
	/// </summary>
	/// <param name="channel"></param>
	public void SetChannel(RadioChannel channel)
	{
		if (isOn) 
		{
			if (channel == RadioChannel.Music) 
			{
				weather.Stop ();
				mysteryChannel.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				staticChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			} 
			else if (channel == RadioChannel.Weather) 
			{
				musicChannel.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				mysteryChannel.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				staticChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			} 
			else if (channel == RadioChannel.Mystery) 
			{
				musicChannel.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				staticChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
				weather.Stop ();
			}
			else if (channel == RadioChannel.Null)
			{
				musicChannel.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				mysteryChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
				weather.Stop ();
			}

			if (this.CurrentChannel == RadioChannel.Music)
			{

				if (channel == RadioChannel.Music) 
				{
					Game.Instance.EventManager.RadioMusicTurnedOn ();
				} 
				else 
				{
					Game.Instance.EventManager.RadioMusicTurnedOff ();
				}


			}

			this.CurrentChannel = channel;
		}
	}

	/// <summary>
	/// Change the radio channel based on dial position.
	/// </summary>
	/// <param name="knobRotation"></param>
	public void ChangeChannel(float knobRotation)
	{
		if (knobRotation > lowMusic && knobRotation < highMusic)
		{
			SetChannel(RadioChannel.Music);
		}
		else if (knobRotation > lowWeather && knobRotation < highWeather)
		{
			SetChannel(RadioChannel.Weather);
		}
		else if (knobRotation > lowMystery && knobRotation < highMystery)
		{
			SetChannel(RadioChannel.Mystery);
		}
		else
		{
			SetChannel(RadioChannel.Null);
		}
	}

	/// <summary>
	/// Flip through the channels.
	/// </summary>
	public void OnChannelClick()
	{        
		if (CurrentChannel == RadioChannel.Null) 
		{
			SetChannel (RadioChannel.Music);
		}
		else if (CurrentChannel == RadioChannel.Music) 
		{
			SetChannel (RadioChannel.Weather);
		} 
		else if (CurrentChannel == RadioChannel.Weather) 
		{
			SetChannel (RadioChannel.Mystery);
		} 
		else if (CurrentChannel == RadioChannel.Mystery) 
		{
			SetChannel (RadioChannel.Music);
		}
	}

	/// <summary>
	/// Creates string based on windSpeed and temperature (and eventually amount of rainfall)
	/// </summary>
	/// <param name="windSpeed"></param>
	/// <param name="temperature"></param>
	/// <returns></returns>
	public string GetWeatherAnnouncement(float windSpeed, float temperature) //this will be part of updating the weather; weather info taken in as struct
	{
		//round the floats to 2 decimal places
		string windSpeedText = windSpeed.ToString("F2");
		string temperatureText = temperature.ToString("F2");

		string newAnnouncement =  "There is heavy rain heading toward the city with a wind speed of " + windSpeedText + " miles per hour and a temperature of " + temperatureText + " degrees Fahrenheit.";
		return newAnnouncement;
	}

	/// <summary>
	/// Gets the mystery announcement.
	/// </summary>
	public string GetMysteryAnnouncement() 
	{
		string newAnnouncement = "This is a mystery";
		return newAnnouncement;
	}

	private void startStatic()
	{
		staticOverlayOn = true;
		musicChannel.setVolume(lowMusic);
	}

	private void stopStatic()
	{
		staticOverlayOn = false;
		musicChannel.setVolume(musicRegVol);
	}

	private void addRaftClip()
	{
		// TODO: Add raft clip to carousel
	}

	/// <summary>
	/// Increases the volume.
	/// </summary>
	public void IncreaseVolume()
	{
		if (!MaxVolume)
		{
			weather.volume += volumeIncrease;
			musicChannel.setVolume(weather.volume);
			mysteryChannel.setVolume(weather.volume);
			staticChannel.setVolume(weather.volume);
		}
	}

	/// <summary>
	/// Decreases the volume.
	/// </summary>
	public void DecreaseVolume()
	{
		if (!MinVolume)
		{
			weather.volume -= volumeDecrease;
			musicChannel.setVolume(weather.volume);
			mysteryChannel.setVolume(weather.volume);
			staticChannel.setVolume(weather.volume);
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Radio"/> max volume.
	/// </summary>
	/// <value><c>true</c> if max volume; otherwise, <c>false</c>.</value>
	public bool MaxVolume 
	{
		get
		{
			return weather.volume == highestVolume;
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Radio"/> minimum volume.
	/// </summary>
	/// <value><c>true</c> if minimum volume; otherwise, <c>false</c>.</value>
	public bool MinVolume 
	{
		get
		{
            return weather.volume == lowestVolume;
		}
	}
}