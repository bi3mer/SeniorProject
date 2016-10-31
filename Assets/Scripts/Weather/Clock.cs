using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
	/// <summary>
	/// 30 seconds make up half an hour in game time
	/// </summary>
	public const int HalfAnHour = 30;

	/// <summary>
	/// Twenty four hours in game time
	/// </summary>
	public const int TwentyFourHours = 1440;

	public const float HourDivisor = 60.0f; // 60*1000

	private int currentTime = 0;
	public int CurrentTime
	{
		get
		{
			return this.currentTime;
		}
		set
		{
			// pass
		}
	}

	/// <summary>
	/// Gets the current game time in hours. Setting the
	/// value is not supported.
	/// </summary>
	/// <value>The current game time in hours.</value>
	public float CurrentGameTimeInHours
	{
		get
		{
			return this.currentTime / Clock.HourDivisor;
		}
		set
		{
			// pass
		}
	}

	private bool isHalfHour = true;

	// delegates for updates
	public delegate void HalfHourDelegateUpdate();
	public event HalfHourDelegateUpdate HalfHourUpdate;

	public delegate void HourDelegateUpdate();
	public event HourDelegateUpdate HourUpdate;

	/// <summary>
	/// Start the update time loop. 
	/// </summary>
	public void Start () 
	{
		StartCoroutine(this.updateTime());
	}

	/// <summary>
	/// Update delegates for every half hour and hour 
	/// long cycle.
	/// </summary>
	/// <returns>The time.</returns>
	private IEnumerator updateTime()
	{
		while(true)
		{
			yield return new WaitForSeconds(Clock.HalfAnHour);

			// update time
			this.currentTime += Clock.HalfAnHour;
			if(this.currentTime == Clock.TwentyFourHours)
			{
				this.currentTime = 0;
			}
				
			// notify hour long subscribed delegates
			if(!this.isHalfHour && this.HourUpdate != null)
			{
				this.HourUpdate();
			}

			// Notify thirty minute long subscribed delegates
			if(this.HalfHourUpdate != null)
			{
				this.HalfHourUpdate();
			}

			// update half hour boolean
			this.isHalfHour = !this.isHalfHour;
		}
	}
}
