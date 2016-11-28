
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetEnvironmentGUI : MonoBehaviour 
{
	[SerializeField]
	private Text clock;

	[SerializeField]
	private Text pressure;
	private string pressureString;

	[SerializeField]
	private Text temperature;
	private string temperatureString;

	[SerializeField]
	private Text windSpeedMagnitude;
	private string windSpeedMagnitudeString;

	[SerializeField]
	private Text windSpeedX;
	private string windSpeedXString;

	[SerializeField]
	private Text windSpeedY;
	private string windSpeedYString;

	[SerializeField]
	private Text relativeHumidity;
	private string relativeHumidityString;

	[SerializeField]
	private Text relativeDewPoint;
	private string relativeDewPointString;

	[SerializeField]
	private Text precipitation;
	private string precipitationString;

	// TODO: make this a game instance in the future
	private Transform playerPosition;

	/// <summary>
	/// Get default strings from each text.
	/// </summary>
	void Start()
	{
		this.pressureString           = this.pressure.text;
		this.temperatureString        = this.temperature.text;
		this.windSpeedMagnitudeString = this.windSpeedMagnitude.text;
		this.windSpeedXString         = this.windSpeedX.text;
		this.windSpeedYString         = this.windSpeedY.text;
		this.relativeHumidityString   = this.relativeHumidity.text;
		this.relativeDewPointString   = this.relativeDewPoint.text;
		this.precipitationString      = this.precipitation.text;
	}

	/// <summary>
	/// Update the gui fields
	/// </summary>
	void Update()
	{
		// update fields
		this.clock.text              = Game.Instance.ClockInstance.FormattedTime;
		this.pressure.text           = this.pressureString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.Pressure].ToString();
		this.temperature.text        = this.temperatureString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.Temperature].ToString();
		this.windSpeedMagnitude.text = this.windSpeedMagnitudeString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.WindSpeedMagnitude].ToString();
		this.windSpeedX.text         = this.windSpeedXString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.WindSpeedX].ToString();
		this.windSpeedY.text         = this.windSpeedYString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.WindSpeedY].ToString();
		this.relativeHumidity.text   = this.relativeHumidityString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.RelativeHumidity].ToString();
		this.relativeDewPoint.text   = this.relativeDewPointString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.RelativeDewPoint].ToString();
		this.precipitation.text      = this.precipitationString + " " + 
		                               Game.Instance.WeatherInstance.WeatherInformation[(int) Weather.Precipitation].ToString();
	}
}
