using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTextLoader : MonoBehaviour 
{
	/// <summary>
	/// The text that the note should display.
	/// </summary>
	[Tooltip("What the note should say")]
	public string NoteText;

	/// <summary>
	/// The associated note.
	/// </summary>
	public Note AssociatedNote;

	/// <summary>
	/// Used only for the tutorial building. Gives a note text.
	/// </summary>
	void Start () 
	{
		AssociatedNote.DisplayText = NoteText;
	}
}
