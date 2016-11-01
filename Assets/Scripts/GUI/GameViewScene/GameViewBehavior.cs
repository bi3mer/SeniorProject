using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameViewBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject inventoryPanel;
	[SerializeField]
	private GameObject pausePanel;
	[SerializeField]
	private GameObject radioPanel;

	/// <summary>
	/// Start the Game View with all other menu panels disabled.
	/// </summary>
	void Start () 
	{
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
		inventoryPanel.SetActive (false);
		radioPanel.SetActive (false);

	}

	/// <summary>
	/// Loads the inventory panel.
	/// </summary>
	public void OnInventoryClick()
	{
		inventoryPanel.SetActive (true);
		pausePanel.SetActive (false);
		radioPanel.SetActive (false);
	}

	/// <summary>
	/// Loads the radio panel.
	/// </summary>
	public void OnRadioClick()
	{
		radioPanel.SetActive (true);
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
	}

	/// <summary>
	/// Returns to the game scene.
	/// </summary>
	public void OnResumeClick()
	{
		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);
		radioPanel.SetActive (false);
	}
}
