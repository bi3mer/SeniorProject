using UnityEngine;
using System.Collections;

public class LightningEffect : MonoBehaviour 
{
	[SerializeField]
	private float gameLifeTime = 0.3f;

	/// <summary>
	/// Scales life time
	/// </summary>
	void Start()
	{
		// scale life time to clock
		this.gameLifeTime = this.gameLifeTime * Game.Instance.ClockInstance.Tick;
	}

	/// <summary>
	/// Raises the enable event and starts coroutine for lightning event
	/// </summary>
	void OnEnable () 
	{
		StartCoroutine(this.activateAndDie());
	}

	private IEnumerator activateAndDie()
	{
		// TODO Activate sound alliy

		yield return new WaitForSeconds(this.gameLifeTime);

		this.gameObject.SetActive(false);
	}
}
