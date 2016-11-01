using System.Collections.Generic;

/// <summary>
///  Class that handles storing the contents of recipes and checking whether or not they are met.
/// </summary>

public class Recipe  
{
	// list of requirements for a recipe
	public List<Requirement> Requirements
	{ 
		get; 
		set; 
	}

	// name of the recipe, generally the name of the item that it will create
	public string RecipeName
	{ 
		get; 
		set; 
	}

	public List<CraftingStat> StatsToCheck
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Recipe"/> class. Used by the yaml deserializer.
	/// </summary>
	public Recipe()
	{
	}

	/// <summary>
	/// Adds a requirement to the list of requirements.
	/// </summary>
	/// <param name="req">Req.</param>
	public void AddRequirement(Requirement req)
	{
		Requirements.Add (req);
	}

	/// <summary>
	/// Checks to see if the recipe is completely fulfilled.
	/// </summary>
	/// <returns><c>true</c>, if all requirements completed, <c>false</c> otherwise.</returns>
	public bool CheckCompleted()
	{
		for (int i = 0; i < Requirements.Count; ++i) 
		{
			if (!Requirements [i].isFullfilled ()) {
				return false;
			}	
		}

		return true;
	}
}
