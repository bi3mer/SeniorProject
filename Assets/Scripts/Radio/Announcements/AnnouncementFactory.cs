using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Crosstales.RTVoice;

public class AnnouncementFactory
{
	public List<MysteryAnnouncement> AnnouncementDatabase;
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
			a.mute;
		}

	}
}

