using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

public class ItemSerializer : MonoBehaviour 
{
	public static ItemSerializer instance;
	public string filename;

	// the possible categories that can be added to a baseItem
	// categoryNames stores name of the category
	// categoryTypes stores the type
	private List<string> categoryNames;
	private List<Type> categoryTypes;

	// tag:yaml.org,2002 is shorthanded as "!" in the yaml file, but when registering the tag, it
	// is necessary to use the full Uri
	private string uriPrefix = "tag:yaml.org,2002:";

	// Use this for initialization
	void Awake () 
	{
		instance = this;
		categoryNames = new List<string> ();
		categoryTypes = new List<Type> ();

		SetUpCategoryInformation ();
	}

	/// <summary>
	/// Fills out the categoryNames and cateogryTypes lists with the necessary information
	/// </summary>
	public void SetUpCategoryInformation()
	{
		categoryNames.Add ("plant");
		categoryTypes.Add (typeof(PlantCategory));

		categoryNames.Add ("solid");
		categoryTypes.Add (typeof(SolidCategory));
	}

	/// <summary>
	/// Deserializes the items that can be created from the text file.
	/// </summary>
	/// <returns>The item information.</returns>
	public Dictionary<string, BaseItem> DeserializeItemInformation()
	{
		string fileyaml = Application.dataPath + "/YAMLFiles/" + filename;
		Dictionary<string, BaseItem> itemDatabase = new Dictionary<string, BaseItem> ();

		string itemListYAML = File.ReadAllText (fileyaml);
		StringReader input = new StringReader(itemListYAML);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		for (int x = 0; x < categoryNames.Count; ++x) 
		{
			deserializer.RegisterTagMapping(uriPrefix + categoryNames[x], categoryTypes[x]);
		}

		List<ItemYAMLMap> itemYamlInfo = deserializer.Deserialize<List<ItemYAMLMap>> (input);

		for (int i = 0; i < itemYamlInfo.Count; ++i) 
		{
			BaseItem item = itemYamlInfo [i].baseItem;
			item.InitializeBaseItem ();

			for (int j = 0; j < itemYamlInfo[i].itemCategories.Count; ++j) 
			{
				item.AddItemCategory (itemYamlInfo[i].itemCategories [j]);
			}

			item.SetUpBaseItem ();
			itemDatabase.Add (item.itemName, item);
		}

		return itemDatabase;
	}
}
