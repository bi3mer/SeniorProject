using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldItemFactory
{
	private Dictionary<string, GameObject> worldItemTemplates;

	/// <summary>
	/// The location of the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private const string triggerObjectLocation = "ItemGeneration/InteractableText";

	/// <summary>
	/// The name of the item file. All yaml files must be placed under "Resources/YAMLFiles"
	/// </summary>
	private const string itemFileName = "ItemListYaml.yml";

	/// <summary>
	/// The name of the layer that items generated to the world by this factory should be.
	/// </summary>
	private const string layerName = "PassInteractable";

	/// <summary>
	/// The name of the tag that items generated to the world by this factory should have.
	/// </summary>
	private const string tagName = "Interactable";

	/// <summary>
	/// the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private GameObject triggerObjectPrefab;

	private RangeAttribute itemAmountRange = new RangeAttribute(1, 10);

	/// <summary>
	/// Initializes a new instance of the <see cref="WorldItemFactory"/> class.
	/// </summary>
	public WorldItemFactory()
	{
		worldItemTemplates = new Dictionary<string, GameObject>();
		triggerObjectPrefab = (GameObject) Resources.Load(triggerObjectLocation);
		LoadTemplates();
	}

	/// <summary>
	/// Loads the item templates.
	/// </summary>
	public void LoadTemplates()
	{
		// I only need this to handle the loading the item list in, so there's no need to load in the generatable items list
		ItemSerializer itemParser = new ItemSerializer(itemFileName, "");

		Dictionary<string, BaseItem> itemDatabase = itemParser.DeserializeItemInformation();

		List<string> keys = new List<string>(itemDatabase.Keys);

		for(int i = 0; i < keys.Count; ++i)
		{
			worldItemTemplates.Add(itemDatabase[keys[i]].ItemName, (GameObject) Resources.Load(itemDatabase[keys[i]].WorldModel));
		}
	}

	/// <summary>
	/// Creates the interactable item that is ready to be placed in the world.
	/// </summary>
	/// <returns>The interactable item.</returns>
	/// <param name="itemToCreate">Item to create.</param>
	/// <param name="amount">Amount.</param>
	public GameObject CreatePickUpInteractableItem(BaseItem itemToCreate, int amount)
	{
		// create the object with the model
		GameObject item = CreateGenericInteractableItem(itemToCreate);
		PickUpItem pickup = item.AddComponent<PickUpItem>();
		pickup.SetUp();
		pickup.Item = itemToCreate;
		pickup.Show = false;

		// TODO: Make the amount found in one stack to be a variable number
		pickup.Amount = amount;
		item.name = itemToCreate.ItemName;

		return item;
	}

	/// <summary>
	/// Creates a random interactable item that is ready to be placed in the world.
	/// </summary>
	/// <returns>A random interactable item.</returns>
	public GameObject CreateRandomPickupInteractableItem(string district)
	{
		// create the object with the model
		BaseItem baseItem = Game.Instance.ItemFactoryInstance.GetWeightedRandomBaseItem(district, true);
		GameObject item = GameObject.Instantiate (worldItemTemplates[baseItem.ItemName]);

		// creates the trigger object that will handle interaction with player
		GameObject triggerObject = GameObject.Instantiate(triggerObjectPrefab);
		triggerObject.transform.SetParent(item.transform);
		triggerObject.transform.localPosition = Vector3.zero;

		PickUpItem pickup = item.AddComponent<PickUpItem>();
		pickup.SetUp();
		pickup.Item = baseItem;
		pickup.Show = false;

		// TODO: Make the amount found in one stack to be a variable number
		pickup.Amount = (int) Random.Range(itemAmountRange.min, itemAmountRange.max);

		item.name = baseItem.ItemName;

		return item;
	}

	/// <summary>
	/// Creates a generic interactable item. Does not add on the interactable script, but sets up item with assumption that player will attach one.
	/// </summary>
	/// <returns>The generic interactable item.</returns>
	/// <param name="itemToCreate">Item to create.</param>
	public GameObject CreateGenericInteractableItem(BaseItem itemToCreate)
	{
		if(!worldItemTemplates.ContainsKey(itemToCreate.ItemName))
		{
			worldItemTemplates.Add(itemToCreate.ItemName, (GameObject) Resources.Load(itemToCreate.WorldModel));
		}

		// create the object with the model
		GameObject item = GameObject.Instantiate (worldItemTemplates[itemToCreate.ItemName]);
		item.layer = LayerMask.NameToLayer(layerName);
		item.tag = tagName;

		// creates the trigger object that will handle interaction with player
		GameObject textObject = GameObject.Instantiate(triggerObjectPrefab);
		textObject.transform.SetParent(item.transform);
		textObject.transform.localPosition = Vector3.zero;
		item.name = itemToCreate.ItemName;

		return item;
	}


	/// <summary>
	/// Gets all interactable items by district. GetRandomItemIndex can be used to get a random item from the Dictionary returned by this function.
	/// </summary>
	/// <returns>The all interactable items by district.</returns>
	/// <param name="setActive">If set to <c>true</c>, gameobjects are active when created.</param>
	/// <param name="water">If set to <c>true</c>, gets objects for water.</param>
	public Dictionary<string, List<GameObject>> GetAllInteractableItemsByDistrict(bool setActive, bool water)
	{
		ItemFactory itemFactory = Game.Instance.ItemFactoryInstance;
		Dictionary<string, List<string>> interactableItemNamesByDistrict = itemFactory.LandItemsByDistrict;

		if(water)
		{
			interactableItemNamesByDistrict = itemFactory.WaterItemsByDistrict;
		}

		Dictionary<string, List<GameObject>> interactableItemsByDistrict = new Dictionary<string, List<GameObject>>();

		int i;

		foreach(string key in interactableItemNamesByDistrict.Keys)
		{
			interactableItemsByDistrict.Add(key, new List<GameObject>());

			for(i = 0; i < interactableItemNamesByDistrict[key].Count; ++i)
			{
				// since these will only be used for templates, there is not need for an amount
				interactableItemsByDistrict[key].Add(CreatePickUpInteractableItem(itemFactory.GetBaseItem(interactableItemNamesByDistrict[key][i]), 0));
				interactableItemsByDistrict[key][i].SetActive(setActive);
			}
		}

		return interactableItemsByDistrict;
	}

	/// <summary>
	/// Gets the random index that can be used to access items in a district. Order is by the Dictionary returned by GetAllInteractableItemsByDistrict.
	/// </summary>
	/// <returns>The random item index.</returns>
	/// <param name="district">District.</param>
	/// <param name="onWater">Whether or not items are generating from water.</param>
	public int GetRandomItemIndex(string district, bool onWater)
	{
		return Game.Instance.ItemFactoryInstance.GetWeightedRandomItemIndex(district, onWater);
	}
}
