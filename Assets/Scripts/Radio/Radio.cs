using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crosstales.RTVoice;

//radio stations
public enum RadioChannel {Music, Weather, Mystery, Null};

public class Radio : MonoBehaviour
{
	// CHANNELS
	[SerializeField]
	private FMOD.Studio.EventInstance musicChannel;

	[SerializeField]
	private FMOD.Studio.EventInstance mysteryChannel;

	// Static channel right now just plays when current active channel is null
    [SerializeField]
	private FMOD.Studio.EventInstance staticChannel;

	// Weather still has an audio source rather than an FMOD instance for text-to-speech purposes
	[SerializeField]
	private AudioSource weather;

	// Default channel sound event paths
	[SerializeField]
	public string MusicDefaultPath = "event:/Radio/Music/Music1";
	[SerializeField]
	public string MysteryDefaultPath = "event:/Radio/Mystery/Mystery1";
	[SerializeField]
	public string StaticDefaultPath = "event:/Radio/Static/Basic_Static"; 

	// Default display texts
	public string RadioOnText = "Radio is on";
	public string RadioOffText = "Radio is off";

	// Radio components
    [SerializeField]
	private Text radioScreenText;

    [SerializeField]
    private Dial dial;

	private bool isOn;
	private RadioChannel CurrentChannel { get; set; }
	private string announcement;

    // Counter for how many times weather played
	private int weatherCounter;
	private WeatherSystem currentWeather;

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

        isOn = false;

		CurrentChannel = RadioChannel.Null;

        // Update the weather and play if radio on
        StartCoroutine(updateWeather());

		// Load in default sounds for all other channels
		mysteryChannel = FMODUnity.RuntimeManager.CreateInstance (MysteryDefaultPath);
		musicChannel = FMODUnity.RuntimeManager.CreateInstance (MusicDefaultPath);
		staticChannel = FMODUnity.RuntimeManager.CreateInstance (StaticDefaultPath);

		Game.Instance.EventManager.StormStartedSubscription += startStatic;
		Game.Instance.EventManager.StormStoppedSubscription += stopStatic;
		Game.Instance.EventManager.PlayerBoardRaftSubscription += addRaftClip;
    }

    /// <summary>
    /// Updates music if on.
    /// </summary>
    void Update()
    {
		if (isOn) 
		{
			radioScreenText.text = RadioOnText;

			// Find the current channel and turn it on if it's not already playing 
			FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.STOPPED;

			if (CurrentChannel == RadioChannel.Mystery) 
			{
				mysteryChannel.getPlaybackState (out state);

				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
				{
					mysteryChannel.start ();
				}
			}

			else if (CurrentChannel == RadioChannel.Music) 
			{
				musicChannel.getPlaybackState (out state);

				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
				{
					musicChannel.start ();
				}
			}

			else if (CurrentChannel == RadioChannel.Weather && !weather.isPlaying) 
			{
				weather.Play ();
			}

			else if (CurrentChannel == RadioChannel.Null) 
			{
				staticChannel.getPlaybackState (out state);

				if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
				{
					staticChannel.start ();
				}
			}
				
			ChangeChannel(dial.DialSlider.value);
		} 
		else 
		{
			radioScreenText.text = RadioOffText;
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
			weather.Stop ();
			mysteryChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			staticChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        // Turn on the radio if it's currently off
        else
        {
            isOn = true;
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
                    if (weatherCounter == 3)
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
                        Debug.Log(announcement);

                        // Send the announcement to the wav file
                        Speaker.Speak(announcement, weather, Speaker.VoiceForCulture("en", 0), false, .9f, 1, Application.dataPath + "/Sounds/Weather", .7f);

                        // Wait for the wave file to update
                        yield return new WaitForSeconds(2);
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
				weather.Stop ();
				staticChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else if (channel == RadioChannel.Null)
			{
				musicChannel.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				weather.Stop ();
				mysteryChannel.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
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
		// TODO: Start some static here because a storm's abrewin'
	}

	private void stopStatic()
	{
		// TODO: Remove that static because we have clear skies
	}

	private void addRaftClip()
	{
		// TODO: Add raft clip to carousel
	}
}


