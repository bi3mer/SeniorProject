using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// loads and places notes from yaml file in world
public class NoteFactory : MonoBehaviour 
{
    // list of world models to be used
	private Dictionary<string, GameObject> worldNoteTemplates;

	/// <summary>
	/// The location of the prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private const string triggerObjectLocation = "ItemGeneration/InteractableTrigger";
    
    /// <summary>
	/// The name of the notes file. All yaml files must be placed under "Resources/YAMLFiles"
	/// </summary>
	private const string itemFileName = "NoteListYaml.yml";

	/// <summary>
	/// The prefab that contains the trigger collider and text mesh needed for interactable objects
	/// </summary>
	private GameObject triggerObjectPrefab;

	/// <summary>
	/// Initializes a new instance of the <see cref="NoteFactory"/> class.
	/// </summary>
	public NoteFactory()
	{
        // get the trigger object for interaction
		triggerObjectPrefab = (GameObject) Resources.Load(triggerObjectLocation);

        // initialize the list of world models to be used
		worldNoteTemplates = new Dictionary<string, GameObject>();
		LoadTemplates();
	}

	/// <summary>
	/// Loads the note templates based on what world models are referenced in the NoteListYaml
	/// </summary>
	public void LoadTemplates()
	{
		// ItemSerializer itemParser = new ItemSerializer(itemFileName, "");
		// Dictionary<string, BaseItem> itemDatabase = itemParser.DeserializeItemInformation();
		// List<string> keys = new List<string>(itemDatabase.Keys);
		// for(int i = 0; i < keys.Count; ++i)
		// {
		// 	worldItemTemplates.Add(itemDatabase[keys[i]].ItemName, (GameObject) Resources.Load(itemDatabase[keys[i]].WorldModel));
		// }
	}

	/// <summary>
	/// Creates the interactable item that is ready to be placed in the world.
	/// </summary>
	/// <returns>The interactable item.</returns>
	/// <param name="noteToCreate">Note to create.</param>
	public GameObject CreateInteractableItem(Note noteToCreate)
	{
		if(!worldNoteTemplates.ContainsKey(noteToCreate.Type))
		{
			worldNoteTemplates.Add(noteToCreate.Type, (GameObject) Resources.Load(noteToCreate.WorldModel));
		}

		// create the object with the model for the given note type
		GameObject item = GameObject.Instantiate (worldNoteTemplates(noteToCreate.Type));

		// // creates the trigger object that will handle interaction with player
		// GameObject triggerObject = GameObject.Instantiate(triggerObjectPrefab);
		// triggerObject.GetComponent<BoxCollider>().size = item.GetComponent<BoxCollider>().size;
		// triggerObject.GetComponent<BoxCollider>().center = item.GetComponent<BoxCollider>().center;
		// triggerObject.transform.SetParent(item.transform);
		// triggerObject.transform.localPosition = Vector3.zero;

        // TODO: replace with note pick up.
		// PickUpItem pickup = triggerObject.AddComponent<PickUpItem>();
		// pickup.SetUp();
		// pickup.SetUpPickUp();
		// pickup.Item = itemToCreate;

        // Currently not allowing notes to be stacked.
		// pickup.Amount = 1;

		// return item;
	}
}


}