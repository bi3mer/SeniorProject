using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameViewBehavior : MonoBehaviour {

	private GameObject inventoryPanel;
	private GameObject pausePanel;
	private GameObject radioPanel;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		inventoryPanel = GameObject.Find ("InventoryPanel");
		pausePanel = GameObject.Find ("PausePanel");
		radioPanel = GameObject.Find ("RadioPanel");
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		radioPanel.SetActive (false);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// update keycodes from the ControlScheme here - to perform the methods stated below
	}

	/// <summary>
	/// Loads the pause panel.
	/// </summary>
	public void OnPauseClick() 
	{
		pausePanel.SetActive (true);
	}

	/// <summary>
	/// Loads the inventory panel.
	/// </summary>
	public void OnInventoryClick()
	{
		inventoryPanel.SetActive (true);
	}

	/// <summary>
	/// Loads the radio panel.
	/// </summary>
	public void OnRadioClick()
	{
		radioPanel.SetActive (true);
	}

	/// <summary>
	/// Returns to the game scene.
	/// </summary>
	public void OnResumeClick()
	{
		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);
		radioPanel.SetActive (false);
		// enable player?/set Raycast Target to true again?
	}

}
