using UnityEngine;
using System.Diagnostics;

public enum RadioChannel {Music, Weather, Mystery};

public class Radio : MonoBehaviour {
    public bool isOn;
    public RadioChannel CurrentChannel { get; set; }
    private string announcement;
    private string appLocation;

    //declare sounds
    public AudioSource[] sounds;
    private AudioSource music;
    private AudioSource weather;
    private AudioSource warning;


    void Start()
    {
        isOn = false;

        //set location for SpeechProgram
        appLocation = Application.dataPath + @"/Scripts/Radio/SpeechProgram.exe";

        //set sounds - this is temp until audi engine figured out
        sounds = GetComponents<AudioSource>();
        music = sounds[0];
	}
	

    void Update()
    {
        if (isOn)
        {
            if (CurrentChannel == RadioChannel.Weather)
            {
                updateWeather();
            }

            else if (CurrentChannel == RadioChannel.Music)
            {
                updateMusic();
            }
        }
    }

    /// <summary>
    /// Updates the weather and creates a new announcement.
    /// </summary>
    private void updateWeather()
    {
        //get struct from weather system

        //numbers will change based on struct 
        announcement = CreateWeatherAnnouncement(57.1f, 45.2f);
        MakeWaveFile(Application.dataPath + @"/Sounds/Weather.wav", announcement);
    }


    /// <summary>
    /// Loops through the music.
    /// </summary>
    private void updateMusic()
    {
        music.Play();
    }


    /// <summary>
    /// Creates string based on windSpeed and temperature (and eventually amount of rainfall)
    /// </summary>
    /// <param name="windSpeed"></param>
    /// <param name="temperature"></param>
    /// <returns></returns>
    public string CreateWeatherAnnouncement(float windSpeed, float temperature) //this will be part of updating the weather; weather info taken in as struct
    {
        string windSpeedText = windSpeed.ToString();
        string temperatureText = temperature.ToString();

        string weatherAnnouncement = "There is heavy rain heading toward the city with a wind speed of " + windSpeedText + " miles per hour and a temperature of " + temperatureText + " degrees Fahrenheit.";

        return weatherAnnouncement;
    }


    /// <summary>
    /// Makes the speech file by opening an application "SpeechProgram" that was created outside
    /// of Unity so that the System.Speech dll is usable. This code is adapted from the Easy Voice
    /// Unity plugin by Game Loop. For now it speaks the text then makes a speech file.
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

        convertText.StartInfo.CreateNoWindow = true;
        convertText.StartInfo.RedirectStandardOutput = true;
        convertText.StartInfo.RedirectStandardError = true;
        convertText.StartInfo.UseShellExecute = false;

        convertText.Start();
        convertText.Close();
    }

    /// <summary>
    /// Creates the wave file with the text to speech
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

        convertText.StartInfo.CreateNoWindow = true;
        convertText.StartInfo.RedirectStandardOutput = true;
        convertText.StartInfo.RedirectStandardError = true;
        convertText.StartInfo.UseShellExecute = false;

        convertText.Start();
        convertText.Close();
    }

}
