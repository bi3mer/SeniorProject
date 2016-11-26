using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreenBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject gameLogoPanel;
	[SerializeField]
	private GameObject teamLogoPanel;
	[SerializeField]
	private GameObject mainMenuPanel;

	/// <summary>
	/// Start this splash screen instance.
	/// </summary>
	void Start () 
	{
		teamLogoPanel.SetActive (true);
		mainMenuPanel.SetActive (false);
		gameLogoPanel.SetActive (false);
		StartCoroutine (DisplayGameLogo());
	}

	/// <summary>
	/// Display the Game Logo panel.
	/// </summary>
	IEnumerator DisplayGameLogo () 
	{
		yield return new WaitForSeconds (2.2f);
		gameLogoPanel.SetActive (true);
		mainMenuPanel.SetActive (false);
		teamLogoPanel.SetActive (false);
		StartCoroutine (DisplayMainMenu());
	}
	
	/// <summary>
	/// Display the Main Menu panel.
	/// </summary>
	IEnumerator DisplayMainMenu () 
	{
		yield return new WaitForSeconds (3.3f);
		mainMenuPanel.SetActive (true);
		teamLogoPanel.SetActive (false);
		gameLogoPanel.SetActive (false);
	}
}
