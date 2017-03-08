using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using UnityEngine;

public class AnnouncementYAMLParser
{
	/// <summary>
	/// The TTS filename.
	/// </summary>
	[SerializeField]
	private string TTSFilename;

	/// <summary>
	/// Initializes a new instance of the <see cref="AnnouncementYAMLParser"/> class.
	/// </summary>
	/// <param name="ttsFile">Tts file.</param>
	public AnnouncementYAMLParser (string ttsFile)
	{
		TTSFilename = ttsFile;
	}

	/// <summary>
	/// Loads the announcements.
	/// </summary>
	/// <returns>The announcements.</returns>
	public List<MysteryAnnouncement> LoadAnnouncements()
	{
		// read in the yaml file and deserialize the input
		string file = FileManager.GetDocument(TTSFilename);
		StringReader input = new StringReader(file);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		// create a list of announcements based on the yaml info and return them
		List<MysteryAnnouncement> announcementYAMLInfo = deserializer.Deserialize<List<MysteryAnnouncement>>(input);

		return announcementYAMLInfo;
	}
}
