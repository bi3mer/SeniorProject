using UnityEngine;
using System;
using System.Collections;

public class Clock : MonoBehaviour
{
	/// <summary>
	/// Twenty four hours in game time
	/// </summary>
	//public const int TwentyFourHours = 1440;
	public const int TwentyFourHours = 240;
	public const int TwelveHours     = 120;
	public const int Hour            = 10;
	public const int HalfHour        = 5;
	public const int Tick            = 1;

	private int currentTime = 0;

	/// <summary>
	/// Gets the current time.
	/// </summary>
	/// <value>The current time.</value>
	public int CurrentTime
	{
		get
		{
			return this.currentTime;
		}
	}

	/// <summary>
	/// Gets the current game time in hours.
	/// </summary>
	/// <value>The current game time in hours.</value>
	public float CurrentGameTimeInHours
	{
		get
		{
			return this.currentTime / (float) Clock.Hour;
		}
	}

	// delegates for updates
	public delegate void SecondDelegateUpdate();
	public event SecondDelegateUpdate SecondUpdate;

	public delegate void HalfHourDelegateUpdate();
	public event HalfHourDelegateUpdate HalfHourUpdate;

	public delegate void HourDelegateUpdate();
	public event HourDelegateUpdate HourUpdate;

	/// <summary>
	/// Start the update time loop. 
	/// </summary>
	void Awake() 
	{
		// subscribe to delegate
		Game.Instance.PauseInstance.ResumeUpdate += this.Resume;

		this.Resume();
	}

	/// <summary>
	/// Gets the formatted time of the game clock.
	/// </summary>
	/// <value>The formatted time.</value>
	public string FormattedTime
	{
		get
		{
			// get hours and minutes
			int hours = Mathf.FloorToInt(this.CurrentGameTimeInHours);
			int minutes = this.CurrentTime - (hours * Clock.Hour);

			// format time
			DateTime date = new DateTime(1, 1, 1, hours, minutes, 0, 0);
			return date.ToString("HH:mm");
		}
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
			if(this.CurrentTime >= Clock.TwentyFourHours)
			{
				this.currentTime = 0;
			}

			// TODO find a way to get non monobehaviour scripts to subscribe to
			//      the seconds instance. This will have to do for now.
			Game.Instance.WeatherInstance.UpdateSystem();

			// notify second long subscribed delegates
			if(this.SecondUpdate != null)
			{
				this.SecondUpdate();
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