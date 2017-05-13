using UnityEngine;
using System.Collections;

public class FloodWater : MonoBehaviour 
{
	[Header("Tide")]
	// view link to see how tide severity works with a graph:
	// https://github.com/bi3mer/WeatherForecasting/blob/master/Tide.ipynb
	[SerializeField]
	private float TideSeverity = 1.0f;
	private Vector3 nightPosition;
	private Vector3 dayPosition;

	[Header("Precipitation")]

	/// <summary>
	/// This is how much the water level is increasing during the game due to
	/// precipitation. 
	/// </summary>
	[SerializeField]
	[Tooltip("Water level affected by the precipitation. Mostly meant for visualization.")]
	private float precipitationWaterLevel = 0f;

	[SerializeField]
	[Range(1,10000)]
	[Tooltip("This divides the precipitation to mitigate it's effect.")]
	private float precipitationMitigation = 5000f;

	[SerializeField]
	[Range(0,100)]
	[Tooltip("This is the level where there is less precipitation and we should decrease the height instead of increase")]
	private float precipitationNegation = 20f;


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
		// only update when game is active
		if(Game.Instance.PauseInstance.IsPaused == false)
		{
			// get precipitation from weather
			float precipitation = Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.Precipitation];

			// make sure that the precipiation received is a valid numer
			if(float.IsNaN(precipitation) == false)
			{
				// if the precipitation is below the negation level
				if(precipitation < this.precipitationNegation)
				{
					// multiply it by negative 1 to subtract in the calculation below
					precipitation *= -1;
				}

				// update precipiation level on a per frame basis.
				this.precipitationWaterLevel += (precipitation / this.precipitationMitigation) * Time.fixedDeltaTime;
				this.transform.position       = this.getTideLevel() + VectorUtility.HeightVector3d(this.precipitationWaterLevel);
			}
		}
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
