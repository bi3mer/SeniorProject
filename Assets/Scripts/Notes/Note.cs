using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Base interactable note  class for all notes.
/// </summary>
public class Note : InteractableObject
{ 
    /// <summary>
	/// Gets or sets the display text from the NoteData object.
	/// </summary>
	/// <value>The display text.</value>
	public string DisplayText 
	{
		get;
		set;
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		SetUp();
	}

	/// <summary>
	/// Sets up the InteractableObject.
	/// </summary>
	public void SetUpNote()
	{
		base.SetUp();

		// Set note interactable action to Read.
		SetAction
		(
			delegate 
			{ 
				Read(); 
			}
		);
	}

	/// <summary>
	/// Toggle visibility of Note UI panel onscreen.
	/// </summary>
	public void Read()
	{
		if (NoteUIController.Instance.NoteUIPanel.activeSelf) 
		{
			NoteUIController.Instance.NoteUIPanel.SetActive (false);

            Game.Instance.PlayerInstance.Controller.IsReading = false;
		} 

		else 
		{
			NoteUIController.Instance.SetText (DisplayText);
			NoteUIController.Instance.NoteUIPanel.SetActive (true);
            Game.Instance.PlayerInstance.Controller.IsReading = true;
        }
	}
}