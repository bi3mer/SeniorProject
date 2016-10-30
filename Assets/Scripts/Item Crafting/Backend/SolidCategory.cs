using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// The solid item category. Contains attributes and actions that befit a solid category item.
/// </summary>
public class SolidCategory : ItemCategory 
{
	public float flexibility { get; set; }
	public float durability { get; set; }
	public float elasticity { get; set; }
	public float stickiness { get; set; }
	public float thickness { get; set; }
	public float sharpness { get; set; }

	float sharpenLowerThreshold = 0.2f;
	float sharpenUpperThreshold = 0.7f;
	float weaveThreshold = 0.8f;

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

	public override ItemCategory GetDuplicate()
	{
		SolidCategory category = new SolidCategory ();
		category.flexibility = flexibility;
		category.durability = durability;
		category.elasticity = elasticity;
		category.stickiness = stickiness;
		category.thickness = thickness;
		category.sharpness = sharpness;

		category.actionCompleted = actionCompleted;

		return category;
	}

	/// <summary>
	/// Preps the category for use by loading attributes.
	/// </summary>
	public override void ReadyCategory()
	{
		attributes = new Dictionary<string, float> ();

		if (actionCompleted == null) 
		{
			actionCompleted = new Dictionary<string, bool> ();
			actionCompleted.Add (weaveActionName, false);
			actionCompleted.Add (sharpenActionName, false);
		}

		attributes.Add (flexAttrName, flexibility);
		attributes.Add (duraAttrName, durability);
		attributes.Add (elasAttrName, elasticity);
		attributes.Add (stickiAttrName, stickiness);
		attributes.Add (sharpAttrName, sharpness);
		attributes.Add (thickAttrName, thickness);
	}

	/// <summary>
	/// Gets all the action that an item can perform. This changes depending on what actions have already been performed.
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.</returns>
	public override Dictionary<string, List<Action>> GetPossibleActions()
	{
		Dictionary<string, List<Action>> possibleActions = new Dictionary<string, List<Action>> ();
	
		if (!possibleActions.ContainsKey (defaultActionCategoryName)) 
		{
			possibleActions.Add (defaultActionCategoryName, new List<Action> ());
		}

		// gets the actions of the component its linked to
		// the linked component does the same until all item category components in the gameobject have added their possible actions
		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}

		if (flexibility < sharpenLowerThreshold && elasticity < sharpenLowerThreshold && durability > sharpenUpperThreshold && !actionCompleted["Sharpen"]) 
		{
			UnityAction sharpen = new UnityAction (Sharpen);
			Action act = new Action (weaveActionName, sharpen);
			possibleActions [defaultActionCategoryName].Add (act);
		}

		if (flexibility > weaveThreshold && !actionCompleted[weaveActionName]) 
		{
			possibleActions.Add(weaveActionName, new List<Action>());

			UnityAction wRope = new UnityAction (WeaveRope);
			Action weaveRope = new Action (weaveRopeActName, wRope);
			possibleActions[weaveActionName].Add (weaveRope);

			UnityAction wBasket = new UnityAction (WeaveBasket);
			Action weaveBasket = new Action (weaveBasketActName, wBasket);
			possibleActions[weaveActionName].Add (weaveBasket);
		}

		return possibleActions;
	}

	/// <summary>
	/// A subcategory action of "Weave". Makes the item into an item of rope type
	/// If the rope is thinner, then the name will be thread, otherwise it will be rope
	/// </summary>
	public void WeaveRope()
	{
		string name;

		if (thickness > 0.5f) 
		{
			name = baseItem.itemName + " Thread";
		} 
		else 
		{
			name = baseItem.itemName + " Rope";
		}
			
		baseItem.ChangeName(name);
		baseItem.tags.Add ("Rope");
		durability = durability * elasticity / 2f;

		actionCompleted [weaveActionName] = true;
		thickness = thickness * 4f;

		baseItem.RemoveCategories (new List<string> () {GetType ().Name}, true);
		baseItem.AddToModifactionList (weaveRopeActName);
	}

	/// <summary>
	/// A subcategory action of "Weave". Makes the item into a "Container" type object. Removes all other
	/// item categories from it afterwards.
	/// </summary>
	public void WeaveBasket()
	{
		string name;

		name = baseItem.itemName + " Basket";
		durability = durability  * elasticity / 2f;
		thickness = thickness * 2f;

		actionCompleted [weaveActionName] = true;

		baseItem.ChangeName(name);

		baseItem.RemoveCategories (new List<string> (){GetType ().Name}, true);
		baseItem.AddToModifactionList (weaveBasketActName);
	}

	/// <summary>
	/// Sharpens the item. If it is sharp enough, it becomes a blade.
	/// </summary>
	public void Sharpen()
	{
		if (sharpness > 3f) 
		{
			if (!baseItem.itemName.Contains ("Blade")) 
			{
				baseItem.ChangeName(baseItem.itemName + " Blade");
			}
		} 
		else 
		{
			baseItem.ChangeName ("Sharpened " + baseItem.itemName);
		}

		sharpness = sharpness * 1.5f;
		actionCompleted [sharpenActionName] = true;
		attributes [sharpAttrName] = sharpness;
	}
}
