using UnityEngine;
using System.Diagnostics;
using System.Collections;

public enum RadioChannel {Music, WeatherInfo, Mystery, Null};

//mock weather struct
public struct WeatherInfo
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
    private WeatherInfo currentWeatherInfo;


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

        StartCoroutine(updateWeatherInfo());
    }
	

    void Update()
    {
        if (isOn)
        {
            //when music channel selected
            if (!music.isPlaying && this.CurrentChannel == RadioChannel.Music)
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
    IEnumerator updateWeatherInfo()
    {
        while (true)
        {
            if (CurrentChannel == RadioChannel.WeatherInfo && !weather.isPlaying)
            {                
                //get struct from weather system
                WeatherInfo newWeatherInfo = GetWeatherInfo();

                //don't update clip if same
                if (currentWeatherInfo.windSpeed != newWeatherInfo.windSpeed && currentWeatherInfo.temperature != newWeatherInfo.temperature)
                {
                    currentWeatherInfo = newWeatherInfo;
                    CreateWeatherInfoAnnouncement(currentWeatherInfo.windSpeed, currentWeatherInfo.temperature);
                    MakeWaveFile(Application.dataPath + "/Sounds/WeatherInfo.wav", announcement);

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
            if (CurrentChannel == RadioChannel.WeatherInfo)
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
    public void CreateWeatherInfoAnnouncement(float windSpeed, float temperature) //this will be part of updating the weather; weather info taken in as struct
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
    public WeatherInfo GetWeatherInfo()
    {
        WeatherInfo currentWeatherInfo;

        //set up mock weather struct
        currentWeatherInfo.temperature = Random.Range(0.0f, 50f);
        currentWeatherInfo.windSpeed = Random.Range(0.0f, 200f);

        return currentWeatherInfo;
    }

	/// <summary>
	/// Flip through the channels.
	/// </summary>
	public void OnChannelClick()
	{
		if (this.CurrentChannel == RadioChannel.Music) 
		{
			this.SetChannel (RadioChannel.WeatherInfo);
		} 
		else if (this.CurrentChannel == RadioChannel.WeatherInfo) 
		{
			this.SetChannel (RadioChannel.Mystery);
		} 
		else if (this.CurrentChannel == RadioChannel.Mystery) 
		{
			this.SetChannel (RadioChannel.Music);
		}
	}

	/// <summary>
	/// Raises the volume up event.
	/// </summary>
	public void OnVolumeUpClick()
	{
		// will update once Fmod is setup
	}

	/// <summary>
	/// Raises the volume down event.
	/// </summary>
	public void OnVolumeDownClick()
	{
		// will update once Fmod is setup
	}
}


