using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class InventoryJsonParser : MonoBehaviour 
{
	public static InventoryJsonParser instance;

	[Tooltip("JSON file that stores the inventory information")]
	public string fileName;

	private Dictionary<string, string> inventoryJSONData;

	void Awake()
	{
		instance = this;
		inventoryJSONData = new Dictionary<string, string> ();

		LoadInventories ();
	}

	public void LoadInventories()
	{
		string file = Application.dataPath + "/JSONFiles/" + fileName;
		JSONArray rawInventoryData = JSON.Parse (File.ReadAllText (file))["inventories"].AsArray;
		for(int i = 0; i < rawInventoryData.Count; ++i)
		{
			inventoryJSONData.Add (rawInventoryData [i] ["name"], rawInventoryData [i] ["items"].ToString());
		}
	}

	public List<InventoryItem> GetInventoryContents(string inventoryID)
	{
		JSONArray itemInfo = JSON.Parse(inventoryJSONData [inventoryID]).AsArray;
		List<InventoryItem> inventoryItems = new List<InventoryItem> ();

		for (int i = 0; i < itemInfo.Count; ++i) 
		{
			List<string> mods = new List<string> ();
			JSONArray modArray = itemInfo [i] ["modifications"].AsArray;

			for (int j = 0; j < modArray.Count; ++j) 
			{
				mods.Add (modArray [i]);
			}

			inventoryItems.Add (new InventoryItem(itemInfo [i]["itemName"], 
				itemInfo[i]["unmodifiedName"], itemInfo[i]["amount"].AsInt, mods));
		}

		return inventoryItems;
	}

	public void AddNewInventory(string inventoryID)
	{
		JSONNode inventory = new JSONClass ();
		inventory ["id"] = inventoryID;
		inventory ["items"] = new JSONArray ();

		inventoryJSONData.Add (inventoryID, inventory);
	}

	public void SaveInventoryInfo()
	{
		JSONNode inventoryHolder = new JSONClass ();
		JSONArray inventoryData = new JSONArray ();

		List<string> inventoryIDS = new List<string>(inventoryJSONData.Keys);
		for (int i = 0; i < inventoryIDS.Count; ++i) 
		{
			JSONNode inventory = new JSONClass ();
			inventory["name"] = inventoryIDS [i];
			inventory["items"] = inventoryJSONData [inventoryIDS [i]];

			inventoryData.Add (inventory);
		}

		inventoryHolder ["inventories"] = inventoryData;

		string file = Application.dataPath + "/JSONFiles/" + fileName;
		File.WriteAllText (file, inventoryHolder.ToString ());
	}

	public void AddToInventory(InventoryItem item, string inventory)
	{
		JSONNode info = new JSONClass ();
		info ["itemName"] = item.ItemName;
		info ["amount"] = item.ItemAmount.ToString();
		info ["unmodifiedName"] = item.UnmodifiedName;

		JSONNode inventoryJSON = JSON.Parse (inventoryJSONData [inventory]);
		inventoryJSON["items"].AsArray.Add (info);
	}

	public void RemoveFromInventory(string name, string inventory)
	{
		((JSONArray)((JSONNode)inventoryJSONData [inventory])["items"]).Remove (name);
	}
}
