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
	public static InventoryYamlParser instance;

	[Tooltip("Yaml file that stores the inventory information")]
	public string fileName;

	private Dictionary<string, List<InventoryYAMLInfo>> inventoryYamlData;

	void Awake()
	{
		instance = this;
		inventoryYamlData = new Dictionary<string, List<InventoryYAMLInfo>> ();

		LoadInventories ();
	}

	public void LoadInventories()
	{
		string file = Application.dataPath + "/YAMLFiles/" + fileName;

		string itemListYAML = File.ReadAllText (file);
		StringReader input = new StringReader(itemListYAML);
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

		List<InventoryYAMLMap> itemYamlInfo = deserializer.Deserialize<List<InventoryYAMLMap>> (input);

		for(int i = 0; i < itemYamlInfo.Count; ++i)
		{
			inventoryYamlData.Add (itemYamlInfo[i].inventoryName, itemYamlInfo[i].items);
		}
	}

	public List<InventoryYAMLInfo> GetInventoryContents(string inventoryID)
	{
		return inventoryYamlData [inventoryID];
	}

	public void AddNewInventory(string inventoryID)
	{
		inventoryYamlData.Add (inventoryID, new List<InventoryYAMLInfo>());
	}

	public void SaveInventoryInfo()
	{
		List<string> inventoryIDS = new List<string>(inventoryYamlData.Keys);
		List<InventoryYAMLMap> yamlMap = new List<InventoryYAMLMap> ();

		for (int i = 0; i < inventoryIDS.Count; ++i) 
		{
			InventoryYAMLMap mapping = new InventoryYAMLMap ();
			mapping.inventoryName = inventoryIDS [i];
			mapping.items = inventoryYamlData [inventoryIDS [i]];

			yamlMap.Add (mapping);
		}

		string file = Application.dataPath + "/YAMLFiles/" + fileName;
		File.WriteAllText (file, yamlMap.ToString ());
	}

	public void AddToInventory(InventoryYAMLInfo item, string inventory)
	{
		inventoryYamlData[inventory].Add (item);
	}

	public void RemoveFromInventory(string name, string inventory)
	{
		inventoryYamlData [inventory].RemoveAll (e => e.itemName == name);
	}
}
