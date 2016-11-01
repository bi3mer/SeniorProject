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
	public float WaterContent 
	{ 
		get; 
		set; 
	}

	public float Toughness 
	{ 
		get; 
		set; 
	}

	public float Sweet 
	{ 
		get; 
		set; 
	}

	public float Bitter 
	{ 
		get; 
		set; 
	}

	public float Sour 
	{ 
		get; 
		set; 
	}

	public float Salty 
	{ 
		get; 
		set; 
	}

	public float Spicy 
	{ 
		get; 
		set; 
	}

	public float StomachEffect 
	{ 
		get; 
		set; 
	}

	public float PneumoniaEffect 
	{ 
		get; 
		set; 
	}

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

	/// <summary>
	/// Creates a copy of this plant category.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		PlantCategory category = new PlantCategory ();
		category.WaterContent = WaterContent;
		category.Sweet = Sweet;
		category.Bitter = Bitter;
		category.Sour = Sour;
		category.Salty = Salty;
		category.Spicy = Spicy;
		category.StomachEffect = StomachEffect;
		category.PneumoniaEffect = PneumoniaEffect;

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

		attributes.Add (waterContAttrName, WaterContent);
		attributes.Add (toughAttrName, Toughness);
		attributes.Add (sweetAttrName, Sweet);
		attributes.Add (bitterAttrName, Bitter);
		attributes.Add (sourAttrName, Sour);
		attributes.Add (saltyAttrName, Salty);
		attributes.Add (spicyAttrName, Spicy);
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

		if (Toughness < 3) 
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
		Toughness = Toughness / 2f;
		WaterContent = WaterContent * 1.2f;

		attributes [toughAttrName] = Toughness;
		attributes [waterContAttrName] = WaterContent;

		if (WaterContent > 6f && !baseItem.ItemName.Contains ("Soup")) 
		{
			baseItem.ChangeName (baseItem.ItemName + " Soup");
		} 
		else if (WaterContent < 6f) 
		{
			baseItem.ChangeName("Cooked " + baseItem.ItemName);
		}

		actionCompleted [cookActName] = true;

		if (Toughness >= 3f) 
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
		WaterContent = WaterContent / 4f;
		Toughness = Toughness * 1.5f;

		attributes [waterContAttrName] = WaterContent;
		attributes [toughAttrName] = Toughness;

		baseItem.ChangeName("Dried " + baseItem.ItemName);
		actionCompleted [dryActName] = true;
		baseItem.AddToModifactionList (dryActName);
	}

	/// <summary>
	/// Consumes the item. May cause illness.
	/// </summary>
	/// <param name="variables">Variables.</param>
	public void Eat()
	{
		if (StomachEffect < 0) 
		{
			// TODO: stomach illness code
		} 
		else if (StomachEffect > 0) 
		{
			// TODO: cure for stomach illness
		}	

		if (PneumoniaEffect < 0) 
		{
			// TODO: pneumonia code
		} 
		else if (PneumoniaEffect > 0) 
		{
			// TODO: pneumonia cure code
		}

		InventoryUIBehavior.instance.targetInventory.RemoveItem (baseItem);

		ItemInfoPanelBehavior.instance.CloseItemInfo ();
	}
}