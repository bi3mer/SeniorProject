using System.Collections;
using System.Collections.Generic;
using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

public abstract class CraftingSystemSerializer
{
	public string Filename;

	/// <summary>
	/// the possible categories that can be added to a baseItem
	/// categoryNames stores name of the category
	/// categoryTypes stores the type
	/// </summary>
	protected List<string> categoryNames;
	protected List<Type> categoryTypes;

	/// <summary>
	/// The category tags as they appear in the yaml file
	/// </summary>
	private const string plantCategoryTag = "plant";
	private const string solidCategoryTag = "solid";
	private const string fishingRodCategoryTag = "fishingRod";

	/// <summary>
	/// tag:yaml.org,2002 is shorthanded as "!" in the yaml file, but when registering the tag, it
	/// is necessary to use the full Uri
	/// </summary>
	protected string uriPrefix = "tag:yaml.org,2002:";

	/// <summary>
	/// Fills out the categoryNames and cateogryTypes lists with the necessary information
	/// </summary>
	public void SetUpCategoryInformation()
	{
		categoryNames.Add (plantCategoryTag);
		categoryTypes.Add (typeof(PlantCategory));

		categoryNames.Add (solidCategoryTag);
		categoryTypes.Add (typeof(SolidCategory));

		categoryNames.Add(fishingRodCategoryTag);
		categoryTypes.Add(typeof(FishingRodCategory));
	}
}
