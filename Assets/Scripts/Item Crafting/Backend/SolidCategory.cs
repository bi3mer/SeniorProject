using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// The solid item category. Contains attributes and actions that befit a solid category item.
/// </summary>
public class SolidCategory : ItemCategory 
{

	/// <summary>
	/// Gets or sets the flexibility.
	/// </summary>
	/// <value>The flexibility.</value>
	public float Flexibility
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the durability.
	/// </summary>
	/// <value>The durability.</value>
	public float Durability
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the elasticity.
	/// </summary>
	/// <value>The elasticity.</value>
	public float Elasticity
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the stickiness.
	/// </summary>
	/// <value>The stickiness.</value>
	public float Stickiness
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the thickness.
	/// </summary>
	/// <value>The thickness.</value>
	public float Thickness
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the sharpness.
	/// </summary>
	/// <value>The sharpness.</value>
	public float Sharpness
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// The thresholds for Sharpen to be a possible action
	/// </summary>
	private float sharpenLowerThreshold = 0.2f;
	private float sharpenUpperThreshold = 2.5f;

	/// <summary>
	/// The threshold for whether or not Weave is a possible action
	/// </summary>
	private float weaveThreshold = 3f;

	/// <summary>
	/// Amount that sharpness will be increased by when Sharpen is executed
	/// </summary>
	private const float sharpenRate = 1.5f;

	/// <summary>
	/// Threshold value of Thickness, under which the item will be considered a thread, and not rope
	/// </summary>
	private const float threadWidthThreshold = 1f;

	/// <summary>
	/// How much the Thickness will increase by when weaving rope
	/// </summary>
	private const float weaveRopeThicknessRate = 4f;

	/// <summary>
	/// How much the Thickness will increase by when weaving basket
	/// </summary>
	private const float weaveBasketThicknessRate = 2f;

	/// <summary>
	/// Threshold value of Sharpess, over which the item will be considered a blade
	/// </summary>
	private const float sharpTypeThreshold = 3f;

	/// <summary>
	/// string added to item name when the resulting rope is thin enough to be a thread
	/// </summary>
	private const string threadNameAddtion = "Thread";

	/// <summary>
	/// string added to item name when weaving rope
	/// </summary>
	private const string defaultRopeNameAddition = "Rope";

	/// <summary>
	/// string added to item name by default when item has been sharpened
	/// </summary>
	private const string defaultSharpenNameAddtion = "Filed";

	private const string sharpenTypeNameAddition = "Sharpened";

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

		category.Actions = new List<ItemAction> ();
		category.Attributes = new List<Attribute> ();

		ItemAction sharpen = new ItemAction (sharpenActionName, new UnityAction (category.Sharpen));
		ItemAction weave = new ItemAction (weaveActionName, null);
		ItemAction weaveRope = new ItemAction (weaveRopeActName, new UnityAction (category.WeaveRope));
		ItemAction weaveBasket = new ItemAction (weaveBasketActName, new UnityAction (category.WeaveBasket));

		weave.SubActions.Add (weaveRope);
		weave.SubActions.Add (weaveBasket);

		category.Actions.Add (sharpen);
		category.Actions.Add (weave);

		finishDuplication(category);
		return category;
	}

	/// <summary>
	/// Preps the category for use by loading attributes.
	/// </summary>
	public override void ReadyCategory()
	{
		Attributes = new List<Attribute>();

		Attributes.Add (new Attribute (flexAttrName, Flexibility));
		Attributes.Add (new Attribute (duraAttrName, Durability));
		Attributes.Add (new Attribute (elasAttrName, Elasticity));
		Attributes.Add (new Attribute (stickiAttrName, Stickiness));
		Attributes.Add (new Attribute (sharpAttrName, Sharpness));
		Attributes.Add (new Attribute (thickAttrName, Thickness));

		Actions = new List<ItemAction> ();

		ItemAction sharpen = new ItemAction (sharpenActionName, new UnityAction (Sharpen));
		ItemCondition sharpenConditionFlex = new ItemCondition (flexAttrName, sharpenLowerThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Less));
		ItemCondition sharpenConditionElas = new ItemCondition (elasAttrName, sharpenLowerThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Less));
		ItemCondition sharpenConditionDura = new ItemCondition (duraAttrName, sharpenUpperThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Greater));

		sharpen.Conditions.Add (sharpenConditionFlex);
		sharpen.Conditions.Add (sharpenConditionElas);
		sharpen.Conditions.Add (sharpenConditionDura);

		ItemAction weave = new ItemAction (weaveActionName, null);
		ItemCondition weaveConditionFlex = new ItemCondition (flexAttrName, weaveThreshold, new BooleanOperator.BooleanOperatorDelegate (BooleanOperator.Greater));
		weave.Conditions.Add (weaveConditionFlex);

		ItemAction weaveRope = new ItemAction (weaveRopeActName, new UnityAction (WeaveRope));
		ItemAction weaveBasket = new ItemAction (weaveBasketActName, new UnityAction (WeaveBasket));

		weave.SubActions.Add (weaveRope);
		weave.SubActions.Add (weaveBasket);

		Actions.Add (sharpen);
		Actions.Add (weave);
	}

	/// <summary>
	/// A subcategory action of "Weave". Makes the item into an item of rope type
	/// If the rope is thinner, then the name will be thread, otherwise it will be rope
	/// </summary>
	public void WeaveRope()
	{
		string name;

		if (Thickness < threadWidthThreshold) 
		{
			name = baseItem.ItemName + " " + threadNameAddtion;
		} 
		else 
		{
			name = baseItem.ItemName + " " + defaultRopeNameAddition;
		}

		// durability of the woven object is determined by averaging the durability and elasticity of the item
		// as tightly weaving something won't work as well with less elastic objects
		Durability = Durability * Elasticity / 2f;

		Thickness = Thickness * weaveRopeThicknessRate;

		baseItem.ChangeName(name);
		baseItem.Types.Add (ItemTypes.Rope);

		int newModelIndex = baseItem.ModifyingActionNames.IndexOf(weaveRopeActName);
		baseItem.SetNewModel(newModelIndex);

		baseItem.RemoveCategoriesExcluding (new List<string> () {GetType ().Name});
		baseItem.DirtyFlag = true;
		SetActionComplete(weaveActionName);
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
		Durability = (Durability  + Elasticity) / 2f;
		GetAttribute (duraAttrName).Value = Durability;

		Thickness = Thickness * weaveBasketThicknessRate;
		GetAttribute (thickAttrName).Value = Thickness;

		baseItem.ChangeName(name);
		baseItem.Types.Add(ItemTypes.Container);

		int newModelIndex = baseItem.ModifyingActionNames.IndexOf(weaveBasketActName);

		baseItem.SetNewModel(newModelIndex);

		baseItem.RemoveCategoriesExcluding (new List<string> (){GetType ().Name});

		ContainerCategory container = new ContainerCategory();

		// TODO: Determine a way to decide container size
		container.Size = 1;
		baseItem.AddItemCategory(container);

		baseItem.DirtyFlag = true;
		// setting the parent action as complete marks all subactions as complete as well
		SetActionComplete(weaveActionName);
	}

	/// <summary>
	/// Sharpens the item. If it is sharp enough, it becomes a blade.
	/// </summary>
	public void Sharpen()
	{
		Sharpness = Sharpness * sharpenRate + Durability;
		GetAttribute (sharpAttrName).Value = Sharpness;

		if(Sharpness > sharpTypeThreshold)
		{
			baseItem.Types.Add(ItemTypes.Sharp);
			baseItem.ChangeName (sharpenTypeNameAddition + " " + baseItem.ItemName);
		}
		else
		{
			baseItem.ChangeName (defaultSharpenNameAddtion + " " + baseItem.ItemName);
		}

		baseItem.DirtyFlag = true;
		SetActionComplete(sharpenActionName);
	}
}
