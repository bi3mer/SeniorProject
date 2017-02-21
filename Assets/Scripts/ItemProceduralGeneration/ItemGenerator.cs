using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemGenerator : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The Item Pool Manager")]
	protected ItemPoolManager poolManager;

	/// <summary>
	/// The item templates used to create the objects in the world
	/// </summary>
	protected Dictionary<string, DistrictItemConfiguration> districtItemInfo;

	/// <summary>
	/// Gets the rarity information for all items in a district.
	/// </summary>
	/// <returns>The rarity information.</returns>
	/// <param name="district">District that the items belong to.</param>
	protected List<float> getRarityInformation(string district)
	{
		ItemFactory itemFactory = Game.Instance.ItemFactoryInstance;
		List<string> itemsInDistrict = itemFactory.LandItemsByDistrict[district];
		List<float> rarityValues = new List<float>();

		for(int i = 0; i < itemsInDistrict.Count; ++i)
		{
			rarityValues.Add(ItemRarity.GetRarity(itemFactory.GetBaseItem(itemsInDistrict[i]).Rarity));
		}

		return rarityValues;
	}

	/// <summary>
	/// Returns the extents of each gameobject. Used during sampling point generation to take into account the extents of the objet
	/// when dealing with minDistance. Only needs the extents of the objects, so only half the size. Assuming that pivot is in center.
	/// </summary>
	/// <returns>The extents of the items.</returns>
	/// <param name="items">Items.</param>
	public List<float> GetItemExtents(List<GameObject> items)
	{
		List<float> extents = new List<float>();
		Bounds itemBound;

		for(int i = 0; i < items.Count; ++i)
		{
			itemBound = calculateBounds(items[i]);
			extents.Add(Mathf.Max(itemBound.size.x, itemBound.size.z));
		}

		return extents;
	}

	/// <summary>
	/// Calculates the bounds of a given gameobject.
	/// </summary>
	/// <returns>The bounds.</returns>
	/// <param name="item">Gameobject to check.</param>
	protected Bounds calculateBounds(GameObject item)
	{
		Renderer renderer = item.GetComponent<Renderer>();

		if(renderer != null)
		{
			// the extents of the item for the purposes of the procedural generation is half of either the depth or width
			// this number will be used to determine how far the minDistance between points must be to avoid objects overlapping
			// since the pivots are generally in the center, the size is halved
			return renderer.bounds;
		}
		else
		{
			MeshRenderer[] meshes = item.GetComponentsInChildren<MeshRenderer>();

			if(meshes.Length > 0)
			{
				Bounds combinedBounds = meshes[0].bounds;

				/// the first mesh is already a part of combinedMesh, so go to the second
				for(int j = 1; j < meshes.Length; ++j)
				{
					combinedBounds.Encapsulate(meshes[j].bounds);
				}

				return combinedBounds;
			}
		}

		return new Bounds();
	}

	/// <summary>
	/// Sets the seed.
	/// </summary>
	/// <param name="newSeed">Seed.</param>
	public void SetSeed(int newSeed)
	{
		Random.InitState(newSeed);
	}

	public void AddTemplatesToItemPool()
	{
		foreach(string key in districtItemInfo.Keys)
		{
			for(int i = 0; i < districtItemInfo[key].ItemTemplates.Count; ++i)
			{
				poolManager.AddItemToPool(districtItemInfo[key].ItemNames[i], districtItemInfo[key].ItemTemplates[i]);
			}
		}
	}
}
