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

	private GameObject triggerObjectPrefab;

	private List<NoteData> noteDatabase;

	private int noteCounter = 0;

	/// <summary>
	/// Gets a value indicating whether this <see cref="NoteFactory"/> has notes available.
	/// </summary>
	/// <value><c>true</c> if notes available; otherwise, <c>false</c>.</value>
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
		noteDatabase = noteParser.LoadNotes();

		for(int i = 0; i < noteDatabase.Count; ++i)
		{
			if(!worldNoteTemplates.ContainsKey(noteDatabase[i].WorldModel))
			{
				worldNoteTemplates.Add (noteDatabase[i].WorldModel, (GameObject)Resources.Load (noteDatabase [i].WorldModel));
			}
		}
	}

	/// <summary>
	/// Creates a note.
	/// </summary>
	/// <returns>The note gameobject.</returns>
	/// <param name="noteToCreate">Data regarding the note to create.</param>
	public GameObject CreateNote(NoteData noteToCreate)
	{
		if(!worldNoteTemplates.ContainsKey(noteToCreate.WorldModel))
		{
			worldNoteTemplates.Add(noteToCreate.WorldModel, (GameObject) Resources.Load(noteToCreate.WorldModel));
		}

		// create the object with the model
		GameObject item = GameObject.Instantiate (worldNoteTemplates[noteToCreate.WorldModel]);
		item.name = noteToCreate.Title;

		item.SetActive (false);

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

		item.SetActive (false);
		return item;
	}

	/// <summary>
	/// Gets the next note.
	/// </summary>
	/// <returns>The next note.</returns>
	public GameObject GetNextNote()
	{
		GameObject nextNote = null;

		if (noteCounter < noteDatabase.Count) 
		{
			nextNote = CreateNote(noteDatabase[noteCounter]);
		} 

		// iterate here and check
		noteCounter += 1;

		// check again so that NotesAvailable is immediately false
		// but previous is still needed to prevent game breaking when noteDatabase is 0 to start with
		if(noteCounter >= noteDatabase.Count) 
		{
			NotesAvailable = false;
		}

		return nextNote;
	}
}