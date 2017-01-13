using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Test class for generating notes from NoteListYaml file
public class DummyNoteGenerator : MonoBehaviour
{
	void Start()
	{
		loadNotes();
	}

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

