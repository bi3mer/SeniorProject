using UnityEngine;
using System.Collections;

public class FloodWater : MonoBehaviour 
{
	// view link to see how tide severity works with a graph:
	// https://github.com/bi3mer/WeatherForecasting/blob/master/Tide.ipynb
	[SerializeField]
	private float TideSeverity = 1.0f;
	private Vector3 nightPosition;
	private Vector3 dayPosition;

	/// <summary>
	/// Gets the height of the water level.
	/// </summary>
	/// <value>The height of the water level.</value>
	public float WaterLevelHeight
	{
		get
		{
			return this.transform.position.y;
		}
	}

	/// <summary>
	/// Gets the level of the tide based on current game time.
	/// </summary>
	private Vector3 getTideLevel()
	{
		if(Game.Instance.ClockInstance.IsDay)
		{
			return Vector3.Lerp(this.nightPosition, 
				                this.dayPosition, 
				                Game.Instance.ClockInstance.GetTwelveHourPercentage());
		}
		else
		{
			return Vector3.Lerp(this.dayPosition, 
				                this.nightPosition, 
				                Game.Instance.ClockInstance.GetTwelveHourPercentage());
		}
	}

	/// <summary>
	/// Update the water level
	/// </summary>
	void FixedUpdate()
	{
		// TODO: this should be updated in the future to handle precipitation
		//       from the weather
		this.transform.position = this.getTideLevel();
	}

	/// <summary>
	/// Set the base water level at the start and set up tide 
	/// water levels for day and night
	/// </summary>
	void Start()
	{
		// create night and day tide positions
		this.nightPosition = this.dayPosition = this.transform.position;
		this.dayPosition.y += this.TideSeverity + this.TideSeverity;
	}
}
