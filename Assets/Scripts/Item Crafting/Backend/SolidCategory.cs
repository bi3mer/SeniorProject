using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// The solid item category. Contains attributes and actions that befit a solid category item.
/// </summary>
public class SolidCategory : ItemCategory 
{
	public float Flexibility
	{ 
		get; 
		set; 
	}

	public float Durability
	{ 
		get; 
		set; 
	}

	public float Elasticity
	{ 
		get; 
		set; 
	}

	public float Stickiness
	{ 
		get; 
		set; 
	}

	public float Thickness
	{ 
		get; 
		set; 
	}

	public float Sharpness
	{ 
		get; 
		set; 
	}

	private float sharpenLowerThreshold = 0.2f;
	private float sharpenUpperThreshold = 0.7f;
	private float weaveThreshold = 0.8f;
	private const float sharpenRate = 1.5f;
	private const float threadWidthThreshold = 1f;
	private const float weaveRopeThicknessRate = 4f;
	private const float weaveBasketThicknessRate = 2f;
	private const float bladeNameThreshold = 3f;

	// string added to item name when the resulting rope is thin enough to be a thread
	private const string threadNameAddtion = "Thread";

	// string added to item name when weaving rope
	private const string defaultRopeNameAddition = "Rope";

	// string added to item name by default when item has been sharpened
	private const string defualtSharpenNameAddtion = "Sharpened";

	// string added to item name when item is sharp enough to be a blade
	private const string bladeNameAddition = "Blade";

	private const string basketNameAddition = "Basket";

	private const string flexAttrName = "flexibility";
	private const string duraAttrName = "durability";
	private const string elasAttrName = "elasticity";
	private const string stickiAttrName = "stickiness";
	private const string thickAttrName = "thickness";
	private const string sharpAttrName = "sharpness";

	private const string weaveActionName = "Weave";
	private const string sharpenActionName = "Sharpen";
	private const string weaveRopeActName = "Weave Rope";
	private const string weaveBasketActName = "Weave Basket";

	/// <summary>
	/// Creates a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		SolidCategory category = new SolidCategory ();
		category.Flexibility = Flexibility;
		category.Durability = Durability;
		category.Elasticity = Elasticity;
		category.Stickiness = Stickiness;
		category.Thickness = Thickness;
		category.Sharpness = Sharpness;

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
	/// Preps the category for use by loading attributes.
	/// </summary>
	public override void ReadyCategory()
	{
		attributes = new List<Attribute>();

		Attribute flexibleAttribute = new Attribute (flexAttrName, Flexibility);
		Attribute durableAttribute = new Attribute (duraAttrName, Durability);
		Attribute elasticAttribute = new Attribute (elasAttrName, Elasticity);
		Attribute stickinessAttribute = new Attribute (stickiAttrName, Stickiness);
		Attribute sharpnessAttribute = new Attribute (sharpAttrName, Sharpness);
		Attribute thicknessAttribute = new Attribute (thickAttrName, Thickness);

		attributes.Add (flexibleAttribute);
		attributes.Add (durableAttribute);
		attributes.Add (elasticAttribute);
		attributes.Add (stickinessAttribute);
		attributes.Add (sharpnessAttribute);
		attributes.Add (thicknessAttribute);

		actions = new List<Action> ();

		Action sharpen = new Action (sharpenActionName, new UnityAction (Sharpen));
		ItemCondition sharpenConditionFlex = new ItemCondition (flexAttrName, sharpenLowerThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Less));
		ItemCondition sharpenConditionElas = new ItemCondition (elasAttrName, sharpenLowerThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Less));
		ItemCondition sharpenConditionDura = new ItemCondition (duraAttrName, sharpenUpperThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.More));

		sharpen.Conditions.Add (sharpenConditionFlex);
		sharpen.Conditions.Add (sharpenConditionElas);
		sharpen.Conditions.Add (sharpenConditionDura);

		Action weave = new Action (weaveActionName, null);
		ItemCondition weaveCondition = new ItemCondition (flexAttrName, weaveThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.More));
		weave.Conditions.Add (weaveCondition);

		Action weaveRope = new Action (weaveRopeActName, new UnityAction (WeaveRope));
		Action weaveBasket = new Action (weaveBasketActName, new UnityAction (WeaveBasket));

		weave.SubActions.Add (weaveRope);
		weave.SubActions.Add (weaveBasket);

		actions.Add (sharpen);
		actions.Add (weave);
	}

	/// <summary>
	/// A subcategory action of "Weave". Makes the item into an item of rope type
	/// If the rope is thinner, then the name will be thread, otherwise it will be rope
	/// </summary>
	public void WeaveRope()
	{
		string name;

		if (Thickness > threadWidthThreshold) 
		{
			name = baseItem.ItemName + " " + threadNameAddtion;
		} 
		else 
		{
			name = baseItem.ItemName + " " + defaultRopeNameAddition;
		}
			
		baseItem.ChangeName(name);
		baseItem.Types.Add (Types.Rod);

		// durability of the woven object is determined by averaging the durability and elasticity of the item
		// as tightly weaving something won't work as well with less elastic objects
		Durability = Durability * Elasticity / 2f;

		Thickness = Thickness * weaveRopeThicknessRate;

		baseItem.RemoveCategories (new List<string> () {GetType ().Name}, true);
	}

	/// <summary>
	/// A subcategory action of "Weave". Makes the item into a "Container" type object. Removes all other
	/// item categories from it afterwards.
	/// </summary>
	public void WeaveBasket()
	{
		string name;

		name = baseItem.ItemName + " " + basketNameAddition;

		// durability of the woven object is determined by averaging the durability and elasticity of the item
		// as tightly weaving something won't work as well with less elastic objects
		Durability = Durability  * Elasticity / 2f;
		GetAttribute (duraAttrName).Value = Durability;

		Thickness = Thickness * weaveBasketThicknessRate;
		GetAttribute (thickAttrName).Value = Thickness;

		baseItem.ChangeName(name);

		baseItem.RemoveCategories (new List<string> (){GetType ().Name}, true);
	}

	/// <summary>
	/// Sharpens the item. If it is sharp enough, it becomes a blade.
	/// </summary>
	public void Sharpen()
	{
		if (Sharpness > bladeNameThreshold) 
		{
			if (!baseItem.ItemName.Contains (bladeNameAddition)) 
			{
				baseItem.ChangeName(baseItem.ItemName + " " + bladeNameAddition);
			}
		} 
		else 
		{
			baseItem.ChangeName (defualtSharpenNameAddtion + " " + baseItem.ItemName);
		}

		Sharpness = Sharpness * sharpenRate;
		GetAttribute (sharpAttrName).Value = Sharpness;
	}
}
