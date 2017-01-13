using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using UnityEngine;


// Takes list of notes from yaml file and loads them into Note objects.
public class NoteYAMLParser 
{
    private string filename = "";

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public NoteYAMLParser (string file) 
	{
		filename = file;
	}

	/// <summary>
	/// Loads the notes from the yaml file into list
	/// </summary>
	/// <returns>The notes in list of Note objects.</returns>
	public List<Note> LoadNotes()
	{
		// read in the yaml file and deserialize the input
		string fileyaml = UnityEngine.Application.dataPath + "/Resources/YAMLFiles/" + filename;
		string noteList = File.ReadAllText (fileyaml);
		StringReader input = new StringReader(noteList);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		// create a list of notes based on the yaml info and return them
		List<Note> noteYamlInfo = deserializer.Deserialize<List<Note>>(input);

		return noteYamlInfo;
	}
}
