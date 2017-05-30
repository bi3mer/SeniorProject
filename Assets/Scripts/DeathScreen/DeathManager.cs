using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathManager 
{
	private const string deathSceneName = "Death";

	/// <summary>
	/// Loads death screen.
	/// </summary>
	public void Death()
	{
		if (Application.isEditor)
		{
			return;
		}

		SceneManager.LoadScene(deathSceneName);
	}
}
