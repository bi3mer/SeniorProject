using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoteUIController : MonoBehaviour {

	public static NoteUIController instance;
	public GameObject NoteUIPanel;
	public Text NoteText;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		instance = this;
		NoteUIPanel.SetActive (false);
	}

	void Update ()
	{

	}

	/// <summary>
	/// Sets the text.
	/// </summary>
	/// <param name="text">Text.</param>
	public void SetText(string text)
	{
		NoteText.text = text;
	}

}
