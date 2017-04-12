using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBehavior : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The name of the main scene")]
	private string mainGameScene;

	/// <summary>
	/// Restarts the game. Goes back to main game scene and resets the game.
	/// </summary>
	public void Restart()
	{
		Game.Instance.Reset();
		SceneManager.LoadScene(mainGameScene);
	}
}
