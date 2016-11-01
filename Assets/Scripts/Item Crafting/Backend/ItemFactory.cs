using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Factory that creates an base items given its name and complex crafted items given recipe and ingredients.
/// Currently just a placeholder class only able to craft a fishing rod.
/// TODO: Configure this to reference a YAML file so that all "craft" actions can go under a single function.
/// TODO: Enable this class to craft base items.
/// </summary>

public class ItemFactory : MonoBehaviour 
{
	// Temporary delegate function that handles crafting a specific object given a list of items
	public delegate void CraftItem(List<BaseItem> ingredients);

	[Tooltip("Basic template gameobject with a baseItem component that is used to create items. Should be in scene.")]
	public GameObject baseObjectTemplate;

	// instance of the factory so that it can be easily accessed by other classes
	public static ItemFactory instance;

	//dictionary of craftItem delegate functions that are keyed by the name of the item they are supposed to craft
	public Dictionary<string, CraftItem> craftingList;


	private Dictionary<string, BaseItem> itemDatabase;

	// the different result tiers of craftable items
	private string[] itemLevels = new string[3] { "Poor", "Good", "Excellent" };

	/// <summary>
	/// Start this instance. 
	/// TODO: Loading in the craftingList by YAML.
	/// </summary>
	void Start () 
	{
		instance = this;

		itemDatabase = new Dictionary<string, BaseItem> ();
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
	public void Craft(Recipe recipe, List<BaseItem> ingredients)
	{
		Dictionary<string, List<BaseItem>> ingredientsByType = new Dictionary<string, List<BaseItem>> ();
		List<string> ingredientsKey;

		for (int i = 0; i < recipe.requirements.Count; ++i) 
		{
			ingredientsByType.Add (recipe.requirements [i].itemType, new List<BaseItem>());
		}

		ingredientsKey = new List<string> (ingredientsByType.Keys);

		// sorts the ingredients into the "types" that they are by their tags
		// this assumes that there are no overlapping types that a single item can fulfill
		// for example, for a fishing rod that require a rod, a rope, and a hook, the item
		// selected for rod is not also a rope.
		for (int j = 0; j < ingredients.Count; ++j) 
		{
			for (int k = 0; k < ingredientsKey.Count; ++k) 
			{
				if (ingredients [j].Tags.Contains (ingredientsKey [k])) 
				{
					ingredientsByType [ingredientsKey [k]].Add (ingredients [j]);
				}
			}
		}

		// level stats at excellent, and decreases as the crafting stat fails to reach the threshold value

		int level = 2;

		for (int x = 0; x < recipe.statsToCheck.Count; ++x) 
		{
			string stat = recipe.statsToCheck [x].statName;
			List<string> affectingItems = recipe.statsToCheck [x].affectingItems;
			float result = 0;

			for (int y = 0; y < affectingItems.Count; ++y)
			{
				for (int z = 0; z < ingredientsByType [affectingItems [y]].Count; ++z) {
					result += ingredientsByType [affectingItems [y]] [z].GetItemAttributes () [stat];
				}
			}

			if (level > 0) 
			{
				// a single crafting stat sum may decrease the level multiple times
				while (result < recipe.statsToCheck [x].threshold [level - 1]) 
				{
					--level;

					if (level <= 0) 
					{
						break;
					}
				}
			} 
		}

		for (int a = 0; a < recipe.requirements.Count; ++a) 
		{
			// for now, the crafting recipes only allow for one item to be selected
			// as an ingredient per requirement, but the list is in place in preparation
			// for multi-item per requirement recipes that will be implemented later
			for (int b = 0; b < ingredientsByType [recipe.requirements [a].itemType].Count; ++b) 
			{
				InventoryUIBehavior.instance.targetInventory.UseItem (ingredientsByType [recipe.requirements [a].itemType] [b].ItemName, recipe.requirements [a].amountRequired);
			}
		}

		string itemName = itemLevels [level] + " " + recipe.recipeName;
		BaseItem craftedItem;

		craftedItem = GetBaseItem (itemName);

		// Adds the item to the player's inventory
		InventoryUIBehavior.instance.targetInventory.AddItem (craftedItem);
		InventoryUIBehavior.instance.RefreshInventory ();
	}

	/// <summary>
	/// Gets the blueprints for every item and stores if in the itemDatabase.
	/// </summary>
	public void LoadItemInformation()
	{
		itemDatabase = ItemSerializer.instance.DeserializeItemInformation ();
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
	public BaseItem GetBaseItem(InventoryYAMLInfo item)
	{
		if (itemDatabase.ContainsKey (item.itemName)) 
		{
			return itemDatabase [item.itemName];
		} 
		else if (itemDatabase.ContainsKey (item.unmodifiedItemName)) 
		{
			return itemDatabase [item.unmodifiedItemName];
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
