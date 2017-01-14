using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoteUIController : MonoBehaviour {

	public static NoteUIController instance;
	public GameObject NoteUIPanel;
	public Text NoteText;

	// Use this for initialization
	void Start () 
	{
		instance = this;
		NoteUIPanel.SetActive (false);
	}

	void Update ()
	{

	}

	public void SetText(string text)
	{
		NoteText.text = text;
	}

}
