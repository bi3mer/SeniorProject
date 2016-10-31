using UnityEngine;
using System.Collections;

public class Tide : MonoBehaviour 
{
	[SerializeField]
	private float TideSeverity     = 5.0f;
	private const float sinOffet   = 6.0f;
	private const float sinDivisor = 3.7f;

	private FloodWater floodWater;

	/// <summary>
	/// Subscribe to clock and get script for updating tide height
	/// </summary>
	void Start () 
	{
		// subscribe to clock
		Game.Instance.ClockInstance.HourUpdate += this.UpdateTideLevel;

		// get component for updating water height
		this.floodWater = this.GetComponent<FloodWater>();
	}

	/// <summary>
	/// Calculates the height of the tide based on calculations from notebook
	/// found at: https://github.com/bi3mer/WeatherForecasting/blob/master/Tide.ipynb
	/// </summary>
	/// <returns>The tide height.</returns>
	private float calculateTideHeight()
	{
		return Mathf.Sin((Game.Instance.ClockInstance.CurrentGameTimeInHours - Tide.sinOffet)/Tide.sinDivisor) * this.TideSeverity;
	}

	/// <summary>
	/// Updates the day night based on the time
	/// </summary>
	private void UpdateTideLevel()
	{
		this.floodWater.CurrentLevelOffset = this.calculateTideHeight();
	}
}
