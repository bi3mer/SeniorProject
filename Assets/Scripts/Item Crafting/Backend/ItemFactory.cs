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

	private Dictionary<string, BaseItem> itemDatabase;

	// the different result tiers of craftable items
	private string[] itemLevels = new string[3] { "Poor", "Good", "Excellent" };

	private int levels = 3;

	private ItemSerializer itemParser;

	/// <summary>
	/// Start this instance. 
	/// Loading in the craftingList by YAML.
	/// </summary>
	public ItemFactory (string itemFile) 
	{
		itemDatabase = new Dictionary<string, BaseItem> ();
		itemParser = new ItemSerializer(itemFile);

		LoadItemInformation ();
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
	public void Craft(Recipe recipe, List<BaseItem> ingredients, Inventory targetInventory)
	{
		List<string> tags = new List<string> ();

		for (int i = 0; i < recipe.Requirements.Count; ++i) 
		{
			tags.Add (recipe.Requirements [i].ItemType);
		}

		Dictionary<string, List<BaseItem>> ingredientsByType = SortIngredientsByTag (tags, ingredients);

		int qualityLevel = GetResultingItemLevel(recipe, ingredientsByType);

		for (int i = 0; i < recipe.Requirements.Count; ++i) 
		{
			// for now, the crafting recipes only allow for one item to be selected
			// as an ingredient per requirement, but the list is in place in preparation
			// for multi-item per requirement recipes that will be implemented later
			for (int j = 0; j < ingredientsByType [recipe.Requirements [i].ItemType].Count; ++j) 
			{
				targetInventory.UseItem (ingredientsByType [recipe.Requirements [i].ItemType] [j], recipe.Requirements [i].AmountRequired);
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
	/// Gets the blueprints for every item and stores if in the itemDatabase.
	/// </summary>
	public void LoadItemInformation()
	{
		itemDatabase = itemParser.DeserializeItemInformation ();
	}

	/// <summary>
	/// Sorts the ingredients by their tag, which determines which part of the recipe
	/// the ingredient fulfills.
	/// </summary>
	/// <returns>The ingredients by tag.</returns>
	/// <param name="tags">Tags.</param>
	/// <param name="ingredients">Ingredients.</param>
	public Dictionary<string, List<BaseItem>> SortIngredientsByTag(List<string> tags, List<BaseItem> ingredients)
	{
		Dictionary<string, List<BaseItem>> ingredientsByType = new Dictionary<string, List<BaseItem>> ();

		for (int i = 0; i < tags.Count; ++i) 
		{
			ingredientsByType.Add (tags [i], new List<BaseItem>());
		}

		// sorts the ingredients into the "types" that they are by their tags
		// this assumes that there are no overlapping types that a single item can fulfill
		// for example, for a fishing rod that require a rod, a rope, and a hook, the item
		// selected for rod is not also a rope.
		for (int j = 0; j < ingredients.Count; ++j) 
		{
			for (int k = 0; k < tags.Count; ++k) 
			{
				if (ingredients [j].Types.Contains (tags [k])) 
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
	public int GetResultingItemLevel(Recipe recipe, Dictionary<string, List<BaseItem>> ingredientsByType)
	{
		// level starts at highest level, and decreases as the crafting stat fails to reach the threshold value
		// highest level is the number of levels - 1 since 0 is the lowest level
		int qualityLevel = levels - 1;

		for (int x = 0; x < recipe.StatsToCheck.Count; ++x) 
		{
			string stat = recipe.StatsToCheck [x].StatName;
			List<string> affectingItems = recipe.StatsToCheck [x].StatAffectingItems;
			float result = 0;

			for (int y = 0; y < affectingItems.Count; ++y)
			{
				for (int z = 0; z < ingredientsByType [affectingItems [y]].Count; ++z) {
					result += ingredientsByType [affectingItems [y]] [z].GetItemAttribute (stat).Value;
				}
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
		if (itemDatabase.ContainsKey (item.Item.ItemName)) 
		{
			return itemDatabase [item.Item.ItemName];
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
		if (itemDatabase.ContainsKey (itemName)) 
		{
			return itemDatabase [itemName];
		}

		return null;
	}
}
