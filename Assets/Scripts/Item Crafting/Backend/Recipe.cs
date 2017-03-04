using System.Collections.Generic;

/// <summary>
///  Class that handles storing the contents of recipes and checking whether or not the requirements are met.
/// </summary>

public class Recipe  
{
	/// <summary>
	/// list of resource requirements for a recipe. Items under this category are used up during crafting.
	/// </summary>
	/// <value>The requirements.</value>
	public List<Requirement> ResourceRequirements
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// List of tool requirements for a recipe. Items under this category are not used up during crafting.
	/// </summary>
	/// <value>The tool requirement.</value>
	public List<Requirement> ToolRequirements
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Recipe"/> is given a tier when crafted.
	/// </summary>
	/// <value><c>true</c> if tiered; otherwise, <c>false</c>.</value>
	public bool Tiered
	{
		get;
		set;
	}

	/// <summary>
	/// name of the recipe, generally the name of the item that it will create
	/// </summary>
	/// <value>The name of the recipe.</value>
	public string RecipeName
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the stats to check when determing the quality of the crafted item
	/// </summary>
	/// <value>The stats to check.</value>
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
		ResourceRequirements.Add (req);
	}

	/// <summary>
	/// Checks to see if the recipe is completely fulfilled.
	/// </summary>
	/// <returns><c>true</c>, if all requirements completed, <c>false</c> otherwise.</returns>
	public bool CheckCompleted()
	{
		for (int i = 0; i < ResourceRequirements.Count; ++i) 
		{
			if (!ResourceRequirements [i].isFullfilled ()) {
				return false;
			}	
		}

		return true;
	}
}
