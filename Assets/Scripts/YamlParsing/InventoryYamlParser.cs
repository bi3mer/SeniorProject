using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using System;

public class InventoryYamlParser : CraftingSystemSerializer 
{
	private Dictionary<string, List<InventoryItemYAMLModel>> inventoryYamlData;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public InventoryYamlParser(string file)
	{
		inventoryYamlData = new Dictionary<string, List<InventoryItemYAMLModel>> ();
		Filename = file;

		categoryNames = new List<string> ();
		categoryTypes = new List<Type> ();
		SetUpCategoryInformation();
		LoadInventories();
	}

	/// <summary>
	/// Loads the inventories from the yaml file.
	/// </summary>
	public void LoadInventories()
	{
		string file = UnityEngine.Application.dataPath + "/Resources/YAMLFiles/" + Filename;

		string itemListYAML = File.ReadAllText (file);
		StringReader input = new StringReader(itemListYAML);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		for (int x = 0; x < categoryNames.Count; ++x) 
		{
			deserializer.RegisterTagMapping(uriPrefix + categoryNames[x], categoryTypes[x]);
		}

		List<InventoryYAMLModel> inventoryYamlInfo = deserializer.Deserialize<List<InventoryYAMLModel>> (input);

		for(int i = 0; i < inventoryYamlInfo.Count; ++i)
		{
			inventoryYamlData.Add (inventoryYamlInfo[i].InventoryName, inventoryYamlInfo[i].Items);
		}
	}

	/// <summary>
	/// Gets the inventory's contents given an inventory.
	/// </summary>
	/// <returns>The inventory contents.</returns>
	/// <param name="inventoryID">Inventory ID.</param>
	public List<InventoryItemYAMLModel> GetInventoryContents(string inventoryID)
	{
		return inventoryYamlData [inventoryID];
	}

	/// <summary>
	/// Creates a new inventory.
	/// </summary>
	/// <param name="inventoryID">Inventory ID.</param>
	public void AddNewInventory(string inventoryID)
	{
		inventoryYamlData.Add (inventoryID, new List<InventoryItemYAMLModel>());
	}


	/// <summary>
	/// Saves the inventory information for all inventories.
	/// </summary>
	public void SaveInventoryInfo()
	{
		List<string> inventoryIDS = new List<string>(inventoryYamlData.Keys);
		List<InventoryYAMLModel> yamlMap = new List<InventoryYAMLModel> ();

		for (int i = 0; i < inventoryIDS.Count; ++i) 
		{
			InventoryYAMLModel mapping = new InventoryYAMLModel ();
			mapping.InventoryName = inventoryIDS [i];
			mapping.Items = inventoryYamlData [inventoryIDS [i]];

			yamlMap.Add (mapping);
		}

		string file = UnityEngine.Application.dataPath + "/Resources/YAMLFiles/" + Filename;
		File.WriteAllText (file, yamlMap.ToString ());
	}

	/// <summary>
	/// Adds item to inventory.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="inventory">Inventory.</param>
	public void AddToInventory(InventoryItemYAMLModel item, string inventory)
	{
		inventoryYamlData[inventory].Add(item);
	}

	/// <summary>
	/// Removes item from inventory.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="inventory">Inventory.</param>
	public void RemoveFromInventory(int location, string inventory)
	{
		inventoryYamlData [inventory][location] = new InventoryItemYAMLModel();
	}
}
