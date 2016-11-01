using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

public class InventoryYamlParser : MonoBehaviour 
{
	public static InventoryYamlParser Instance;

	[Tooltip("Yaml file that stores the inventory information")]
	public string FileName;

	private Dictionary<string, List<InventoryYAMLModel>> inventoryYamlData;


	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Instance = this;
		inventoryYamlData = new Dictionary<string, List<InventoryYAMLModel>> ();

		LoadInventories ();
	}

	/// <summary>
	/// Loads the inventories from the yaml file.
	/// </summary>
	public void LoadInventories()
	{
		string file = Application.dataPath + "/YAMLFiles/" + FileName;

		string itemListYAML = File.ReadAllText (file);
		StringReader input = new StringReader(itemListYAML);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<InventoryYAMLMap> itemYamlInfo = deserializer.Deserialize<List<InventoryYAMLMap>> (input);

		for(int i = 0; i < itemYamlInfo.Count; ++i)
		{
			inventoryYamlData.Add (itemYamlInfo[i].InventoryName, itemYamlInfo[i].Items);
		}
	}

	/// <summary>
	/// Gets the inventory's contents given an inventory.
	/// </summary>
	/// <returns>The inventory contents.</returns>
	/// <param name="inventoryID">Inventory ID.</param>
	public List<InventoryYAMLModel> GetInventoryContents(string inventoryID)
	{
		return inventoryYamlData [inventoryID];
	}

	/// <summary>
	/// Creates a new inventory.
	/// </summary>
	/// <param name="inventoryID">Inventory ID.</param>
	public void AddNewInventory(string inventoryID)
	{
		inventoryYamlData.Add (inventoryID, new List<InventoryYAMLModel>());
	}


	/// <summary>
	/// Saves the inventory information for all inventories.
	/// </summary>
	public void SaveInventoryInfo()
	{
		List<string> inventoryIDS = new List<string>(inventoryYamlData.Keys);
		List<InventoryYAMLMap> yamlMap = new List<InventoryYAMLMap> ();

		for (int i = 0; i < inventoryIDS.Count; ++i) 
		{
			InventoryYAMLMap mapping = new InventoryYAMLMap ();
			mapping.InventoryName = inventoryIDS [i];
			mapping.Items = inventoryYamlData [inventoryIDS [i]];

			yamlMap.Add (mapping);
		}

		string file = Application.dataPath + "/YAMLFiles/" + FileName;
		File.WriteAllText (file, yamlMap.ToString ());
	}

	/// <summary>
	/// Adds item to inventory.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="inventory">Inventory.</param>
	public void AddToInventory(InventoryYAMLModel item, string inventory)
	{
		inventoryYamlData[inventory].Add (item);
	}

	/// <summary>
	/// Removes item from inventory.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="inventory">Inventory.</param>
	public void RemoveFromInventory(string name, string inventory)
	{
		inventoryYamlData [inventory].RemoveAll (e => e.ItemName == name);
	}
}
