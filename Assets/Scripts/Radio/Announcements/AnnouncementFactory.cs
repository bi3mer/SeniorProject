using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Crosstales.RTVoice;
using System.IO;

public class AnnouncementFactory
{
	/// <summary>
	/// The full announcement database.
	/// </summary>
	public List<MysteryAnnouncement> AnnouncementDatabase;

	/// <summary>
	/// The database of announcements in the location category.
	/// </summary>
	public List<MysteryAnnouncement> LocationDatabase;

	/// <summary>
	/// The database of announcements in the weather category.
	/// </summary>
	public List<MysteryAnnouncement> WeatherDatabase;

	/// <summary>
	/// The database of announcements in the time category.
	/// </summary>
	public List<MysteryAnnouncement> TimeDatabase;

	/// <summary>
	/// The database of announcements in the player category.
	/// </summary>
	public List<MysteryAnnouncement> PlayerDatabse;

	/// <summary>
	/// The announcement parser.
	/// </summary>
	private AnnouncementYAMLParser announcementParser;

	/// <summary>
	/// The announcement folder path.
	/// </summary>
	[SerializeField]
	private const string announcementFolderPath = "/Sounds/Announcements/";

	/// <summary>
	/// The mystery TTSYAML file path.
	/// </summary>
	[SerializeField]
	private const string mysteryTTSYAMLPath = "MysteryChannelYAML.yml";

	/// <summary>
	/// The audio source for use in the tts file generation.
	/// This is really just a dummy object that just allows me to instantiate an audio source.
	/// I need to instantiate an audio source to prevent the announcements from playing immediately.
	/// see: https://forum.unity3d.com/threads/rt-voice-run-time-text-to-speech-solution.340046/page-7#post-2982267
	/// </summary>
	[SerializeField]
	private AudioSource loadingAudioSource;
		
	/// <summary>
	/// The rate.
	/// </summary>
	[SerializeField]
	private float rate = .9f;

	/// <summary>
	/// The volume.
	/// </summary>
	[SerializeField]
	private float volume = 1.0f;

	/// <summary>
	/// The pitch.
	/// </summary>
	[SerializeField]
	private float pitch = 0.7f;

	private const string announcementObjectName = "Announcenment Object";

	/// <summary>
	/// Initializes a new instance of the <see cref="AnnouncementFactory"/> class.
	/// </summary>
	public AnnouncementFactory ()
	{
		announcementParser = new AnnouncementYAMLParser (mysteryTTSYAMLPath);
	
		AnnouncementDatabase = new List<MysteryAnnouncement> ();
		LocationDatabase = new List<MysteryAnnouncement> ();
		WeatherDatabase = new List<MysteryAnnouncement> ();
		TimeDatabase = new List<MysteryAnnouncement> ();
		PlayerDatabse = new List<MysteryAnnouncement> ();
	}

	/// <summary>
	/// Loads the announcements.
	/// </summary>
	public void LoadAnnouncements()
	{
		// Load the announcement data
		AnnouncementDatabase = announcementParser.LoadAnnouncements ();

		GameObject obj = new GameObject ();
		obj.name = announcementObjectName;
		loadingAudioSource = obj.AddComponent<AudioSource> ();

		// Turn them into clips
		for (int i = 0; i < AnnouncementDatabase.Count; ++i) 
		{
			//TODO: allow rate, volume, and pitch to be set by YAML.
			// Pipe the announcement to a new wav file
			Speaker.Speak(AnnouncementDatabase[i].Text, loadingAudioSource, null, false, rate, volume, Application.dataPath + announcementFolderPath + AnnouncementDatabase[i].EventPath, pitch);

			// Add it to the correct database for use later
			if (AnnouncementDatabase [i].Category == AnnouncementCategory.Location) 
			{
				LocationDatabase.Add (AnnouncementDatabase [i]);
			} 
			else if (AnnouncementDatabase [i].Category == AnnouncementCategory.Weather) 
			{
				WeatherDatabase.Add (AnnouncementDatabase [i]);
			} 
			else if (AnnouncementDatabase [i].Category == AnnouncementCategory.Time) 
			{
				TimeDatabase.Add (AnnouncementDatabase [i]);
			} 
			else if (AnnouncementDatabase [i].Category == AnnouncementCategory.Player) 
			{
				PlayerDatabse.Add (AnnouncementDatabase [i]);
			}
		}
	}
}

