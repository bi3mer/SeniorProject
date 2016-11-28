using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldItemFactory
{
	private Dictionary<string, GameObject> worldItemTemplates;

	/// <summary>
	/// The location of the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private const string triggerObjectLocation = "ItemGeneration/InteractableTrigger";

	/// <summary>
	/// The name of the item file. All yaml files must be placed under "Resources/YAMLFiles"
	/// </summary>
	private const string itemFileName = "ItemListYaml.yml";

	/// <summary>
	/// the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private GameObject triggerObjectPrefab;

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
	public GameObject CreateInteractableItem(BaseItem itemToCreate, int amount)
	{
		if(!worldItemTemplates.ContainsKey(itemToCreate.ItemName))
		{
			worldItemTemplates.Add(itemToCreate.ItemName, (GameObject) Resources.Load(itemToCreate.WorldModel));
		}

		// create the object with the model
		GameObject item = GameObject.Instantiate (worldItemTemplates[itemToCreate.ItemName]);

		// creates the trigger object that will handle interaction with player
		GameObject triggerObject = GameObject.Instantiate(triggerObjectPrefab);
		triggerObject.GetComponent<BoxCollider>().size = item.GetComponent<BoxCollider>().size;
		triggerObject.GetComponent<BoxCollider>().center = item.GetComponent<BoxCollider>().center;
		triggerObject.transform.SetParent(item.transform);
		triggerObject.transform.localPosition = Vector3.zero;

		PickUpItem pickup = triggerObject.AddComponent<PickUpItem>();
		pickup.SetUp();
		pickup.SetUpPickUp();
		pickup.Item = itemToCreate;

		// TODO: Make the amount found in one stack to be a variable number
		pickup.Amount = amount;

		return item;
	}
}
