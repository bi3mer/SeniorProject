using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Contains attributes and actions that befit a plant category item.
/// </summary>
public class PlantCategory : ItemCategory 
{
	/// <summary>
	/// Gets or sets the water content of the item.
	/// </summary>
	/// <value>The content of the water.</value>
	public float WaterContent 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the toughness.
	/// </summary>
	/// <value>The toughness.</value>
	public float Toughness 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the sweetness.
	/// </summary>
	/// <value>The sweet.</value>
	public float Sweet 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the bitterness.
	/// </summary>
	/// <value>The bitter.</value>
	public float Bitter 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the sournss.
	/// </summary>
	/// <value>The sour.</value>
	public float Sour 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the saltiness.
	/// </summary>
	/// <value>The salty.</value>
	public float Salty 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the spiciness.
	/// </summary>
	/// <value>The spicy.</value>
	public float Spicy 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the stomach effect.
	/// </summary>
	/// <value>The stomach effect.</value>
	public float StomachEffect 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the pneumonia effect.
	/// </summary>
	/// <value>The pneumonia effect.</value>
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

		category.Actions = new List<ItemAction> ();
		category.Attributes = new List<Attribute> ();

		ItemAction cook = new ItemAction (cookActName, new UnityAction(category.Cook));
		ItemAction dry = new ItemAction (dryActName, new UnityAction(category.Dry));
		ItemAction eat = new ItemAction (eatActName, new UnityAction(category.Eat));


		// the actions must be added in the same order as they were in the original copy of the category
		// unable to pass along UnityAction delegate, as that will continue to point to the original copy of the item category
		category.Actions.Add (cook);
		category.Actions.Add (dry);
		category.Actions.Add (eat);

		finishDuplication(category);

		return category;
	}

	/// <summary>
	/// Readies the item category by adding the attributes and actions it can complete.
	/// </summary>
	public override void ReadyCategory()
	{
		Attributes = new List<Attribute> ();

		Attributes.Add (new Attribute (waterContAttrName, WaterContent));
		Attributes.Add (new Attribute (toughAttrName, Toughness));
		Attributes.Add (new Attribute (sweetAttrName, Sweet));
		Attributes.Add (new Attribute(bitterAttrName, Bitter));
		Attributes.Add (new Attribute(sourAttrName, Sour));
		Attributes.Add (new Attribute(saltyAttrName, Salty));
		Attributes.Add (new Attribute(spicyAttrName, Spicy));

		Actions = new List<ItemAction> ();

		ItemAction cook = new ItemAction (cookActName, new UnityAction(Cook));
		ItemAction dry = new ItemAction (dryActName, new UnityAction(Dry));

		ItemAction eat = new ItemAction (eatActName, new UnityAction(Eat));
		ItemCondition eatCondition = new ItemCondition (toughAttrName, toughnessEdibleThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Less));

		eat.Conditions.Add (eatCondition);

		Actions.Add (cook);
		Actions.Add (dry);
		Actions.Add (eat);
	}
		
	/// <summary>
	/// Cooks the item. Lowers toughness and increases water content. If the item 
	///has high enough water content, it becomes soup. Otherwise, it becomes Cooked X.
	/// </summary>
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

		baseItem.DirtyFlag = true;
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

		baseItem.DirtyFlag = true;
		SetActionComplete (dryActName);
	}

	/// <summary>
	/// Consumes the item. May cause illness.
	/// </summary>
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

		baseItem.RemovalFlag = true;
	}
}