using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreenBehavior : MonoBehaviour 
{
	[SerializeField]
	private float timer = 2f;

	[SerializeField]
	private GameObject mainMenuPanel;

	/// <summary>
	/// Start this splash screen instance.
	/// </summary>
	void Start () 
	{
		mainMenuPanel.SetActive (false);
		StartCoroutine ("DisplayMainMenu");
	}
	
	/// <summary>
	/// Display the Main Menu panel.
	/// </summary>
	IEnumerator DisplayMainMenu () 
	{
		yield return new WaitForSeconds (timer);
		mainMenuPanel.SetActive (true);
		GameObject.Find ("SplashScreen").SetActive (false);
	}
}
