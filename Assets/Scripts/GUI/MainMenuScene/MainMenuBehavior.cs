using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject settingsPanel;

	[SerializeField]
	private GameObject endCreditsPanel;

	[SerializeField]
	private GameObject helpPanel;

	[SerializeField]
	private GameObject mainPanel;

	[SerializeField]
	private GameObject startOrLoadPanel;

	private string masterSceneName = "Master";

	/// <summary>
	/// Initialize main menu.
	/// </summary>
	void Start ()
	{
		mainPanel.SetActive (true);
		settingsPanel.SetActive (false);
		endCreditsPanel.SetActive (false);
		helpPanel.SetActive (false);
		startOrLoadPanel.SetActive(false);
	}

	/// <summary>
	/// Loads the settings panel.
	/// </summary>
	public void OnSettingsClick()
	{
		mainPanel.SetActive (false);
		settingsPanel.SetActive (true);
		endCreditsPanel.SetActive (false);
		helpPanel.SetActive (false);
		startOrLoadPanel.SetActive(false);
	}

	/// <summary>
	/// Loads the help panel.
	/// </summary>
	public void OnHelpClick()
	{
		mainPanel.SetActive (false);
		helpPanel.SetActive (true);
		settingsPanel.SetActive (false);
		endCreditsPanel.SetActive (false);
		startOrLoadPanel.SetActive(false);
	}

	/// <summary>
	/// Loads the end credits panel.
	/// </summary>
	public void OnExitClick()
	{
		mainPanel.SetActive (false);
		endCreditsPanel.SetActive (true);
		settingsPanel.SetActive (false);
		helpPanel.SetActive (false);
		startOrLoadPanel.SetActive(false);

		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	/// <summary>
	/// Loads the game scene.
	/// </summary>
	public void OnStartClick()
	{
		mainPanel.SetActive(false);
		helpPanel.SetActive(false);
		settingsPanel.SetActive(false);
		endCreditsPanel.SetActive(false);

		// TODO add checking so if there is no game save it'll
		//      automatically start the game.
		startOrLoadPanel.SetActive(true);
	}

	/// <summary>
	/// Raises the start game click event.
	/// </summary>
	public void OnStartGameClick() 
	{
		SceneManager.LoadScene (masterSceneName);
	}

	/// <summary>
	/// Raises the load game click event.
	/// </summary>
	public void OnLoadGameClick() 
	{
		Debug.LogError("Not Implemented");
	}

	/// <summary>
	/// Loads the main menu panel
	/// </summary>
	public void OnBackToMainClick()
	{
		mainPanel.SetActive (true);
		endCreditsPanel.SetActive (false);
		settingsPanel.SetActive (false);
		helpPanel.SetActive (false);
		startOrLoadPanel.SetActive(false);
	}
}
