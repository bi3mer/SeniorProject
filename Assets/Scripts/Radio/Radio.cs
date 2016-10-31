using UnityEngine;
using System.Diagnostics;
using System.Collections;

public enum RadioChannel {Music, Weather, Mystery, Null};

//mock weather struct
public struct Weather
{
    public float windSpeed; 
    public float temperature;
}

public class Radio : MonoBehaviour
{
    public bool isOn;
    public RadioChannel CurrentChannel { get; private set; }
    public string announcement;
    public string appLocation;

    //declare sounds
    private AudioSource[] sounds;
    private AudioSource music;
    private AudioSource weather;
    private AudioSource warning;


    //mock weather
    private Weather currentWeather;


    void Start()
    {
        isOn = false;
        CurrentChannel = RadioChannel.Null;

        //set location for SpeechProgram
        appLocation = Application.dataPath + "/TextToSpeech/SpeechProgram.exe";

        //set sounds - this is temp until audio engine figured out
        sounds = GetComponents<AudioSource>();
        music = sounds[0];
        weather = sounds[1];
        warning = sounds[2];

        StartCoroutine(updateWeather());
    }
	

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
        if (isOn)
        {
            isOn = false;
            CurrentChannel = RadioChannel.Null;
            music.Stop();
            weather.Stop();
        }

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
            if (CurrentChannel == RadioChannel.Weather && !weather.isPlaying)
            {                
                //get struct from weather system
                Weather newWeather = GetWeather();

                //don't update clip if same
                if (currentWeather.windSpeed != newWeather.windSpeed && currentWeather.temperature != newWeather.temperature)
                {
                    currentWeather = newWeather;
                    CreateWeatherAnnouncement(currentWeather.windSpeed, currentWeather.temperature);
                    MakeWaveFile(Application.dataPath + "/Sounds/Weather.wav", announcement);

                    //wait so the new updated clip can load
                    yield return new WaitForSeconds(1f);
                    weather.Play();
                }
                else
                {
                    weather.Play();
                    yield return null;
                }
            }
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
    public void CreateWeatherAnnouncement(float windSpeed, float temperature) //this will be part of updating the weather; weather info taken in as struct
    {
        //round the floats to 2 decimal places
        string windSpeedText = windSpeed.ToString("F2");
        string temperatureText = temperature.ToString("F2");

        announcement = "There is heavy rain heading toward the city with a wind speed of " + windSpeedText + " miles per hour and a temperature of " + temperatureText + " degrees Fahrenheit.";
    }


    /// <summary>
    /// This code is adapted from the Easy Voice the Unity plugin by Game Loop. 
    /// For now it speaks the text.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="text"></param>
    /// <param name="action"></param>
    public void SpeakText(string text)
    {
        //create process to use SpeechProgram
        Process convertText = new Process();

        //set path to SpeechProgram and arguments
        convertText.StartInfo.FileName = appLocation;
        convertText.StartInfo.Arguments = "\"" + null + "\"" + " \"" + text + "\"" + " \"" + "speakText" + "\"";

        //don't create the program window when run
        convertText.StartInfo.CreateNoWindow = true;

        //don't use shell to execute program
        convertText.StartInfo.UseShellExecute = false;

        convertText.Start();
        convertText.Close();

    }



    /// <summary>
    /// This code is adapted from the Easy Voice the Unity plugin by Game Loop. 
    /// Creates the wave file with the text to speech.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="text"></param>
    public void MakeWaveFile(string filename, string text)
    {
        //create process to use SpeechProgram
        Process convertText = new Process();

        //set path to SpeechProgram and arguments
        convertText.StartInfo.FileName = appLocation;
        convertText.StartInfo.Arguments = "\"" + filename + "\"" + " \"" + text + "\"" + " \"" + "makeFile" + "\"";

        //run shell to execute program - clip doesn't update if shell isn't executed
        convertText.StartInfo.UseShellExecute = true;

        //minimize the shell window
        convertText.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

        convertText.Start();
        convertText.WaitForExit();
    }


    /// <summary>
    /// Mock weather getter.
    /// </summary>
    /// <returns></returns>
    public Weather GetWeather()
    {
        Weather currentWeather;

        //set up mock weather struct
        currentWeather.temperature = Random.Range(0.0f, 50f);
        currentWeather.windSpeed = Random.Range(0.0f, 200f);

        return currentWeather;
    }
}


