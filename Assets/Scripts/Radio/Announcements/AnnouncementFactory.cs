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

	private const string mysteryTTSYAMLPath = "MysteryChannelTTSYAML.yml";
		
	public AnnouncementFactory ()
	{
		announcementParser = new AnnouncementYAMLParser (mysteryTTSYAMLPath);
		AnnouncementDatabase = new List<MysteryAnnouncement> ();
		loadAnnouncements ();
	}

	private void loadAnnouncements()
	{
		// Load the announcement data
		AnnouncementDatabase = announcementParser.LoadAnnouncements ();

		// turn them into clips
		for (int i = 0; i < AnnouncementDatabase.Count; ++i) 
		{
			AudioSource a = new AudioSource ();

			Speaker.Speak (AnnouncementDatabase[i].Text, a, null, false, 0.01f, 0, Application.dataPath + "/Sounds/Announcements/" + AnnouncementDatabase[i].EventPath, .01f);
			Debug.Log (Application.dataPath + "/Sounds/Announcements/" + AnnouncementDatabase [i].EventPath);
			//a.volume = 0.0f;

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

