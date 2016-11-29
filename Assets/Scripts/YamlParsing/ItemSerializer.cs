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
using UnityEngine;

public class ItemSerializer: CraftingSystemSerializer
{
	string naturallyOccuringListFileName;
	/// <summary>
	/// Initializes a new instance of the <see cref="ItemSerializer"/> class.
	/// </summary>
	/// <param name="file">File.</param>
	public ItemSerializer(string itemListFile, string naturallyOccuringListFile)
	{
		Filename = itemListFile;
		naturallyOccuringListFileName = naturallyOccuringListFile;

		categoryNames = new List<string> ();
		categoryTypes = new List<Type> ();

		SetUpCategoryInformation();
	}

	/// <summary>
	/// Deserializes the items that can be created from the text file.
	/// </summary>
	/// <returns>The item information.</returns>
	public Dictionary<string, BaseItem> DeserializeItemInformation()
	{
		string file = "YAMLFiles/" + Filename;
		Dictionary<string, BaseItem> itemDatabase = new Dictionary<string, BaseItem> ();

		TextAsset itemListYaml = (TextAsset) Resources.Load(file, typeof(TextAsset));
		StringReader input = new StringReader(itemListYaml.text);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		for (int x = 0; x < categoryNames.Count; ++x) 
		{
			deserializer.RegisterTagMapping(uriPrefix + categoryNames[x], categoryTypes[x]);
		}

		List<ItemYAMLMap> itemYamlInfo = deserializer.Deserialize<List<ItemYAMLMap>> (input);

		for (int i = 0; i < itemYamlInfo.Count; ++i) 
		{
			BaseItem item = itemYamlInfo [i].BaseItem;
			item.InitializeBaseItem ();

			for (int j = 0; j < itemYamlInfo[i].ItemCategories.Count; ++j) 
			{
				item.AddItemCategory (itemYamlInfo[i].ItemCategories [j]);
			}

			item.SetUpBaseItem ();
			itemDatabase.Add (item.ItemName, item);
		}

		return itemDatabase;
	}

	/// <summary>
	/// Deserializes the data for objects that can be found in the world. That is items that do not require crafting to obtain.
	/// </summary>
	/// <returns>The naturally occuring item data.</returns>
	public List<GeneratableItemInfoModel> DeserializeNaturallyOccuringItemData()
	{
		string file = "YAMLFiles/" + naturallyOccuringListFileName;

		TextAsset itemListYaml = (TextAsset) Resources.Load(file, typeof(TextAsset));
		StringReader input = new StringReader(itemListYaml.text);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		return deserializer.Deserialize<List<GeneratableItemInfoModel>> (input);
	} 
}
