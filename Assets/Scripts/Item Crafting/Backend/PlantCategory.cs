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

	// variables needed to adjust the WaterContent and Toughness attributes in the drying function
	private const float dryingWaterDecreaseRate = 0.25f;
	private const float dryingToughnessIncreaseRate = 1.5f;

	// variables needed to adujst the WaterContent and Toughness attributes in the cooking function
	private const float cookingWaterIncreaseRate = 1.2f;
	private const float cookingToughnessDecreaseRate = 0.5f;

	// max toughness before you can't eat an object
	private const float toughnessEdibleThreshold = 3f;

	// how high the waterContent needs to be before it is soup
	private const float waterContentSoupThreshold = 6f;

	// string that will be added to the names of items that are soup
	private const string soupNameAddition = "Soup";

	// string that will be added to the names of items that cooked by default
	private const string defaultCookedNameAddition = "Cooked";

	// string that will be added to the names of items that are dried
	private const string defaultDryNameAddition = "Dried";

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

		category.Actions = new List<Action> ();
		category.Attributes = new List<Attribute> ();

		for (int i = 0; i < actions.Count; ++i) 
		{
			category.Actions.Add (actions [i]);
		}

		for(int i = 0; i < attributes.Count; ++i)
		{
			category.Attributes.Add (attributes [i]);
		}

		return category;
	}

	/// <summary>
	/// Readies the item category by adding the attributes and actions it can complete.
	/// </summary>
	public override void ReadyCategory()
	{
		attributes = new List<Attribute> ();

		Attribute waterAttribute = new Attribute (waterContAttrName, WaterContent);
		Attribute toughAttribute = new Attribute (toughAttrName, Toughness);
		Attribute sweetAttribute = new Attribute (sweetAttrName, Sweet);
		Attribute bitterAttribute = new Attribute(bitterAttrName, Bitter);
		Attribute sourAttribute = new Attribute(sourAttrName, Sour);
		Attribute saltyAttribute = new Attribute(saltyAttrName, Salty);
		Attribute spicyAttribute = new Attribute(spicyAttrName, Spicy);

		attributes.Add (waterAttribute);
		attributes.Add (toughAttribute);
		attributes.Add (sweetAttribute);
		attributes.Add (bitterAttribute);
		attributes.Add (sourAttribute);
		attributes.Add (saltyAttribute);
		attributes.Add (spicyAttribute);

		actions = new List<Action> ();

		Action cook = new Action (cookActName, new UnityAction(Cook));
		Action dry = new Action (dryActName, new UnityAction(Dry));

		Action eat = new Action (eatActName, new UnityAction(Eat));
		ItemCondition eatCondition = new ItemCondition (toughAttrName, toughnessEdibleThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Less));

		eat.Conditions.Add (eatCondition);

		actions.Add (cook);
		actions.Add (dry);
		actions.Add (eat);
	}
		
	/// <summary>
	/// Cooks the item. Lowers toughness and increases water content. If the item has high enough water content, it becomes soup.
	/// Otherwise, it is just called Cooked X.
	/// </summary>
	/// <param name="variables">Variables.</param>
	public void Cook()
	{
		Toughness = Toughness * cookingWaterIncreaseRate;
		WaterContent = WaterContent * cookingWaterIncreaseRate;

		GetAttribute (toughAttrName).Value = Toughness;
		GetAttribute (waterContAttrName).Value = WaterContent;

		if (WaterContent > waterContentSoupThreshold && !baseItem.ItemName.Contains (soupNameAddition)) 
		{
			baseItem.ChangeName (baseItem.ItemName + " " + soupNameAddition);
		} 
		else if (WaterContent < waterContentSoupThreshold) 
		{
			baseItem.ChangeName(defaultCookedNameAddition + baseItem.ItemName);
		}

		SetActionComplete (cookActName);
	}

	/// <summary>
	/// Dries this item. Lowers the waterContent greatly and increases toughness.
	/// </summary>
	public void Dry()
	{
		WaterContent = WaterContent * dryingWaterDecreaseRate;
		Toughness = Toughness * dryingToughnessIncreaseRate;

		GetAttribute (waterContAttrName).Value = WaterContent;
		GetAttribute (toughAttrName).Value = Toughness;

		baseItem.ChangeName(defaultDryNameAddition + " " + baseItem.ItemName);

		SetActionComplete(dryActName) = true;
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