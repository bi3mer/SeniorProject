using UnityEngine;
using System;
using System.Collections;

public class Clock : MonoBehaviour
{
	[SerializeField]
	[Tooltip ("How a second compares to a minute. For example: 1 corresponds to 1 seconds is 1 minute of gametime.")]
	private float tick = 1f;

	private float lastGameSecond;

	[SerializeField]
	[Tooltip("Should the clock in the scene stop updating the weather")]
	private bool freezeWeatherUpdates;

	public float Tick
	{
		get;
		private set;
	}

	/// <summary>
	/// The twenty four hours.
	/// </summary>
	public float TwentyFourHours
	{
		get;
		private set;
	}

	/// <summary>
	/// The twelve hours.
	/// </summary>
	public float TwelveHours
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the hour.
	/// </summary>
	/// <value>The hour.</value>
	public float Hour
	{
		get;
		private set;
	}

	/// <summary>
	/// Half an hour of game time in real world seconds
	/// </summary>
	/// <value>The half hour.</value>
	public float HalfHour
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the angle per second based on time scale.
	/// </summary>
	/// <value>The angle per second.</value>
	public float AnglePerSecond
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the current time.
	/// </summary>
	/// <value>The current time.</value>
	public float CurrentTime
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the current game time in hours.
	/// </summary>
	/// <value>The current game time in hours.</value>
	public float CurrentGameTimeInHours
	{
		get
		{
			return this.CurrentTime / this.Hour;
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
		this.Tick = this.tick;

		this.CurrentTime = 0f;

		// 60 minutes in an hour
		this.Hour = this.Tick * 60f;

		// 1440 minutes in a day
		this.TwentyFourHours = this.Tick * 1440f;

		// divide hour by 2 to get half an hour
		this.HalfHour = this.Hour / 2f;

		// divide day by 2 to get twelve hours of game time
		this.TwelveHours = this.TwentyFourHours / 2f;

		// 1/24Hours = x/360
		this.AnglePerSecond =  360f / this.TwentyFourHours;

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
			int hours   = Mathf.FloorToInt(this.CurrentGameTimeInHours);
			int minutes = Mathf.FloorToInt(this.CurrentTime - (hours * this.Hour));

			// format time
			DateTime date = new DateTime(1, 1, 1, hours, minutes, 0, 0);
			return date.ToString("HH:mm");
		}
	}

	/// <summary>
	/// Updates the time.
	/// </summary>
	/// <returns>The time.</returns>
	void Update()
	{
		if(Game.Instance.PauseInstance.IsPaused)
		{
			// break out, we don't want to update while the game is paused
			return;
		}

		// check if the weather system should be updated
		if(!freezeWeatherUpdates)
		{
			// TODO: move this to it's own monobehavior class
			Game.Instance.WeatherInstance.UpdateSystem();
		}

		// update time
		this.CurrentTime += Time.deltaTime / this.Tick;

		if(this.lastGameSecond + this.Tick < this.CurrentTime)
		{
			this.lastGameSecond += this.Tick;

			if(this.CurrentTime >= this.TwentyFourHours)
			{
				this.CurrentTime = 0;
			}

			// notify second long subscribed delegates
			if(this.SecondUpdate != null)
			{
				this.SecondUpdate();
			}

			// Notify thirty minute long subscribed delegates
			if(this.HalfHourUpdate != null && Mathf.Approximately(this.CurrentTime % this.HalfHour, 0f))
			{
				this.HalfHourUpdate();
			}

			// notify hour long subscribed delegates
			if(this.HourUpdate != null && Mathf.Approximately(this.CurrentTime % this.Hour, 0f))
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
			return Game.Instance.ClockInstance.CurrentTime <= this.TwelveHours;
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
			return this.CurrentTime / this.TwelveHours;
		}
		else
		{
			return (this.CurrentTime - this.TwelveHours) / this.TwelveHours;
		}
	}
}