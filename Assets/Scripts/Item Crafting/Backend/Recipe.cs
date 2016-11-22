﻿using System.Collections.Generic;

/// <summary>
///  Class that handles storing the contents of recipes and checking whether or not the requirements are met.
/// </summary>

public class Recipe  
{
	/// <summary>
	/// list of requirements for a recipe
	/// </summary>
	/// <value>The requirements.</value>
	public List<Requirement> Requirements
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
