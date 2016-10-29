using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour 
{
	/// <summary>
	/// Subscribe to clock
	/// </summary>
	void Start () 
	{
		Game.Instance.ClockInstance.HalfHourUpdate += this.UpdateDayNight;
	}

	/// <summary>
	/// Updates the day night based on the time
	/// </summary>
	private void UpdateDayNight()
	{
		// TODO: Update this to affect the directional light, come up
		//       with better update in the future.
	}
}
