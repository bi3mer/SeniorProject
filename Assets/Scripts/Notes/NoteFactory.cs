using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

// loads and places notes from yaml file in world
public class NoteFactory 
{
	private Dictionary<string, GameObject> worldNoteTemplates;
	private const string worldObjectLocation = "ITM/Notes/ITM_NoteBook";
	private const string triggerObjectLocation = "ItemGeneration/InteractableText";
	private const string noteFileName = "Notes.yml";

	[SerializeField]
	private Vector3 defaultpos = new Vector3(-143.9574f, 0.6875932f, 62.05219f);

	private GameObject triggerObjectPrefab;

	public List<string> NoteTitles;
	public List<GameObject> NoteObjects;

	private int noteCounter = -1;

	public bool NotesAvailable 
	{
		get;
		private set;
	}

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

		NotesAvailable = true;
	}

	/// <summary>
	/// Loads the note templates based on what world models are referenced in the NoteListYaml
	/// </summary>
	public void LoadTemplates()  
	{
		NoteYAMLParser noteParser = new NoteYAMLParser(noteFileName);
		List<NoteData> noteDatabase = noteParser.LoadNotes();
		NoteObjects = new List<GameObject> ();
		NoteTitles = new List<string> ();

		for(int i = 0; i < noteDatabase.Count; ++i)
		{
			worldNoteTemplates.Add (noteDatabase[i].Title, (GameObject)Resources.Load (noteDatabase [i].WorldModel));
			CreateInteractableItem (noteDatabase [i]);
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

		// Save note item so we can get it later by its title, return it
		NoteObjects.Add (item);
		NoteTitles.Add ("Note:" + noteToCreate.Title);

		Debug.Log ("adding " + noteToCreate.Title);
		return item;
	}

	/// <summary>
	/// Returns a note given that note's unique title (which serves as its ID)
	/// </summary>
	/// <param name="title">Title.</param>
	public GameObject GetNote(string title)
	{

		GameObject note = new GameObject();
		for (int i = 0; i < NoteTitles.Count; i++) {

			if (NoteTitles [i] == title) {

				note = NoteObjects [i];
			}
		}

		return note;
	}

	/// <summary>
	/// Gets the next note.
	/// </summary>
	/// <returns>The next note.</returns>
	public string GetNextNote()
	{
		// iterate here and check
		noteCounter += 1;
		string noteTitle = "";

		if (noteCounter < NoteObjects.Count) 
		{
			noteTitle = NoteTitles [noteCounter];
		} 
		else 
		{
			NotesAvailable = false;
		}

		if (noteCounter == (NoteObjects.Count - 1))
		{
			NotesAvailable = false;
		}


		Debug.Log ("returning " + noteTitle);
		return noteTitle;
	}
}