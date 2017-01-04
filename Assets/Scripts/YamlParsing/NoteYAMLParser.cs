using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

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
	/// Loads the notes from the yaml file.
	/// </summary>
	/// <returns>The notes.</returns>
	public Dictionary<string, Note> LoadNotes()
	{
		string fileyaml = UnityEngine.Application.dataPath + "/Resources/YAMLFiles/" + Filename;
		Dictionary<string, Note> noteDatabase = new Dictionary<string, Note> ();

		string noteList = File.ReadAllText (fileyaml);
		StringReader input = new StringReader(noteList);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<Note> noteYamlInfo = deserializer.Deserialize<List<Note>>(input);
		
		for (int i = 0; i < noteYamlInfo.Count; ++i) 
		{
			noteDatabase.Add (noteYamlInfo [i].Title, noteYamlInfo [i]);
		}

		return noteDatabase;
	}
}
