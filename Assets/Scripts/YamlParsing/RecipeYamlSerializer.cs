using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

public class RecipeYamlSerializer : CraftingSystemSerializer 
{
	/// <summary>
	/// Awake this instance.
	/// </summary>
	public RecipeYamlSerializer (string file) 
	{
		Filename = file;
	}

	/// <summary>
	/// Loads the recipes from the yaml file.
	/// </summary>
	/// <returns>The recipes.</returns>
	public Dictionary<string, Recipe> LoadRecipes()
	{
		string fileyaml = UnityEngine.Application.dataPath + "/Resources/YAMLFiles/" + Filename;
		Dictionary<string, Recipe> recipeDatabase = new Dictionary<string, Recipe> ();

		string itemListYAML = File.ReadAllText (fileyaml);
		StringReader input = new StringReader(itemListYAML);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<Recipe> itemYamlInfo = deserializer.Deserialize<List<Recipe>> (input);

		for (int i = 0; i < itemYamlInfo.Count; ++i) 
		{
			recipeDatabase.Add (itemYamlInfo [i].RecipeName, itemYamlInfo [i]);
		}

		return recipeDatabase;
	}
}
