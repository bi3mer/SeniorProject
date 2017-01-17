using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Crosstales.RTVoice;

//radio stations
public enum RadioChannel {Music, Weather, Mystery, Null};

public class Radio : MonoBehaviour
{
	// set sound sources
	[SerializeField]
	private AudioSource music;
	[SerializeField]
	private AudioSource weather;
	[SerializeField]
	private AudioSource mystery;
    [SerializeField]
    private AudioSource statics;
    [SerializeField]
	private Text radioScreenText;
    [SerializeField]
    private Dial dial;

    private bool isOn;
    private RadioChannel CurrentChannel { get; set; }
    private string announcement;

    //set up weather
    public WeatherSystem currentWeather;

    //counter for how many times weather played
    private int weatherCounter;

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
    /// Sets up radio for usage.
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

        //update the weather and play if radio on
        StartCoroutine(updateWeather());
    }

    /// <summary>
    /// Updates music if on.
    /// </summary>
    void Update()
    {
		if (isOn) 
		{
			radioScreenText.text = "Radio is On";
			// When music channel selected
			if (!music.isPlaying && CurrentChannel == RadioChannel.Music) 
			{
				music.Play ();
			}
			if (!mystery.isPlaying && CurrentChannel == RadioChannel.Mystery) 
			{
				mystery.Play ();
			}
			if (!weather.isPlaying && CurrentChannel == RadioChannel.Weather) 
			{
				weather.Play ();
			}
            if (!statics.isPlaying && CurrentChannel == RadioChannel.Null)
            {
                statics.Play();
            }

			ChangeChannel(dial.DialSlider.value);
		} 
		else 
		{
			radioScreenText.text = "Radio is Off";
		}
    }

    /// <summary>
    /// Turns radio on and off.
    /// </summary>
    public void Power()
    {
        //Turn off the radio and all channels
        if (isOn)
        {
            isOn = false;
            CurrentChannel = RadioChannel.Null;
            music.Stop();
            weather.Stop();
			mystery.Stop();
            statics.Stop();
        }

        //Turn on the radio
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
                //check if the weather is not playing and if the radio is on the weather channel
                if (!weather.isPlaying && CurrentChannel == RadioChannel.Weather)
                {
                    //Play the weather 3 times before restarting to 0 
                    if (weatherCounter == 3)
                    {
                        weatherCounter = 0;
                    }

                    //update the weather when it has played 3 times or has just turned on
                    if (weatherCounter == 0)
                    {
                        //update weather
                        currentWeather = Game.Instance.WeatherInstance;

                        //get new announcment
                        announcement = GetWeatherAnnouncement(currentWeather.WeatherInformation[(int)Weather.WindSpeedMagnitude], currentWeather.WeatherInformation[(int)Weather.Temperature]);
                        Debug.Log(announcement);
                        //send the announcement to the wave file
                        Speaker.Speak(announcement, weather, Speaker.VoiceForCulture("en", 0), false, .9f, 1, Application.dataPath + "/Sounds/Weather", .7f);

                        //wait for the wave file to update
                        yield return new WaitForSeconds(2);
                    }

                    ++weatherCounter;
                    weather.Play();
                }

                //don't do anything if the weather is still playing
                else
                {
                    yield return null;
                }
            }

            //don't do anything if radio is not on
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
				mystery.Stop ();
                statics.Stop();
				this.CurrentChannel = channel;
			} 
			else if (channel == RadioChannel.Weather) 
			{
				music.Stop ();
				mystery.Stop ();
                statics.Stop();
                this.CurrentChannel = channel;
			} 
			else if (channel == RadioChannel.Mystery) 
			{
				music.Stop ();
				weather.Stop ();
                statics.Stop();
                this.CurrentChannel = channel;
			}
            else if (channel == RadioChannel.Null)
            {
                music.Stop();
                weather.Stop();
                this.CurrentChannel = channel;
            }
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
}


