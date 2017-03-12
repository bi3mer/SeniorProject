using UnityEngine;
using System.Collections;

/// <summary>
/// Enumeratino for accessing weather variables in the 
/// array that contians the weather data in classs
/// WeatherSystem
/// </summary>
public enum Weather
{
	Pressure,
	Temperature,
	WindSpeedMagnitude,
	WindSpeedX,
	WindSpeedY,
	RelativeHumidity,
	RelativeDewPoint,
	Precipitation
};

public class WeatherSystem 
{
	// For these constants below please view
	// https://github.com/bi3mer/WeatherForecasting/blob/master/Lorenz%20Simulation.ipynb
	// to see how they are used in the context of graphs
	private const float airDensity         = 3.4f;
	private const float kelvinConverter    = 273.15f; 

	// constants for celcius to farenheight
	private const float celciusNumerator   = 9.0f;
	private const float celciusDenominator = 5.0f;
	private const float celciusAdder       = 32.0f;

	// wind speed
	private readonly float[] windSpeedCoefficents = {0f, -13.641617f, 0.00670152751f};
	private readonly int[,] windSpeedPowers       = {{0}, {1}, {2}};
	private const float windSpeedIntercept        = 6951.17066465f;

	// relative humidity
	private readonly float[] relativeHumidityCoefficients = {0f,0f, 0f, -0.00042569f, 0.00282497f, -0.01846812f};
	private readonly int[,] relativeHumidityPowers        = {{0, 0},{1, 0},{0, 1},{2, 0},{1, 1},{0, 2}};
	private const float relativeHumidityIntercept         = 402.68743138357206f;

	// precipitation
	private readonly float[] precipitationCoefficients = { 0f,0f,0f,
	                                                       0f,0f,0f,
		                                                  -3.85205537e-06f, -5.77718149e-09f,  4.79910109e-08f,
		                                                   9.82160232e-04f, -7.69745423e-08f, 0f,
		                                                   9.82160232e-04f, -7.69745423e-08f, 0f,
		                                                   0f,0f,0f,
		                                                   0f,0f,0f,
		                                                   0f,0f,0f};
	private readonly int[,] precipitationPowers        = {{0, 0, 0, 0, 0},{1, 0, 0, 0, 0},{0, 1, 0, 0, 0},{0, 0, 1, 0, 0},
		                                                  {0, 0, 0, 1, 0},{0, 0, 0, 0, 1},{2, 0, 0, 0, 0},{1, 1, 0, 0, 0},
		                                                  {1, 0, 1, 0, 0},{1, 0, 0, 1, 0},{1, 0, 0, 0, 1},{0, 2, 0, 0, 0},
		                                                  {0, 1, 1, 0, 0},{0, 1, 0, 1, 0},{0, 1, 0, 0, 1},{0, 0, 2, 0, 0},
		                                                  {0, 0, 1, 1, 0},{0, 0, 1, 0, 1},{0, 0, 0, 2, 0},{0, 0, 0, 1, 1},
		                                                  {0, 0, 0, 0, 2}};
	private const float precipitationIntercept         = 4.6207279047035579f;

	// Calculating relative dew point constants
	private const float relativeHumidityConstant  = 243.04f;
	private const float percentageDivisor         = 100f;
	private const float temperatureCoefficient    = 17.625f;

	// Precipitation flag for storm and delegates for beginning of storm and end
	private const float minPrecipitationForStorm = 20f;

	// possible vectors for 8 directions
	private readonly Vector2[] defaultDirectionVector = new Vector2[] {new Vector2(0,0), new Vector2(1,1), new Vector2(0,1), new Vector2(-1,1),
	                                                                   new Vector2(-1,0), new Vector2(-1,-1), new Vector2(0,-1), new Vector2(1,-1)};

	private bool ongoingStorm = false;
	private bool updateWeather = true;

	/// <summary>
	/// Gets the wind direction in 2d.
	/// </summary>
	/// <value>The wind direction.</value>
	public Vector2 WindDirection2d
	{
		get
		{
			return new Vector2(this.WeatherInformation[(int) Weather.WindSpeedX],
			                   this.WeatherInformation[(int) Weather.WindSpeedY]);
		}
	}

	/// <summary>
	/// Gets the wind direction in 3d.
	/// </summary>
	/// <value>The wind direction3d.</value>
	public Vector3 WindDirection3d
	{
		get
		{
			return new Vector3(this.WeatherInformation[(int) Weather.WindSpeedX],
			                   0,
			                   this.WeatherInformation[(int) Weather.WindSpeedY]);
		}
	}

	/// <summary>
	/// Gets the cartesian wind direction2d. 
	/// </summary>
	/// <value>The cartesian wind direction2d.</value>
	public Vector2 NormalizedOctantWindDirection2d
	{
		get
		{
			const int numDirections = 8;
			const float twoPi       = 2f * Mathf.PI;

			Vector2 wind = this.WindDirection2d;

			// positive angle of vector in degrees
			float angle = (Mathf.Atan2(wind.y, wind.x) + twoPi) % twoPi;

			// conver the angle to one of 8 directions
			int index = Mathf.RoundToInt(((numDirections * angle) / twoPi)) % numDirections;

			return defaultDirectionVector[index];
		}
	}

	public PressureSystems WeatherPressureSystems
	{
		get;
		private set;
	}

	public float[] WeatherInformation
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the pressure based on the player from the pressure centers
	/// </summary>
	/// <returns>The pressure.</returns>
	/// <param name="position">Position.</param>
	/// <param name="center">Center.</param>
	private float getPressure(Vector2 position, PressureSystem center)
	{
		// Get multiplier for kind of pressure system
		int multiplier = 1;
		if(center.IsHighPressure)
		{
			multiplier = -1;
		}

		// calculate pressure system
		return center.Pressure + (multiplier * Vector2.Distance(position, center.Position));
	}


	/// <summary>
	/// Convert kelvin temperature to celcius
	/// </summary>
	/// <returns>The to celcius.</returns>
	/// <param name="temperature">Temperature.</param>
	private float kelvinToCelcius(float temperature)
	{
		return temperature - WeatherSystem.kelvinConverter;
	}

	/// <summary>
	/// Celciuses to fahrenheit conversion
	/// </summary>
	/// <returns>The to fahrenheit.</returns>
	/// <param name="temperature">Temperature.</param>
	private float celciusToFahrenheit(float temperature)
	{
		return temperature * (WeatherSystem.celciusNumerator / WeatherSystem.celciusDenominator) + WeatherSystem.celciusAdder;
	}

	/// <summary>
	/// Kelvin to fahrenheit conversion.
	/// </summary>
	/// <returns>The to fahrenheit.</returns>
	/// <param name="temperature">Temperature.</param>
	private float kelvinToFahrenheit(float temperature)
	{
		return this.celciusToFahrenheit(this.kelvinToCelcius(temperature));
	}

	/// <summary>
	/// Gets the temperature.
	/// </summary>
	/// <returns>The temperature.</returns>
	private float getTemperature()
	{
		return this.kelvinToFahrenheit(this.WeatherInformation[(int)Weather.Pressure] / WeatherSystem.airDensity);
	}

	/// <summary>
	/// Predicts the wind speed with linearRegressionPrediction
	/// </summary>
	/// <returns>The wind speed.</returns>
	private float getWindSpeed() 
	{
		float[] inputs = {this.WeatherInformation[(int) Weather.Pressure]};
		return Regression.Prediction(this.windSpeedCoefficents, 
			                         this.windSpeedPowers, 
			                         inputs,
			                         WeatherSystem.windSpeedIntercept);
	}

	/// <summary>
	/// Determine if the index is indicative of a high pressure system
	/// or low pressure system. This is important for determining if 
	/// a system is high or low pressure
	///
	/// TODO: this will be updated in the future when >2 pressure systems
	///       are present
	/// </summary>
	/// <returns><c>true</c>, if high is high pressure, <c>false</c> otherwise.</returns>
	/// <param name="index">Index.</param>
	private bool isHighPressure(int index)
	{
		return index == 1;
	}

	/// <summary>
	/// Calculates the position on slope and direction of wind vector.
	/// </summary>
	/// <returns>The position on slope and direction.</returns>
	/// <param name="position">Position.</param>
	/// <param name="center">Center.</param>
	/// <param name="yIntercept">Y intercept.</param>
	/// <param name="slope">Slope.</param>
	private Vector2 calculatePositionOnSlopeAndDirection(Vector2 position, PressureSystem center, float yIntercept, float slope)
	{
		// get x and y coord in direction of the wind force
		Vector2 newPosition = position;
		if(center.Position.y == position.y)
		{
			// this is the case where the line is either going straight up
			// or striaght down
			if(center.Position.x >= position.x)
			{
				if(center.IsHighPressure)
				{
					newPosition.y += 1;
				}
				else
				{
					newPosition.y -= 1;
				}
			}
			else
			{
				if(center.IsHighPressure)
				{
					newPosition.y -= 1;
				}
				else
				{
					newPosition.y += 1;
				}
			}
		}
		else
		{
			if(center.Position.y > position.y)
			{
				if(center.IsHighPressure)
				{
					newPosition.x += 1;
				}
				else
				{
					newPosition.x -= 1;
				}
			}
			else
			{
				if(center.IsHighPressure)
				{
					newPosition.x -= 1;
				}
				else
				{
					newPosition.x =+ 1;
				}
			}

			// calculate new y point
			newPosition.y = (slope * newPosition.x) + yIntercept;
		}

		return newPosition;
	}

	/// <summary>
	/// Sets the wind speed vector based on wind magnitude and pressure system type.
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="center">Center.</param>
	private void setWindSpeedVector(Vector2 position, PressureSystem center)
	{
		float perpindicularSlope = VectorUtility.GetPerpindicularSlope(position, center.Position);

		// calculate y intercept
		float yIntercept = position.y - (perpindicularSlope * position.x);

		// get position of point on the line in correct direction
		Vector2 newPosition = this.calculatePositionOnSlopeAndDirection(position, center, yIntercept, perpindicularSlope);

		// get angle of the wind
		float windAngle = VectorUtility.GetAngle(newPosition, position);

		// add forces to weatherInformation
		this.WeatherInformation[(int) Weather.WindSpeedX] = Mathf.Sin(windAngle) * this.WeatherInformation[(int) Weather.WindSpeedMagnitude];
		this.WeatherInformation[(int) Weather.WindSpeedY] = Mathf.Cos(windAngle) * this.WeatherInformation[(int) Weather.WindSpeedMagnitude];
	}

	/// <summary>
	/// Gets the relative humidity using linear regression constants
	/// contained in class
	/// </summary>
	/// <returns>The relative humidity.</returns>
	private float getRelativeHumidity()
	{
		float[] inputs = {this.WeatherInformation[(int) Weather.Pressure], 
		                  this.WeatherInformation[(int) Weather.Temperature]};

		return Regression.Prediction(this.relativeHumidityCoefficients, 
			                         this.relativeHumidityPowers, 
			                         inputs,
			                         WeatherSystem.relativeHumidityIntercept);
	}

	/// <summary>
	/// Gets the relative dew point using equaiton found at
	/// http://andrew.rsmas.miami.edu/bmcnoldy/Humidity.html
	/// </summary>
	/// <returns>The relative dew point.</returns>
	private float getRelativeDewPoint()
	{
		float relativeHumidity = this.WeatherInformation[(int) Weather.RelativeHumidity];
		float temperature      = this.WeatherInformation[(int) Weather.Temperature];

		return WeatherSystem.relativeHumidityConstant*(Mathf.Log(relativeHumidity/WeatherSystem.percentageDivisor) +
			   ((WeatherSystem.temperatureCoefficient*temperature)/(WeatherSystem.relativeHumidityConstant+temperature))) /
			   (WeatherSystem.temperatureCoefficient-Mathf.Log(relativeHumidity/WeatherSystem.percentageDivisor) - 
			   ((WeatherSystem.temperatureCoefficient*temperature)/(WeatherSystem.relativeHumidityConstant+temperature)));
	}

	/// <summary>
	/// Gets the precipitation using linear regression constants
	/// </summary>
	/// <returns>The precipitaiton.</returns>
	private float getPrecipitation()
	{
		float[] inputs = {this.WeatherInformation[(int) Weather.Pressure],
			              this.WeatherInformation[(int) Weather.Temperature],
			              this.WeatherInformation[(int) Weather.RelativeHumidity],
			              this.WeatherInformation[(int) Weather.WindSpeedMagnitude],
			              this.WeatherInformation[(int) Weather.RelativeDewPoint]};

		return Regression.Prediction(this.precipitationCoefficients, 
			                         this.precipitationPowers, 
		                             inputs,
			                         WeatherSystem.precipitationIntercept);
	}

	/// <summary>
	/// Updates the delegates.
	/// </summary>
	private void updateStormDelegates()
	{
		if(this.ongoingStorm)
		{
			if(this.WeatherInformation[(int) Weather.Precipitation] < WeatherSystem.minPrecipitationForStorm)
			{
				this.ongoingStorm = false;

				// update subscribed
				Game.Instance.EventManager.StormStop ();
			}
		}
		else if(this.WeatherInformation[(int) Weather.Precipitation] >= WeatherSystem.minPrecipitationForStorm)
		{
			this.ongoingStorm = true;

			// update subscribed
			Game.Instance.EventManager.StormStart ();
		}
	}

	/// <summary>
	/// Updates the weather array variable
	/// </summary>
	/// <param name="position">Position.</param>
	public void UpdateWeather(Vector3 positionVector)
	{
		// convert position to vector2
		Vector2 position = new Vector2(positionVector.x, positionVector.z);
		PressureSystem center = this.WeatherPressureSystems.GetClosestPressureSystem(position);

		this.WeatherInformation[(int) Weather.Pressure]           = this.getPressure(position, center);
		this.WeatherInformation[(int) Weather.Temperature]        = this.getTemperature() + DiurnalTemperatureVariance.TemperatureVariance;
		this.WeatherInformation[(int) Weather.WindSpeedMagnitude] = this.getWindSpeed();
		this.WeatherInformation[(int) Weather.RelativeHumidity]   = this.getRelativeHumidity();
		this.WeatherInformation[(int) Weather.RelativeDewPoint]   = this.getRelativeDewPoint();
		this.WeatherInformation[(int) Weather.Precipitation]      = this.getPrecipitation();
		this.setWindSpeedVector(position, center);

		this.updateStormDelegates();

		// update weather sounds
		Game.Instance.EventManager.WeatherUpdated (this.getPrecipitation());
	}

	/// <summary>
	/// Enables the weather updates.
	/// </summary>
	public void EnableWeather()
	{
		updateWeather = true;
	}

	/// <summary>
	/// Disables the weather updates.
	/// </summary>
	public void DisableWeather()
	{
		updateWeather = false;
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="WeatherSystem"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="WeatherSystem"/>.</returns>
	public override string ToString()
	{
		const string newLine = "\n";

		string weather = "Pressure: " + this.WeatherInformation[(int) Weather.Pressure] + newLine;
		weather += "Temperature: " + this.WeatherInformation[(int) Weather.Temperature] + newLine;
		weather += "Wind Magnitude: " + this.WeatherInformation[(int) Weather.WindSpeedMagnitude] + newLine;
		weather += "Wind X: " + this.WeatherInformation[(int) Weather.WindSpeedX] + newLine;
		weather += "Wind X: " + this.WeatherInformation[(int) Weather.WindSpeedX] + newLine;
		weather += "Relative Humidity: " + this.WeatherInformation[(int) Weather.RelativeHumidity] + newLine;
		weather += "Relative Dew Point: " + this.WeatherInformation[(int) Weather.RelativeDewPoint] + newLine;
		weather += "Precipitation: " + this.WeatherInformation[(int) Weather.Precipitation] + newLine;

		return weather;
	}

	/// <summary>
	/// Updates the weather and internal pressure systems.
	/// </summary>
	public void UpdateSystem()
	{
		if(updateWeather)
		{
			this.WeatherPressureSystems.UpdatePressureSystem();
			this.UpdateWeather(Game.Instance.PlayerInstance.WorldPosition);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="WeatherSystem"/> class.
	/// </summary>
	/// <param name="bounds">Bounds of the city.</param>
	/// <param name="pauseController">Pause system instance.</param>
	public WeatherSystem(CityBoundaries bounds, PauseSystem pauseController)
	{
#if UNITY_EDITOR
		if(!Application.isPlaying)
		{
			return;
		}
#endif

		this.WeatherInformation = new float[Weather.GetNames(typeof(Weather)).Length];
		this.WeatherPressureSystems = new PressureSystems(bounds);
		pauseController.PauseUpdate += DisableWeather;
		pauseController.ResumeUpdate += EnableWeather;
	}
}