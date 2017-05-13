using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RooftopGeneration: ItemGenerator
{
	[SerializeField]
    [Tooltip("Likelihood of a rooftop having items, from 0 to 1")]
	[Range(0, 1)]
    private float itemGenerationChance;

	[SerializeField]
    [Tooltip("Likelihood of a rooftop having a door, from 0 to 1")]
	[Range(0, 1)]
    private float doorGenerationChance;

    [SerializeField]
    [Tooltip("Likelihood of an item being a note, from 0 to 1")]
    [Range(0, 1)]
    private float noteGenerationChance;

    [SerializeField]
    [Tooltip("Likelihood of a rooftop having a shelter, from 0 to 1")]
    [Range(0, 1)]
    private float shelterGenerationChance;

	/// <summary>
	/// Generator that creates the points to place objects
	/// </summary>
	private RooftopPointGenerator generator;

	private bool noteGenerate;

	/// <summary>
	/// Awakens this instance.
	/// </summary>
	void Awake()
	{
		generator = new RooftopPointGenerator ();
		districtItemInfo = new Dictionary<string, DistrictItemConfiguration>();
		Dictionary<string, List<GameObject>> itemTemplates = Game.Instance.WorldItemFactoryInstance.GetAllInteractableItemsByDistrict(false, false);

		// get district name here
		foreach(string key in itemTemplates.Keys)
		{
			districtItemInfo.Add(key, new DistrictItemConfiguration());
			districtItemInfo[key].ItemTemplates = itemTemplates[key];
			districtItemInfo[key].ItemExtents = GetItemExtents(itemTemplates[key]);
			districtItemInfo[key].ItemNames = Game.Instance.ItemFactoryInstance.LandItemsByDistrict[key];
		}

		noteGenerate = false;
	}

	/// <summary>
	/// Adds the doors to district.
	/// </summary>
	/// <param name="doors">Doors.</param>
	/// <param name="doorExtents">Door extents.</param>
	/// <param name="district">District.</param>
	public void AddDoorsToDistrict(List<GameObject> doors, List<float> doorExtents, string district)
	{
		districtItemInfo[district].DoorExtents = doorExtents;
		districtItemInfo[district].DoorTemplates = doors;
	}

	/// <summary>
	/// Adds the shelters to district.
	/// </summary>
	/// <param name="shelters">Shelters.</param>
	/// <param name="shelterExtents">Shelter extents.</param>
	/// <param name="district">District.</param>
	public void AddSheltersToDistrict(List<GameObject> shelters, List<float> shelterExtents, string district)
	{
		districtItemInfo[district].ShelterExtents = shelterExtents;
		districtItemInfo[district].ShelterTemplates = shelters;
	}

	/// <summary>
	/// Populates the rooftop of a building.
	/// </summary>
	/// <param name="building">Building information.</param>
	/// <param name="district">District.</param>
	public void PopulateRoof(Building building, string district)
	{
		float itemChance;
		float doorChance;
		float shelterChance;

		List<ItemPlacementSamplePoint> points;

		// check to see if this building will have objects on its roof
		itemChance = Random.value;
		doorChance = Random.value;
		shelterChance = Random.value;

		bool hasDoor = false;
		bool hasShelter = false;

		if (itemChance < itemGenerationChance) 
		{
			if(doorChance < doorGenerationChance && districtItemInfo[district].DoorTemplates.Count > 0)
			{
				hasDoor = true;
			}
			else if(shelterChance < shelterGenerationChance && districtItemInfo[district].ShelterTemplates.Count > 0)
			{
				hasShelter = true;
			}

			points = generator.GetValidPoints (building.BoundingBox, building.Position, districtItemInfo[district], district, hasDoor, hasShelter);

			if (points.Count > 0) 
			{
				// for now, all roofs with items will have doors
				generateObjects(district, points, building);
			}
		}
	}

	/// <summary>
	/// Generates the objects that needs to be placed on the building's surface.
	/// </summary>
	/// <param name="district">Name of the district for which generation is occuring.</param>
	/// <param name="points">Points.</param>
	/// <param name="currentBuilding">Current Building GameObject.</param>
	private void generateObjects(string district, List<ItemPlacementSamplePoint> points, Building currentBuilding)
	{
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;
		List<GameObject> attachments = new List<GameObject>();
		List<ItemPlacementSamplePoint> attachmentInformation = new List<ItemPlacementSamplePoint>();

		noteGenerate = false;

		for (int i = 0; i < points.Count; ++i) 
		{
			// a note is generated if there are notes still available to generate and the
			// random number falls under the noteGenerationChance
			noteGenerate = (Game.Instance.NoteFactoryInstance.NotesAvailable && 
							RandomUtility.RandomPercent <= noteGenerationChance);

			if(points[i].Type == ItemPlacementSamplePoint.PointType.DOOR)
			{
				attachments.Add(districtItemInfo[district].DoorTemplates[points[i].ItemIndex]);
				attachmentInformation.Add(points[i]);
				noteGenerate = false;
			}
			else if(points[i].Type == ItemPlacementSamplePoint.PointType.SHELTER)
			{
				attachments.Add(districtItemInfo[district].ShelterTemplates[points[i].ItemIndex]);
				attachmentInformation.Add(points[i]);
				noteGenerate = false;
			}

			if (noteGenerate) 
			{
				GameObject note = Game.Instance.NoteFactoryInstance.GetNextNote ();
				poolManager.AddToGrid (points [i].WorldSpaceLocation, note.name, false);
				poolManager.AddItemToPool (note.name, note);
			} 
			else if(points[i].ItemIndex < districtItemInfo[points[i].District].ItemNames.Count)
			{
				poolManager.AddToGrid(points[i].WorldSpaceLocation, districtItemInfo[points[i].District].ItemNames[points[i].ItemIndex], false);
			}
		}

		currentBuilding.Attachments = attachments;
		currentBuilding.AttachmentInformation = attachmentInformation;
	}
}
