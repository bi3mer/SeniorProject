using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartBehavior : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The name of the main scene")]
	private string mainGameScene;

	public void Restart()
	{
		SceneManager.LoadScene(mainGameScene);
	}
}
