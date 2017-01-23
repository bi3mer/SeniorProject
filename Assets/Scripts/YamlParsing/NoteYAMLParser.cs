using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using UnityEngine;

/// <summary>
// Takes list of notes from yaml file and loads them into Note objects.
/// </summary>
public class NoteYAMLParser 
{
    private string fileName;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public NoteYAMLParser (string file) 
	{
		fileName = file;
	}

	/// <summary>
	/// Loads the notes from the yaml file into list
	/// </summary>
	/// <returns>The notes in list of Note objects.</returns>
	public List<NoteData> LoadNotes()
	{
		// read in the yaml file and deserialize the input
		string file = FileManager.GetDocument(fileName);
		StringReader input = new StringReader(file);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		// create a list of notes based on the yaml info and return them
		List<NoteData> noteYamlInfo = deserializer.Deserialize<List<NoteData>>(input);

		return noteYamlInfo;
	}
}
