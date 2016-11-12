using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour
{
	/// <summary>
	/// Twenty four hours in game time
	/// </summary>
	public const int TwentyFourHours = 1440;
	public const int TwelveHours     = 720;
	public const int Hour            = 60;
	public const int HalfHour        = 30;
	public const int Tick            = 1;

	public const float HourDivisor = 60.0f;

	private int currentTime = 0;
	public int CurrentTime
	{
		get
		{
			return this.currentTime;
		}
	}

	public float CurrentGameTimeInHours
	{
		get
		{
			return this.currentTime / (float) Clock.HourDivisor;
		}
	}

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
		// subscribe to delegate
		Game.Instance.PauseInstance.ResumeUpdate += this.Resume;

		this.Resume();
	}

	/// <summary>
	/// Update delegates for every half hour and hour 
	/// long cycle.
	/// </summary>
	/// <returns>The time.</returns>
	private IEnumerator updateTime()
	{
		while(!Game.Instance.PauseInstance.IsPaused)
		{
			yield return new WaitForSeconds(Clock.Tick);

			// update time
			this.currentTime += Clock.Tick;
			if(this.CurrentTime == Clock.TwentyFourHours)
			{
				this.currentTime = 0;
			}

			// Notify thirty minute long subscribed delegates
			if(this.HalfHourUpdate != null && this.currentTime % Clock.HalfHour == 0)
			{
				this.HalfHourUpdate();
			}

			// notify hour long subscribed delegates
			if(this.HourUpdate != null && this.currentTime % Clock.Hour == 0)
			{
				this.HourUpdate();
			}
		}
	}

	/// <summary>
	/// Determines whether this it is or day night.
	/// </summary>
	/// <returns><c>true</c> if this instance is day; otherwise, <c>false</c>.</returns>
	public bool IsDay
	{
		get
		{
			return Game.Instance.ClockInstance.CurrentTime <= Clock.TwelveHours;
		}
	}

	/// <summary>
	/// Gets the percentage that of twelve hours that the current 
	/// time is representative of. This is useful for lerping
	/// and other related functions.
	/// </summary>
	/// <returns>The twelve hour percentage.</returns>
	public float GetTwelveHourPercentage()
	{
		if(this.IsDay)
		{
			return this.CurrentTime/(float)Clock.TwelveHours;
		}
		else
		{
			return (this.CurrentTime - Clock.TwelveHours)/(float)Clock.TwelveHours;
		}
	}

	/// <summary>
	/// Resume the clock running.
	/// </summary>
	public void Resume()
	{
		StartCoroutine(this.updateTime());
	}
}
