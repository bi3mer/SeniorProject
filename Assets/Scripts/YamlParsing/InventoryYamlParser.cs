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
	/// Serializes the contents of inventoryYamlData and updates the YAML file.
	/// </summary>
	public void SaveInventoriesToFile()
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
	/// Saves the inventory passed to it to inventoryYamlData.
	/// </summary>
	/// <param name="inventory">Inventory.</param>
	/// <param name="inventoryName">Inventory name.</param>
	public void SaveInventory(Inventory inventory, string inventoryName)
	{
		List<InventoryItemYAMLModel> saveData = new List<InventoryItemYAMLModel>();
		Stack[] contents =  inventory.GetInventory();

		for(int i = 0; i < contents.Length; ++i)
		{
			InventoryItemYAMLModel itemModel = new InventoryItemYAMLModel ();
			itemModel.Item = contents[i].Item;
			itemModel.ItemAmount = contents[i].Amount;
			itemModel.StackId = contents[i].Id;
			itemModel.ItemCategories = contents[i].Item.GetItemCategories();

			saveData.Add(itemModel);
		}

		inventoryYamlData[inventoryName] = saveData;
	}
}
