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
	/// Deserializes the district item data.
	/// </summary>
	/// <param name="landDistrictStorage">Dictionary where information about items appearing on land should appear.</param>
	/// <param name="waterDistrictStorage">Dictionary where information about items that can appear floating in water should appear.</param>
	public void DeserializeDistrictItemData(ref Dictionary<string, List<string>> landDistrictStorage, ref Dictionary<string, List<string>> waterDistrictStorage)
	{
		string file = FileManager.GetDocument(districtItemFileName);

		StringReader input = new StringReader(file);

		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<ItemDistrictModel> itemDistrictData = deserializer.Deserialize<List<ItemDistrictModel>> (input);

		for(int i = 0; i < itemDistrictData.Count; ++i)
		{
			landDistrictStorage.Add(itemDistrictData[i].DistrictName, itemDistrictData[i].LandItems);
			waterDistrictStorage.Add(itemDistrictData[i].DistrictName, itemDistrictData[i].WaterItems);
		}
	}
}
