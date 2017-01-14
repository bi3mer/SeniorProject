using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Factory that creates an base items given its name and complex crafted items given recipe and ingredients.
/// Currently just a placeholder class only able to craft a fishing rod.
/// TODO: Configure this to reference a YAML file so that all "craft" actions can go under a single function.
/// TODO: Enable this class to craft base items.
/// </summary>

public class ItemFactory 
{
	/// <summary>
	/// Temporary delegate function that handles crafting a specific object given a list of items
	/// </summary>
	public delegate void CraftItem(List<BaseItem> ingredients);

	public Dictionary<string, BaseItem> ItemDatabase
	{
		get;
		private set;
	}

	public Dictionary<string, List<string>> ItemsByLocation
	{
		get;
		private set;
	}

	public Dictionary<string, DistrictItemRarityConfiguration> DistrictItemRarityInfo
	{
		get;
		private set;
	}

	// the different result tiers of craftable items
	private string[] itemLevels = new string[3] { "Poor", "Good", "Excellent" };

	private int levels = 3;

	private ItemSerializer itemParser;

	/// <summary>
	/// The name of the item file. All yaml files must be placed under "Resources/YAMLFiles"
	/// </summary>
	private const string itemFileName = "ItemListYaml.yml";
	private const string districtItemFileName = "DistrictItemConfiguration.yml";

	/// <summary>
	/// Start this instance. 
	/// Loading in the craftingList by YAML.
	/// </summary>
	public ItemFactory () 
	{
		ItemDatabase = new Dictionary<string, BaseItem> ();
		ItemsByLocation = new Dictionary<string, List<string>>();

		itemParser = new ItemSerializer(itemFileName, districtItemFileName);
		loadItemInformation ();
		loadItemRarityInformation();
	}

	/// <summary>
	/// Gets the blueprints for every item and stores if in the itemDatabase.
	/// </summary>
	private void loadItemInformation()
	{
		ItemDatabase = itemParser.DeserializeItemInformation ();
		ItemsByLocation = itemParser.DeserializeDistrictItemData();
	}

	/// <summary>
	/// Crafts the item based on the requirements of the recipe and the ingredients provided.
	/// The final stats of the resultant item is determined by the level that an item has achieved.
	/// 
	/// Different parts of the recipe affects different stats, which is specified by affectingItems.
	/// If the sum of the values from the affectingItems exceeds a crafting stat's threshold
	/// values for a level, then then the crafting stat is considered that level. For example, if
	/// the threshold for "flexibility" is 3 or a "Good" level, and "Excellent" is 6, and the affecting
	/// items are all of the "Rope" type. If the user has selected 5 "Rope" type items, each with flexiblity 1, 
	/// then resultant sum is 5. It exceeds the "Good" level, but not the "Excellent" level, so the the "flexibility" 
	/// crafting stat is considered "Good".
	/// 
	/// If all the crafting stats are "Good" or above, then the resultant item will be the "Good" version of
	/// the item. If all are "Excellent", then the resultant item will be the "Excellent" version of the item.
	/// Otherwise, it is just the "Poor" version of the item that will be created.
	/// </summary>
	/// <param name="recipe">Recipe for the item.</param>
	/// <param name="ingredients">Ingredients to be used. Required by CraftItem delegate functions.</param>
	public void Craft(Recipe recipe, List<Ingredient> ingredients, Inventory targetInventory)
	{
		List<string> tags = new List<string> ();
		Ingredient currentIngredient;

		for (int i = 0; i < recipe.Requirements.Count; ++i) 
		{
			tags.Add (recipe.Requirements [i].ItemType);
		}

		Dictionary<string, List<Ingredient>> ingredientsByType = SortIngredientsByTag (tags, ingredients);

		int qualityLevel = GetResultingItemLevel(recipe, ingredientsByType);

		for (int i = 0; i < recipe.Requirements.Count; ++i) 
		{
			// for now, the crafting recipes only allow for one item to be selected
			// as an ingredient per requirement, but the list is in place in preparation
			// for multi-item per requirement recipes that will be implemented later
			for (int j = 0; j < ingredientsByType [recipe.Requirements [i].ItemType].Count; ++j) 
			{
				currentIngredient = ingredientsByType [recipe.Requirements [i].ItemType] [j];
				targetInventory.UseItem (currentIngredient.IngredientName, currentIngredient.Amount);
			}
		}

		string itemName = itemLevels [qualityLevel] + " " + recipe.RecipeName;
		BaseItem craftedItem;

		craftedItem = GetBaseItem (itemName);

		// Adds the item to the player's inventory
		// for now, it will only create one at a time
		targetInventory.AddItem (craftedItem, 1);
	}

	/// <summary>
	/// Sorts the ingredients by their tag, which determines which part of the recipe
	/// the ingredient fulfills.
	/// </summary>
	/// <returns>The ingredients by tag.</returns>
	/// <param name="tags">Tags.</param>
	/// <param name="ingredients">Ingredients.</param>
	private Dictionary<string, List<Ingredient>> SortIngredientsByTag(List<string> tags, List<Ingredient> ingredients)
	{
		Dictionary<string, List<Ingredient>> ingredientsByType = new Dictionary<string, List<Ingredient>> ();
		BaseItem currentIngredient;
		Inventory inventory =  Game.Instance.PlayerInstance.Inventory;
		for (int i = 0; i < tags.Count; ++i) 
		{
			ingredientsByType.Add (tags [i], new List<Ingredient>());
		}

		// sorts the ingredients into the "types" that they are by their tags
		// this assumes that there are no overlapping types that a single item can fulfill
		// for example, for a fishing rod that require a rod, a rope, and a hook, the item
		// selected for rod is not also a rope.
		for (int j = 0; j < ingredients.Count; ++j) 
		{
			currentIngredient = inventory.GetInventoryBaseItem(ingredients[j].IngredientName);

			for (int k = 0; k < tags.Count; ++k) 
			{
				if (currentIngredient.Types.Contains (tags [k])) 
				{
					ingredientsByType [tags [k]].Add (ingredients [j]);
				}
			}
		}

		return ingredientsByType;
	}

	/// <summary>
	/// Gets the quality level of the item based on the ingredients that goes into it.
	/// Quality is determined by the sum of specified stats reaching certain thresholds, as
	/// defined by the recipe.
	/// </summary>
	/// <returns>The resulting item level.</returns>
	/// <param name="recipe">Recipe.</param>
	/// <param name="ingredientsByType">Ingredients sorted by type.</param>
	private int GetResultingItemLevel(Recipe recipe, Dictionary<string, List<Ingredient>> ingredientsByType)
	{
		// level starts at highest level, and decreases as the crafting stat fails to reach the threshold value
		// highest level is the number of levels - 1 since 0 is the lowest level
		int qualityLevel = levels - 1;
		BaseItem currentItem;
		Inventory inventory = Game.Instance.PlayerInstance.Inventory;

		// checks each crafting stat that is marked as a stat to check when considering the item's quality level
		for (int x = 0; x < recipe.StatsToCheck.Count; ++x) 
		{
			string stat = recipe.StatsToCheck [x].StatName;
			List<string> affectingItems = recipe.StatsToCheck [x].StatAffectingItems;
			float result = 0;

			// checks each item type that is marked as an item that affects the outcome of the crafting stat
			for (int y = 0; y < affectingItems.Count; ++y)
			{
				float typeSum = 0;
				float typeUnits = 0;

				// Checks each item of that type used during crafting
				// There may be multiple if the recipe called for multiple items for X type and the player chose to fulfill that 
				// requirement by using a mix of different items of the same type.

				// The contribution from the type of item is the average of the values from each of the items, scaled depending on how much of that
				// item was used to fulfill the recipe. So if 2 of one item was used, and only 1 of another, then the first will affect the final
				// contribution twice as much as the second object.
				for (int z = 0; z < ingredientsByType [affectingItems [y]].Count; ++z) 
				{
					currentItem = inventory.GetInventoryBaseItem(ingredientsByType [affectingItems [y]] [z].IngredientName);
					// to weight the average more heavily towards the items that are used more in the crafting process, for each unit of the item being
					// used in the crafting process, that unit will contribute once to the stat. Then the average contribution for a given type of items
					// will be calculated by dividing that sum by the total number of units of all items of the given type.
					for(int i = 0; i < ingredientsByType [affectingItems [y]] [z].Amount; ++i)
					{
						typeSum += currentItem.GetItemAttribute (stat).Value;
					}

					typeUnits +=  ingredientsByType [affectingItems [y]] [z].Amount;
				}

				// add the average values
				// average is calculated by dividing that sum by the total number of units of all items of the given type.
				result += (typeSum / typeUnits);
			}

			if (qualityLevel > 0) 
			{
				// a single crafting stat sum may decrease the level multiple times
				while (result < recipe.StatsToCheck [x].QualityThreshold [qualityLevel - 1]) 
				{
					--qualityLevel;

					if (qualityLevel <= 0) 
					{
						break;
					}
				}
			} 
		}

		return qualityLevel;
	}

	/// <summary>
	/// Gets the base item based on the item's InventoryYAMLInfo. This is generally
	/// used for uncrafted items. These items may have had their names changed after
	/// an action has been applied to them, as such, their unmodifiedName will need
	/// to be used to get their blueprints from the itemDatabase.
	/// 
	/// For example, "River Reed" is stored within the ItemListYaml file, but
	/// "River Reed Thread", which is the result of the "Weave Rope" action
	/// being applied to "River Reed" is not. Instead, the blueprint for "River Reed"
	/// is taken instead.
	/// </summary>
	/// <returns>The base item.</returns>
	/// <param name="item">Item.</param>
	public BaseItem GetBaseItem(InventoryItemYAMLModel item)
	{
		if (ItemDatabase.ContainsKey (item.Item.ItemName)) 
		{
			return ItemDatabase [item.Item.ItemName];
		} 

		return null;
	}

	/// <summary>
	/// Gets the base item by the item's name.
	/// </summary>
	/// <returns>The base item.</returns>
	/// <param name="itemName">Item name.</param>
	public BaseItem GetBaseItem(string itemName)
	{
		if (ItemDatabase.ContainsKey (itemName)) 
		{
			return ItemDatabase [itemName];
		}

		return null;
	}

	/// <summary>
	/// Loads the item rarity information.
	/// </summary>
	private void loadItemRarityInformation()
	{
		List<float> rarityValues = new List<float>();
		DistrictItemRarityInfo = new Dictionary<string, DistrictItemRarityConfiguration>();

		foreach(string key in ItemsByLocation.Keys)
		{
			for(int i = 0; i < ItemsByLocation[key].Count; ++i)
			{
				rarityValues.Add(ItemRarity.GetRarity(GetBaseItem(ItemsByLocation[key][i]).Rarity));
			}

			DistrictItemRarityInfo.Add(key, new DistrictItemRarityConfiguration());
			DistrictItemRarityInfo[key].SetUpVoseAlias(rarityValues);
			rarityValues.Clear();
		}
	}

	/// <summary>
	/// Gets the index of the weighted random item in a district.
	/// </summary>
	/// <returns>The weighted random item index.</returns>
	/// <param name="district">District.</param>
	public int GetWeightedRandomItemIndex(string district)
	{
		return DistrictItemRarityInfo[district].GetWeightedRandomItemIndex();
	}

	/// <summary>
	/// Gets a weighted random base item in a district.
	/// </summary>
	/// <returns>The weighted random base item.</returns>
	/// <param name="district">District.</param>
	public BaseItem GetWeightedRandomBaseItem(string district)
	{
		int index = GetWeightedRandomItemIndex(district);
		return ItemDatabase[ItemsByLocation[district][index]];
	}
}
