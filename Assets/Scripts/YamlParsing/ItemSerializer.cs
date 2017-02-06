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
	private string districtItemFileName;

	/// <summary>
	/// Initializes a new instance of the <see cref="ItemSerializer"/> class.
	/// </summary>
	/// <param name="itemListFile">File that contains all items.</param>
	/// <param name="districtItemFile">File that contains what items appear in each district.</param>
	public ItemSerializer(string itemListFile, string districtItemFile)
	{
		Filename = itemListFile;
		districtItemFileName = districtItemFile;

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
		string file = FileManager.GetDocument(Filename);
		Dictionary<string, BaseItem> itemDatabase = new Dictionary<string, BaseItem> ();

		StringReader input = new StringReader(file);
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

			if(itemYamlInfo[i].ItemCategories != null && itemYamlInfo[i].ItemCategories.Count > 0)
			{
				for (int j = 0; j < itemYamlInfo[i].ItemCategories.Count; ++j) 
				{
					item.AddItemCategory (itemYamlInfo[i].ItemCategories [j]);
				}
			}

			item.SetUpBaseItem ();
			itemDatabase.Add (item.ItemName, item);
		}

		return itemDatabase;
	}

	public Dictionary<string, List<string>> DeserializeDistrictItemData()
	{
		string file = FileManager.GetDocument(districtItemFileName);

		StringReader input = new StringReader(file);

		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<ItemDistrictModel> itemDistrictData = deserializer.Deserialize<List<ItemDistrictModel>> (input);

		Dictionary<string, List<string>> itemDistrictSortedData = new Dictionary<string, List<string>>();

		for(int i = 0; i < itemDistrictData.Count; ++i)
		{
			itemDistrictSortedData.Add(itemDistrictData[i].DistrictName, itemDistrictData[i].Items);
		}

		return itemDistrictSortedData;
	}
}
