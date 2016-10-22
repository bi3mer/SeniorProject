using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour {

	[SerializeField]
	private GameObject settingsPanel;
	[SerializeField]
	private GameObject endCreditsPanel;
	[SerializeField]
	private GameObject helpPanel;


	/// <summary>
	/// Initialize main menu.
	/// </summary>
	void Start ()
	{
		//settingsPanel = GameObject.Find("SettingsPanel");
		//endCreditsPanel = GameObject.Find("EndCreditsPanel");
		settingsPanel.SetActive (false);
		endCreditsPanel.SetActive (false);
		helpPanel.SetActive (false);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// update keycodes from the ControlScheme here - to perform the methods stated below
	}

	/// <summary>
	/// Loads the settings panel.
	/// </summary>
	public void OnSettingsClick()
	{
		settingsPanel.SetActive (true);
		endCreditsPanel.SetActive (false);
		helpPanel.SetActive (false);
	}

	/// <summary>
	/// Loads the help panel.
	/// </summary>
	public void OnHelpClick()
	{
		helpPanel.SetActive (true);
		settingsPanel.SetActive (false);
		endCreditsPanel.SetActive (false);
	}


	/// <summary>
	/// Loads the end credits panel.
	/// </summary>
	public void OnExitClick()
	{
		endCreditsPanel.SetActive (true);
		settingsPanel.SetActive (false);
		helpPanel.SetActive (false);
	}

	/// <summary>
	/// Loads the game scene.
	/// </summary>
	public void OnStartClick(int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
	}

	/// <summary>
	/// Loads the main menu panel
	/// </summary>
	public void OnBackToMainClick()
	{
		endCreditsPanel.SetActive (false);
		settingsPanel.SetActive (false);
		helpPanel.SetActive (false);
	}
}
