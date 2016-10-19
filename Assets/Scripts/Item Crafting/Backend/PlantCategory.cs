using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CraftingSystem
{
	/// <summary>
	/// Contains attributes and actions that befit a plant category item.
	/// </summary>
	public class PlantCategory : ItemCategory 
	{
		public float waterContent;
		public float toughness;
		public float sweet;
		public float bitter;
		public float sour;
		public float salty;
		public float spicy;
		public float stomachEffect;
		public float pneumoniaEffect;

		/// <summary>
		/// Readies the item category by adding the attributes and actions it can complete.
		/// </summary>
		public override void ReadyCategory()
		{
			attributes = new Dictionary<string, float> ();
			actionCompleted = new Dictionary<string, bool> ();

			attributes.Add ("waterContent", waterContent);
			attributes.Add ("toughness", toughness);
			attributes.Add ("sweet", sweet);
			attributes.Add ("bitter", bitter);
			attributes.Add ("sour", sour);
			attributes.Add ("salty", salty);
			attributes.Add ("spicy", spicy);

			actionCompleted.Add ("Dry", false);
			actionCompleted.Add ("Cook", false);
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

			if (!actionCompleted ["Cook"]) 
			{
				ActionToPerform cookAct = new ActionToPerform (Cook);
				Action cook = new Action ("Cook", cookAct);
				possibleActions [defaultActionCategoryName].Add (cook);
			}
		
			if (!actionCompleted ["Dry"]) 
			{
				ActionToPerform dryAct = new ActionToPerform (Dry);
				Action dry = new Action ("Dry", dryAct);
				possibleActions[defaultActionCategoryName].Add (dry);
			}

			if (toughness < 3) 
			{
				ActionToPerform eatAct = new ActionToPerform (Eat);
				Action eat = new Action ("Eat", eatAct);
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

			attributes ["toughness"] = toughness;
			attributes ["waterContent"] = waterContent;

			if (waterContent > 6f && !gameObject.name.Contains ("Soup")) 
			{
				gameObject.name = gameObject.name + " Soup";
				GetComponent<BaseItem> ().ChageName (gameObject.name);
			} 
			else if (waterContent < 6f) 
			{
				gameObject.name = "Cooked " + gameObject.name;
				GetComponent<BaseItem> ().ChageName (gameObject.name);
			}

			actionCompleted ["Cook"] = true;

			if (toughness >= 3f) 
			{
				GetComponent<BaseItem> ().RemoveCategories (new List<string> (){ "SolidAttribute" });
			}
		}

		/// <summary>
		/// Dries this item. Lowers the waterContent greatly and increases toughness.
		/// </summary>
		public void Dry()
		{
			waterContent = waterContent / 4f;
			toughness = toughness * 1.5f;

			attributes ["waterContent"] = waterContent;
			attributes ["toughness"] = toughness;

			gameObject.name = "Dried " + gameObject.name;
			GetComponent<BaseItem> ().ChageName (gameObject.name);

			actionCompleted ["Dry"] = true;
		}

		/// <summary>
		/// Consumes the item. May cause illness.
		/// </summary>
		/// <param name="variables">Variables.</param>
		public void Eat()
		{
			if (stomachEffect < 0) 
			{
				Debug.Log ("You're gonna barf");
			} 
			else if (stomachEffect > 0) 
			{
				Debug.Log ("You're gonna stop barfing");
			}	

			if (pneumoniaEffect < 0) 
			{
				Debug.Log ("You're gonna get pneumonia bub.");
			} 
			else if (pneumoniaEffect > 0) 
			{
				Debug.Log ("You're gonna get less feverish whoo");
			}

			ItemInfoPanelBehavior.instance.CloseItemInfo ();
			GameObject.Destroy (this.gameObject);
		}
	}
}