using System.Collections.Generic;

namespace CraftingSystem
{
	/// <summary>
	///  Class that handles storing the contents of recipes and checking whether or not they are met.
	/// </summary>

	public class Recipe  
	{
		// list of requirements for a recipe
		List<Requirement> requirements;
		// name of the recipe, generally the name of the item that it will create
		string recipeName;

		/// <summary>
		/// Initializes a new instance of the <see cref="Recipe"/> class. Takes in the name of the item that
		/// the recipe will create and uses that as the name of the recipe. Instantiates the list of
		/// requirements.
		/// </summary>
		/// <param name="name">Name of recipe.</param>
		public Recipe(string name)
		{
			recipeName = name;
			requirements = new List<Requirement> ();
		}

		/// <summary>
		/// Adds a requirement to the list of requirements.
		/// </summary>
		/// <param name="req">Req.</param>
		public void AddRequirement(Requirement req)
		{
			requirements.Add (req);
		}

		/// <summary>
		/// Gets the list of requirements.
		/// </summary>
		/// <returns>The requirements.</returns>
		public List<Requirement> GetRequirements()
		{
			return requirements;
		}

		/// <summary>
		/// Checks to see if the recipe is completely fulfilled.
		/// </summary>
		/// <returns><c>true</c>, if all requirements completed, <c>false</c> otherwise.</returns>
		public bool CheckCompleted()
		{
			for (int i = 0; i < requirements.Count; ++i) 
			{
				if (!requirements [i].isFullfilled ()) {
					return false;
				}	
			}

			return true;
		}

		/// <summary>
		/// Gets the name of the recipe.
		/// </summary>
		/// <returns>The recipe name.</returns>
		public string GetRecipeName()
		{
			return recipeName;
		}
	}
}
