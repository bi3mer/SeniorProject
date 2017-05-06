using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTextLoader : MonoBehaviour 
{
	[Tooltip("What the note should say")]
	public string NoteText;

	public Note AssociatedNote;

	/// <summary>
	/// Used only for the tutorial building. Gives a note text.
	/// </summary>
	void Start () 
	{
		AssociatedNote.DisplayText = NoteText;
	}
}
