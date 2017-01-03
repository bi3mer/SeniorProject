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

		// string itemListYAML = File.ReadAllText (fileyaml);
		// StringReader input = new StringReader(itemListYAML);
		// Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		// List<Recipe> itemYamlInfo = deserializer.Deserialize<List<Recipe>> (input);

		// for (int i = 0; i < itemYamlInfo.Count; ++i) 
		// {
		// 	recipeDatabase.Add (itemYamlInfo [i].RecipeName, itemYamlInfo [i]);
		// }

		return noteDatabase;
	}
}
