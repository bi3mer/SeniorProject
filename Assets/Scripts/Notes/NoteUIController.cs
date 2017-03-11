using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Note user interface controller.
/// TODO: Make this a child of game UI
/// </summary>
public class NoteUIController : MonoBehaviour 
{
	/// <summary>
	/// The instance.
	/// </summary>
	public static NoteUIController Instance;

	/// <summary>
	/// The note user interface panel.
	/// </summary>
	public GameObject NoteUIPanel;

	/// <summary>
	/// The note text.
	/// </summary>
	public Text NoteText;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Awake () 
	{
		Instance = this;
		NoteUIPanel.SetActive (false);
	}

	/// <summary>
	/// Sets the text.
	/// </summary>
	/// <param name="text">Text.</param>
	public void SetText(string text)
	{
		NoteText.text = text;
	}

	/// <summary>
	/// Sets the UI active to false.
	/// </summary>
	public void DisableUI()
	{
		NoteUIPanel.SetActive (false);
	}

}
