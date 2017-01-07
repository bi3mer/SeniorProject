using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DummyNoteGenerator : MonoBehaviour
{
	void Start()
	{
		// read in YAML notes
		loadNotes();
	}

	private void loadNotes(){

		NoteYAMLParser parser = new NoteYAMLParser("NoteListYaml.yml");
		Dictionary<string, Note> noteDict = parser.LoadNotes ();
//		Debug.Log("yay");
//		Debug.Log ("Note generator:" + noteDict.Count);
//		Debug.Log("yay2");
	}

}

