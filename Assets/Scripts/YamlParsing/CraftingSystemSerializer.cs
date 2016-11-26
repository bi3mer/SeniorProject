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

	// the possible categories that can be added to a baseItem
	// categoryNames stores name of the category
	// categoryTypes stores the type
	protected List<string> categoryNames;
	protected List<Type> categoryTypes;

	// tag:yaml.org,2002 is shorthanded as "!" in the yaml file, but when registering the tag, it
	// is necessary to use the full Uri
	protected string uriPrefix = "tag:yaml.org,2002:";

	/// <summary>
	/// Fills out the categoryNames and cateogryTypes lists with the necessary information
	/// </summary>
	public void SetUpCategoryInformation()
	{
		categoryNames.Add ("plant");
		categoryTypes.Add (typeof(PlantCategory));

		categoryNames.Add ("solid");
		categoryTypes.Add (typeof(SolidCategory));
	}
}
