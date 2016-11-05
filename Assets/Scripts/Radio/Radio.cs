using UnityEngine;
using System.Diagnostics;
using System.Collections;
using Crosstales.RTVoice;

//radio stations
public enum RadioChannel {Music, Weather, Mystery, Null};

//mock weather struct
public struct MockWeather
{
    public float windSpeed; 
    public float temperature;
}

public class Radio : MonoBehaviour
{
    public bool isOn;
    public RadioChannel CurrentChannel { get; private set; }
    public string announcement;

    //counter for how many times weather played
    private int weatherCounter;

    //declare sounds
    private AudioSource[] sounds;
    private AudioSource music;
    private AudioSource weather;

    //mock weather
    private MockWeather currentWeather;

    /// <summary>
    /// Sets up radio for usage.
    /// </summary>
    void Start()
    {
        isOn = false;
        CurrentChannel = RadioChannel.Null;

        //set sounds - this is temp until audio engine figured out
        sounds = GetComponents<AudioSource>();
        music = sounds[0];
        weather = sounds[1];

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
            //when music channel selected
            if (!music.isPlaying && CurrentChannel == RadioChannel.Music)
            {
                music.Play();
            }
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
        }

        //Turn on the radio
        else
        {
            isOn = true;
        }
    }

    /// <summary>
    /// Updates the weather and creates a new announcement.
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
                        //get struct from weather system
                        MockWeather newWeather = GetWeather();

                        //don't update clip if same
                        if (currentWeather.windSpeed != newWeather.windSpeed && currentWeather.temperature != newWeather.temperature)
                        {
                            //update weather and get new announcment
                            currentWeather = newWeather;
                            GetWeatherAnnouncement(currentWeather.windSpeed, currentWeather.temperature);

                            //send the announcement to the wave file
                            Speaker.Speak(announcement, weather, Speaker.VoiceForCulture("en", 0), false, .9f, 1, Application.dataPath + "/Sounds/Weather.wav", .7f);

                            //wait for the wave file to update
                            yield return new WaitForSeconds(2);
                        }
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

            //don't do anything if radio not on
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
        if (CurrentChannel != channel)
        {        
            if (CurrentChannel == RadioChannel.Music)
            {
                music.Stop();
            }
            if (CurrentChannel == RadioChannel.Weather)
            {
                weather.Stop();
            }
            CurrentChannel = channel;
        }
    }

    /// <summary>
    /// Creates string based on windSpeed and temperature (and eventually amount of rainfall)
    /// </summary>
    /// <param name="windSpeed"></param>
    /// <param name="temperature"></param>
    /// <returns></returns>
    public void GetWeatherAnnouncement(float windSpeed, float temperature) //this will be part of updating the weather; weather info taken in as struct
    {
        //round the floats to 2 decimal places
        string windSpeedText = windSpeed.ToString("F2");
        string temperatureText = temperature.ToString("F2");

        announcement = "There is heavy rain heading toward the city with a wind speed of " + windSpeedText + " miles per hour and a temperature of " + temperatureText + " degrees Fahrenheit.";
    }

    /// <summary>
    /// Gets the mystery announcement.
    /// </summary>
    public void GetMysteryAnnouncement() 
    {
        
    }

    /// <summary>
    /// Mock weather getter.
    /// </summary>
    /// <returns></returns>
    public MockWeather GetWeather()
    {
        MockWeather currentWeather;

        //set up mock weather struct
        currentWeather.temperature = Random.Range(0.0f, 50f);
        currentWeather.windSpeed = Random.Range(0.0f, 200f);

        return currentWeather;
    }
}


