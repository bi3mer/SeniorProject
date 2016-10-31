using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Contains attributes and actions that befit a plant category item.
/// </summary>
public class PlantCategory : ItemCategory 
{
	public float waterContent { get; set; }
	public float toughness { get; set; }
	public float sweet { get; set; }
	public float bitter { get; set; }
	public float sour { get; set; }
	public float salty { get; set; }
	public float spicy { get; set; }
	public float stomachEffect { get; set; }
	public float pneumoniaEffect { get; set; }

	private const string waterContAttrName = "waterContent";
	private const string toughAttrName = "toughness";
	private const string sweetAttrName = "sweet";
	private const string bitterAttrName = "bitter";
	private const string sourAttrName = "sour";
	private const string saltyAttrName = "salty";
	private const string spicyAttrName = "spicy";

	private const string dryActName = "Dry";
	private const string cookActName = "Cook";
	private const string eatActName = "Eat";

	public override ItemCategory GetDuplicate()
	{
		PlantCategory category = new PlantCategory ();
		category.waterContent = waterContent;
		category.sweet = sweet;
		category.bitter = bitter;
		category.sour = sour;
		category.salty = salty;
		category.spicy = spicy;
		category.stomachEffect = stomachEffect;
		category.pneumoniaEffect = pneumoniaEffect;

		category.ReadyCategory ();

		List<string> keys = new List<string> (actionCompleted.Keys);

		for(int i = 0; i < keys.Count; ++i)
		{
			category.actionCompleted[keys[i]] = actionCompleted [keys[i]];
		}

		return category;
	}

	/// <summary>
	/// Readies the item category by adding the attributes and actions it can complete.
	/// </summary>
	public override void ReadyCategory()
	{
		attributes = new Dictionary<string, float> ();

		if (actionCompleted == null) 
		{
			actionCompleted = new Dictionary<string, bool> ();
			actionCompleted.Add (dryActName, false);
			actionCompleted.Add (cookActName, false);
		}

		attributes.Add (waterContAttrName, waterContent);
		attributes.Add (toughAttrName, toughness);
		attributes.Add (sweetAttrName, sweet);
		attributes.Add (bitterAttrName, bitter);
		attributes.Add (sourAttrName, sour);
		attributes.Add (saltyAttrName, salty);
		attributes.Add (spicyAttrName, spicy);
	}

	/// <summary>
	/// Gets all the action that an item can perform.
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.</returns>
	public override Dictionary<string, List<Action>> GetPossibleActions()
	{
		Dictionary<string, List<Action>> possibleActions = new Dictionary<string, List<Action>> ();

		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}

		if (!possibleActions.ContainsKey (defaultActionCategoryName)) 
		{
			possibleActions.Add (defaultActionCategoryName, new List<Action> ());
		}

		if (!actionCompleted [cookActName]) 
		{
			UnityAction cookAct = new UnityAction (Cook);
			Action cook = new Action (cookActName, cookAct);
			possibleActions [defaultActionCategoryName].Add (cook);
		}
	
		if (!actionCompleted [dryActName]) 
		{
			UnityAction dryAct = new UnityAction (Dry);
			Action dry = new Action (dryActName, dryAct);
			possibleActions[defaultActionCategoryName].Add (dry);
		}

		if (toughness < 3) 
		{
			UnityAction eatAct = new UnityAction (Eat);
			Action eat = new Action (eatActName, eatAct);
			possibleActions [defaultActionCategoryName].Add (eat);
		}

		return possibleActions;
	}

	/// <summary>
	/// Cooks the item. Lowers toughness and increases water content. If the item has high enough water content, it becomes soup.
	/// Otherwise, it is just called Cooked X.
	/// </summary>
	/// <param name="variables">Variables.</param>
	public void Cook()
	{
		toughness = toughness / 2f;
		waterContent = waterContent * 1.2f;

		attributes [toughAttrName] = toughness;
		attributes [waterContAttrName] = waterContent;

		if (waterContent > 6f && !baseItem.itemName.Contains ("Soup")) 
		{
			baseItem.ChangeName (baseItem.itemName + " Soup");
		} 
		else if (waterContent < 6f) 
		{
			baseItem.ChangeName("Cooked " + baseItem.itemName);
		}

		actionCompleted [cookActName] = true;

		if (toughness >= 3f) 
		{
			baseItem.RemoveCategories (new List<string> (){ "SolidAttribute" });
		}

		baseItem.AddToModifactionList (cookActName);
	}

	/// <summary>
	/// Dries this item. Lowers the waterContent greatly and increases toughness.
	/// </summary>
	public void Dry()
	{
		waterContent = waterContent / 4f;
		toughness = toughness * 1.5f;

		attributes [waterContAttrName] = waterContent;
		attributes [toughAttrName] = toughness;

		baseItem.ChangeName("Dried " + baseItem.itemName);
		actionCompleted [dryActName] = true;
		baseItem.AddToModifactionList (dryActName);
	}

	/// <summary>
	/// Consumes the item. May cause illness.
	/// </summary>
	/// <param name="variables">Variables.</param>
	public void Eat()
	{
		if (stomachEffect < 0) 
		{
			// insert stomach illness code
		} 
		else if (stomachEffect > 0) 
		{
			// insert cure for stomach illness
		}	

		if (pneumoniaEffect < 0) 
		{
			// insert pneumonia code
		} 
		else if (pneumoniaEffect > 0) 
		{
			// insert pneumonia cure code
		}

		InventoryUIBehavior.instance.targetInventory.RemoveItem (baseItem);

		ItemInfoPanelBehavior.instance.CloseItemInfo ();
	}
}