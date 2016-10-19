using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace CraftingSystem
{
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

		/// <summary>
		/// Start this instance. 
		/// TODO: Loading in the craftingList by YAML.
		/// </summary>
		void Start () 
		{
			instance = this;
			CraftItem fishingRod = new CraftItem (CraftFishingRod);
			craftingList = new Dictionary<string, CraftItem> ();
			craftingList.Add ("Fishing Rod", fishingRod);
		}

		/// <summary>
		/// Calls upon the delegate function that will create an item.
		/// </summary>
		/// <param name="name">Name of the item to be crafted</param>
		/// <param name="ingredients">Ingredients to be used. Required by CraftItem delegate functions.</param>
		public void Craft(string name, List<BaseItem> ingredients)
		{
			craftingList [name](ingredients);
		}

		/// <summary>
		/// Crafts the fishing rod. Stats of the fishing rod are averages of the components that make it up.
		/// Requires: Rod, Rope, Hook
		/// </summary>
		/// <param name="ingredients">Ingredients that make the fishing rod.</param>
		public void CraftFishingRod(List<BaseItem> ingredients)
		{
			GameObject ob = GameObject.Instantiate (baseObjectTemplate);
			ob.SetActive (true);

			BaseItem baseOb = ob.GetComponent<BaseItem> ();

			BaseItem rod = null;
			BaseItem line = null;
			BaseItem hook = null;

			for (int i = 0; i < ingredients.Count; ++i) 
			{
				if (ingredients [i].tags.Contains ("Rod")) 
				{
					rod = ingredients [i];
				} 
				else if (ingredients [i].tags.Contains ("Rope")) 
				{
					line = ingredients [i];
				} 
				else if (ingredients [i].tags.Contains ("Hook")) 
				{
					hook = ingredients [i];
				}
			}

			string name = rod.itemName + " Fishing Rod";

			baseOb.ChageName(name);
			ob.name = baseOb.itemName;

			baseOb.amount = 1;
			baseOb.comprisedOf.Add (rod);
			baseOb.comprisedOf.Add (line);
			baseOb.comprisedOf.Add (hook);

			SolidCategory sol = ob.AddComponent<SolidCategory> ();
			Dictionary<string, float> rodAttr = rod.GetAttributes ();
			Dictionary<string, float> lineAttr = line.GetAttributes ();
			Dictionary<string, float> hookAttr = hook.GetAttributes ();

			sol.durability = (rodAttr ["durability"] + lineAttr ["durability"] + hookAttr["durability"]) / 3f;
			sol.elasticity = (rodAttr ["elasticity"] + lineAttr ["elasticity"]) / 2f;
			sol.flexibility = (rodAttr ["flexibility"] + lineAttr ["flexibility"]) / 2f;
			sol.sharpness = hookAttr ["sharpness"];
			sol.stickiness = rodAttr ["stickiness"];
			sol.thickness = rodAttr ["thickness"];

			baseOb.SetUpBaseObject ();
			InventoryUIBehavior.instance.targetInventory.AddItem (ob);
		}
	}
}
