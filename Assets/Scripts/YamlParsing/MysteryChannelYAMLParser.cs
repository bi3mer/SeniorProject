using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using UnityEngine;

public class MysteryChannelYAMLParser
{
	public string Filename;

	public MysteryChannelYAMLParser (string file)
	{
		Filename = file;
	}

	public List<MysteryAnnouncement> LoadAnnouncements()
	{
		// read in the yaml file and deserialize the input
		string file = FileManager.GetDocument(Filename);
		StringReader input = new StringReader(file);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		// create a list of announcements based on the yaml info and return them
		List<MysteryAnnouncement> announcementYAMLInfo = deserializer.Deserialize<List<MysteryAnnouncement>>(input);

		return announcementYAMLInfo;
	}
}
