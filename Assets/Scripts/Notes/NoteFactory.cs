using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// loads and places notes from yaml file in world
public class NoteFactory 
{
	private Dictionary<string, GameObject> worldNoteTemplates;
	private const string worldObjectLocation = "ITM/Notes/ITM_NoteBook";
	private const string triggerObjectLocation = "ItemGeneration/InteractableText";
	private const string noteFileName = "Notes.yml";

	private Vector3 defaultpos = new Vector3(-124.726f, .704f, -1f);

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
		NoteYAMLParser noteParser = new NoteYAMLParser(noteFileName);
		List<NoteData> noteDatabase = noteParser.LoadNotes();

		for(int i = 0; i < noteDatabase.Count; ++i)
		{
			Debug.Log (noteDatabase [i]);
			worldNoteTemplates.Add (noteDatabase[i].Title, (GameObject)Resources.Load (noteDatabase [i].WorldModel));
		}
	}

	/// <summary>
	/// Creates an interactable note item that is ready to be placed in the world.
	/// </summary>
	/// <returns>The interactable item.</returns>
	/// <param name="noteToCreate">Note to create.</param>
	public GameObject CreateInteractableItem(NoteData noteToCreate)
	{
		if(!worldNoteTemplates.ContainsKey(noteToCreate.WorldModel))
		{
			worldNoteTemplates.Add(noteToCreate.WorldModel, (GameObject) Resources.Load(noteToCreate.WorldModel));
		}

		// create the object with the model
		GameObject item = GameObject.Instantiate (worldNoteTemplates[noteToCreate.WorldModel]);
		item.name = noteToCreate.Title;
		item.transform.position = defaultpos;

		// creates the trigger object that will handle interaction with player
		GameObject textObject = GameObject.Instantiate(triggerObjectPrefab);
		textObject.transform.SetParent(item.transform);
		textObject.transform.localPosition = Vector3.zero;

		// Set up the interactable note
		Note itemNote = item.AddComponent<Note>();
		itemNote.DisplayText = noteToCreate.Text;
		itemNote.SetUp ();
		itemNote.Text = noteToCreate.Title;
		itemNote.Show = false;

		return item;
	}
}