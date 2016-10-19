using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CraftingSystem
{
	/// <summary>
	/// The solid item category. Contains attributes and actions that befit a solid category item.
	/// </summary>
	public class SolidCategory : ItemCategory 
	{
		public float flexibility;
		public float durability;
		public float elasticity;
		public float stickiness;
		public float thickness;
		public float sharpness;

		float sharpenLowerThreshold = 0.2f;
		float sharpenUpperThreshold = 0.7f;
		float weaveThreshold = 0.8f;

		/// <summary>
		/// Preps the category for use by loading attributes.
		/// </summary>
		public override void ReadyCategory()
		{
			attributes = new Dictionary<string, float> ();
			actionCompleted = new Dictionary<string, bool> ();

			attributes.Add ("flexibility", flexibility);
			attributes.Add ("durability", durability);
			attributes.Add ("elasticity", elasticity);
			attributes.Add ("stickiness", stickiness);
			attributes.Add ("sharpness", sharpness);
			attributes.Add ("thickness", thickness);
			actionCompleted.Add ("Weave", false);
			actionCompleted.Add ("Sharpen", false);
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
				ActionToPerform sharpen = new ActionToPerform (Sharpen);
				Action act = new Action ("Sharpen", sharpen);
				possibleActions [defaultActionCategoryName].Add (act);
			}

			if (flexibility > weaveThreshold && !actionCompleted["Weave"]) 
			{
				possibleActions.Add("Weave", new List<Action>());

				ActionToPerform wRope = new ActionToPerform (WeaveRope);
				Action weaveRope = new Action ("Weave Rope", wRope);
				possibleActions["Weave"].Add (weaveRope);

				ActionToPerform wBasket = new ActionToPerform (WeaveBasket);
				Action weaveBasket = new Action ("Weave Basket", wBasket);
				possibleActions["Weave"].Add (weaveBasket);
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
			GameObject baseOb = this.gameObject;
			BaseItem baseObComponent = baseOb.GetComponent<BaseItem> ();

			if (thickness > 0.5f) 
			{
				name = baseItem.itemName + " Thread";
			} 
			else 
			{
				name = baseItem.itemName + " Rope";
			}

			baseOb.name =  name;
			baseItem.ChageName(name);
			baseItem.tags.Add ("Rope");
			durability = durability * elasticity / 2f;

			actionCompleted ["Weave"] = true;
			thickness = thickness * 4f;

			baseObComponent.RemoveCategories (new List<string> () {GetType ().Name}, true);
		}

		/// <summary>
		/// A subcategory action of "Weave". Makes the item into a "Container" type object. Removes all other
		/// item categories from it afterwards.
		/// </summary>
		public void WeaveBasket()
		{
			string name;
			GameObject baseOb = this.gameObject;

			name = baseItem.itemName + " Basket";
			durability = durability  * elasticity / 2f;
			thickness = thickness * 2f;

			actionCompleted ["Weave"] = true;

			baseOb.name = name;
			baseItem.ChageName(name);

			baseOb.GetComponent<BaseItem> ().RemoveCategories (new List<string> (){GetType ().Name}, true);
		}

		/// <summary>
		/// Sharpens the item. If it is sharp enough, it becomes a blade.
		/// </summary>
		public void Sharpen()
		{
			if (sharpness > 3f) 
			{
				if (!this.gameObject.name.Contains ("Blade")) 
				{
					this.gameObject.name = this.gameObject.name + " Blade";
					GetComponent<BaseItem> ().ChageName (this.gameObject.name);
				}
			} 
			else 
			{
				this.gameObject.name = "Sharpened " + this.gameObject.name;
				GetComponent<BaseItem> ().ChageName (this.gameObject.name);
			}

			sharpness = sharpness * 1.5f;
			actionCompleted ["Sharpen"] = true;
			attributes ["sharpness"] = sharpness;
		}
	}
}
