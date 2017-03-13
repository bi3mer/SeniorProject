using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.UI;

// Test class for generating notes from NoteListYaml file
public class DummyNoteGenerator : MonoBehaviour
{
	/// <summary>
	/// Start this instance.
	/// </summary>
	/// 
	void Start()
	{
		loadNotes();
	}

	/// <summary>
	/// Loads the notes.
	/// </summary>
	private void loadNotes()
	{
		NoteYAMLParser parser = new NoteYAMLParser("Notes.yml");
		List<NoteData> noteList = parser.LoadNotes();
		NoteFactory factory = new NoteFactory ();

		for (int i = 0; i < noteList.Count; ++i) 
		{ 
			factory.CreateInteractableItem (noteList[i]);
		}
	
		// TODO: Replace this with code that references a not yet made extension of Shreya's GUI classes for notes
		NoteUIController.Instance.SetText (noteList[0].Text + " -- " + noteList[0].Title);
	}
}