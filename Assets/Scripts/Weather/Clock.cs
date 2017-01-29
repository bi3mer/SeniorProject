using UnityEngine;
using System;
using System.Collections;

public class Clock : MonoBehaviour
{
	[SerializeField]
	[Tooltip ("1 is where 1 minute is equal to one hour in game time.")]
	private float tick = 1f;

	private float twentyFourHours;
	private float twelveHours;
	private float hour;
	private float halfHour;

	private const float addTick = 1f;

	[SerializeField]
	[Tooltip("Should the clock in the scene stop updating the weather")]
	private bool freezeWeatherUpdates;

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
			return this.CurrentTime / this.hour;
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
		// 1 minute * 60 = 60 minutes = 1 hour
		this.hour = tick * 60;

		// half hour is half of an hour
		this.halfHour = this.hour / 2f;

		// 1hour * 12 = 12 hours
		this.twelveHours = this.hour * 12f;

		// 12hours * 2 = 24 hours
		this.twentyFourHours = this.twelveHours * 2;

		// 1/24Hours = x/360
		this.AnglePerSecond =  360f / this.twentyFourHours;

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
			int minutes = Mathf.FloorToInt(this.CurrentTime - (hours * this.hour));

			// format time
			DateTime date = new DateTime(1, 1, 1, hours, minutes, 0, 0);
			return date.ToString("HH:mm");
		}
	}

	/// <summary>
	/// Updates the time.
	/// </summary>
	/// <returns>The time.</returns>
	private IEnumerator updateTime()
	{
		while(!Game.Instance.PauseInstance.IsPaused)
		{
			yield return new WaitForSeconds(this.tick);

			// update time
			this.CurrentTime += Clock.addTick;
			if(this.CurrentTime >= this.twentyFourHours)
			{
				this.CurrentTime = 0;
			}

			// TODO find a way to get non monobehaviour scripts to subscribe to
			//      the seconds instance. This will have to do for now.

			if(!freezeWeatherUpdates)
			{
				Game.Instance.WeatherInstance.UpdateSystem();
			}

			// notify second long subscribed delegates
			if(this.SecondUpdate != null)
			{
				this.SecondUpdate();
			}

			int integerTime = Mathf.FloorToInt(this.CurrentTime);

			// Notify thirty minute long subscribed delegates
			if(this.HalfHourUpdate != null && integerTime % Mathf.FloorToInt(this.halfHour) == 0)
			{
				this.HalfHourUpdate();
			}

			// notify hour long subscribed delegates
			if(this.HourUpdate != null && integerTime % Mathf.FloorToInt(this.hour) == 0)
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
			return Game.Instance.ClockInstance.CurrentTime <= this.twelveHours;
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
			return this.CurrentTime / this.twelveHours;
		}
		else
		{
			return (this.CurrentTime - this.twelveHours) / this.twelveHours;
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