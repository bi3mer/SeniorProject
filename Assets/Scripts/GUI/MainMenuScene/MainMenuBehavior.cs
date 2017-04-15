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
		SceneManager.LoadScene (masterSceneName);
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
	}
}
