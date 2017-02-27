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

	private const string mysteryTTSYAMLPath = "MysteryChannelYAML.yml";
		
	public AnnouncementFactory ()
	{
		announcementParser = new AnnouncementYAMLParser (mysteryTTSYAMLPath);

		AnnouncementDatabase = new List<MysteryAnnouncement> ();
		LocationDatabase = new List<MysteryAnnouncement> ();
		WeatherDatabase = new List<MysteryAnnouncement> ();
		TimeDatabase = new List<MysteryAnnouncement> ();
		PlayerDatabse = new List<MysteryAnnouncement> ();

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

		//	Speaker.Speak (AnnouncementDatabase[i].Text, null,null, false, 0.9f, 0, Application.dataPath + "/Sounds/Announcements/" + AnnouncementDatabase[i].EventPath, .9f);
			Debug.Log (Application.dataPath + "/Sounds/Announcements/" + AnnouncementDatabase [i].EventPath);

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

