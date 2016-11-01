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
	private ControlScheme controlScheme;

	/// <summary>
	/// Start the Game View with all other menu panels disabled.
	/// </summary>
	void Start () 
	{
		controlScheme = Game.Instance.PlayerInstance.Controller.Scheme;
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		radioPanel.SetActive (false);
	}

	/// <summary>
	/// Get player input and update accordingly.
	/// </summary>
	void Update()
	{
		if (Input.GetKey (controlScheme.Pause)) 
		{
			if (Game.Instance.IsPaused) 
			{
				OnResumeClick ();
			} 
			else 
			{
				OnPauseClick ();
			}
		}
		if (Input.GetKey (controlScheme.Inventory)) 
		{
			OnInventoryClick ();
		}
		if (Input.GetKey (controlScheme.Radio)) 
		{
			OnRadioClick ();
		}
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
