using UnityEngine;
using System.Collections;

public class Tide : MonoBehaviour 
{
	/// <summary>
	/// Subscribe to clock
	/// </summary>
	void Start () 
	{
		Game.Instance.ClockInstance.HourUpdate += this.UpdateTideLevel;
	}

	/// <summary>
	/// Updates the day night based on the time
	/// </summary>
	private void UpdateTideLevel()
	{
		// TODO: Update this to affect the water level
	}
}
