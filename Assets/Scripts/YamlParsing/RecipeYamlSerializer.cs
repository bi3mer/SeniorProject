using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

public class RecipeYamlSerializer : MonoBehaviour 
{
	public static RecipeYamlSerializer instance;

	[Tooltip("The yaml file used to store the recipes")]
	public string filename;

	// Use this for initialization
	void Awake () 
	{
		instance = this;
	}

	public Dictionary<string, Recipe> LoadRecipes()
	{
		string fileyaml = Application.dataPath + "/YAMLFiles/" + filename;
		Dictionary<string, Recipe> recipeDatabase = new Dictionary<string, Recipe> ();

		string itemListYAML = File.ReadAllText (fileyaml);
		StringReader input = new StringReader(itemListYAML);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<Recipe> itemYamlInfo = deserializer.Deserialize<List<Recipe>> (input);

		for (int i = 0; i < itemYamlInfo.Count; ++i) 
		{
			recipeDatabase.Add (itemYamlInfo [i].recipeName, itemYamlInfo [i]);
		}

		return recipeDatabase;
	}
}
