using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnnouncementFactory
{
	public List<MysteryAnnouncement> AnnouncementDatabase;

	private AnnouncementYAMLParser announcementParser;

	private const string mysteryPregenYAMLPath = "MysteryChannelPregenYAML.yml";

	private const string mysteryTTSYAMLPath = "MysteryChannelTTSYAML.yml";
		
	public AnnouncementFactory ()
	{
		announcementParser = new AnnouncementYAMLParser (mysteryPregenYAMLPath, mysteryTTSYAMLPath);
		AnnouncementDatabase = new List<MysteryAnnouncement> ();
	}


	public void LoadAnnouncements()
	{

		AnnouncementDatabase = announcementParser.LoadAnnouncements ();

	}
}

