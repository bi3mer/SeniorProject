using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;

public class ItemSerializer : MonoBehaviour 
{
	public static ItemSerializer instance;
	public string filename;
	// Use this for initialization
	void Awake () 
	{
		instance = this;
	}

	public Dictionary<string, BaseItem> DeserializeItemInformation()
	{
		string file = Application.dataPath + "/JSONFiles/" + filename;
		string itemList = File.ReadAllText (file);
		JSONArray rawItemData = JSON.Parse (itemList).AsArray;
		Dictionary<string, BaseItem> itemDatabase = new Dictionary<string, BaseItem> ();

		for (int i = 0; i < rawItemData.Count; ++i) 
		{
			BaseItem item = JsonUtility.FromJson<BaseItem> (rawItemData [i] ["baseItem"]);
			item.InitializeBaseItem ();
			JSONArray categories = rawItemData[i]["itemCategories"].AsArray;
			for (int j = 0; j < categories.Count; ++j) 
			{
				ItemCategory cat = DeserializeCategory (categories [j] ["categoryName"], categories [j] ["categoryJson"]);
				if (cat != null) 
				{
					item.AddItemCategory (cat);
				}
			}

			item.SetUpBaseItem ();
			itemDatabase.Add (item.itemName, item);
		}

		return itemDatabase;
	}

	public ItemCategory DeserializeCategory(string category, string json)
	{
		switch (category.ToLower())
		{
		case "solidcategory":
			return JsonUtility.FromJson<SolidCategory> (json);
		case "plantcategory":
			return JsonUtility.FromJson<PlantCategory> (json);
		default:
			return null;
		}
	}

	public JSONNode SerializeItem(BaseItem item)
	{
		JSONNode itemHolder = new JSONArray();

		JSONNode baseItemHolder = new JSONClass ();
		baseItemHolder ["itemName"] = item.itemName;
		baseItemHolder ["itemCategories"] = new JSONArray ();
		baseItemHolder ["baseItem"] = JsonUtility.ToJson (item);
		ItemCategory currentCategory = item.GetLastCategory ();

		while (currentCategory != null) 
		{
			JSONNode itemCategoryHolder = new JSONClass ();

			itemCategoryHolder ["categoryName"] = currentCategory.GetType ().Name;
			itemCategoryHolder ["categoryJson"] = JsonUtility.ToJson (currentCategory);
			baseItemHolder ["itemCategories"].Add (itemCategoryHolder);

			currentCategory = currentCategory.GetConnected ();
		}

		itemHolder.Add (baseItemHolder);
		return itemHolder;
	}
}
