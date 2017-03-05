using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Crosstales.RTVoice;

public class AnnouncementFactory
{
	public List<MysteryAnnouncement> AnnouncementDatabase;
	public List<MysteryAnnouncement> LocationDatabase;
	public List<MysteryAnnouncement> WeatherDatabase;
	public List<MysteryAnnouncement> TimeDatabase;
	public List<MysteryAnnouncement> PlayerDatabse;

	private AnnouncementYAMLParser announcementParser;

	[SerializeField]
	private const string mysteryTTSYAMLPath = "MysteryChannelYAML.yml";

	[SerializeField]
	private AudioSource myAudio;
		
	public AnnouncementFactory ()
	{
		announcementParser = new AnnouncementYAMLParser (mysteryTTSYAMLPath);
	
		AnnouncementDatabase = new List<MysteryAnnouncement> ();
		LocationDatabase = new List<MysteryAnnouncement> ();
		WeatherDatabase = new List<MysteryAnnouncement> ();
		TimeDatabase = new List<MysteryAnnouncement> ();
		PlayerDatabse = new List<MysteryAnnouncement> ();

		// This is really just a dummy object that just allows me to instantiate an audio source.
		// I need to instantiate an audio source to prevent the announcements from playing immediately.
		// see: https://forum.unity3d.com/threads/rt-voice-run-time-text-to-speech-solution.340046/page-7#post-2982267
		GameObject o = new GameObject ();
		myAudio = o.AddComponent<AudioSource> ();

		loadAnnouncements ();
	}

	private void loadAnnouncements()
	{
		// Load the announcement data
		AnnouncementDatabase = announcementParser.LoadAnnouncements ();

		// Turn them into clips
		for (int i = 0; i < AnnouncementDatabase.Count; ++i) 
		{
			// Pipe the announcement to a new wav file
			Speaker.Speak(AnnouncementDatabase[i].Text, myAudio, null, false, .9f, 1, Application.dataPath + "/Sounds/Announcements/" + AnnouncementDatabase[i].EventPath, .7f);

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

