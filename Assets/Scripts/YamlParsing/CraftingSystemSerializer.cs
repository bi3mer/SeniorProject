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
    private const string fleshCategoryTag = "flesh";
    private const string medicineCategoryTag = "medicine";
	private const string clothCategoryTag = "cloth";
	private const string containerCategoryTag = "container";
	private const string fuelCategoryTag = "fuel";
	private const string fireBaseCategoryTag = "fireBase";
    private const string shelterCategoryTag = "shelter";
    private const string raftCategoryTag = "raft";
	private const string warmthIdolCategoryTag = "warmthIdol";
	private const string lightCategoryTag = "light";
	private const string equipableTag = "equipable";

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

        categoryNames.Add(fleshCategoryTag);
        categoryTypes.Add(typeof(FleshCategory));

        categoryNames.Add(medicineCategoryTag);
        categoryTypes.Add(typeof(MedicineCategory));
 
		categoryNames.Add(clothCategoryTag);
		categoryTypes.Add(typeof(ClothCategory));

		categoryNames.Add(containerCategoryTag);
		categoryTypes.Add(typeof(ContainerCategory));

		categoryNames.Add(fuelCategoryTag);
		categoryTypes.Add(typeof(FuelCategory));

		categoryNames.Add(fireBaseCategoryTag);
		categoryTypes.Add(typeof(FireBaseCategory));

        categoryNames.Add(shelterCategoryTag);
        categoryTypes.Add(typeof(ShelterCategory));

        categoryNames.Add(raftCategoryTag);
        categoryTypes.Add(typeof(RaftCategory));

		categoryNames.Add(warmthIdolCategoryTag);
		categoryTypes.Add(typeof(WarmthIdolCategory));

		categoryNames.Add(lightCategoryTag);
		categoryTypes.Add(typeof(LightCategory));

		categoryNames.Add (equipableTag);
		categoryTypes.Add (typeof(EquipableCategory));
	}
}
