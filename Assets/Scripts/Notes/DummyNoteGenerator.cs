using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Test class for generating notes from NoteListYaml file
public class DummyNoteGenerator : MonoBehaviour
{
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		loadNotes();
	}

	/// <summary>
	/// Loads the notes.
	/// </summary>
	private void loadNotes()
	{
		NoteYAMLParser parser = new NoteYAMLParser("NoteListYaml.yml");
		List<Note> noteList = parser.LoadNotes();
		NoteFactory factory = new NoteFactory ();

		for (int i = 0; i < noteList.Count; ++i) 
		{
			factory.CreateInteractableItem (noteList[i]);
		}
	}
}

