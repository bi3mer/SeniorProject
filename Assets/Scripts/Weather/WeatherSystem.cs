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
	private const int highPressureIndex = 1;
	private const int lowPressureIndex  = 0;

	private Vector2[] stormCenterLocations;
	private float[] StormCenterPressure;
	public float[] WeatherInformation
	{
		get;
		private set;
	}

	// For these constants below please view
	// https://github.com/bi3mer/WeatherForecasting/blob/master/Lorenz%20Simulation.ipynb
	// to see how they are used in the context of graphs
	private const float airDensity                = 3.4f;
	private const float kelvinConverter           = 273.15f; 

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
	private readonly float[] precipitationCoefficients = {0.00000000e+00f,  -0.00000000e+00f,  -0.00000000e+00f,
                                                         -0.00000000e+00f,   0.00000000e+00f,   0.00000000e+00f,
                                                         -4.46876693e-06f,   2.07456756e-07f,   1.50875127e-07f,
                                                          9.82522088e-04f,  -3.10613821e-07f,  -0.00000000e+00f,
                                                         -0.00000000e+00f,   0.00000000e+00f,  -0.00000000e+00f,
                                                         -0.00000000e+00f,   0.00000000e+00f,  -0.00000000e+00f,
                                                         -0.00000000e+00f,   0.00000000e+00f,   0.00000000e+00f};
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

	/// <summary>
	/// Gets the pressure based on the player from the pressure centers
	/// </summary>
	/// <returns>The pressure.</returns>
	/// <param name="position">Position.</param>
	/// <param name="centerIndex">Center index.</param>
	private float getPressure(Vector2 position, int centerIndex)
	{
		// Get multiplier for kind of pressure system
		int multiplier = 1;
		if(centerIndex == WeatherSystem.highPressureIndex)
		{
			multiplier = -1;
		}

		// calculate pressure system
		return StormCenterPressure[centerIndex] + 
			   multiplier * Vector2.Distance(position, this.stormCenterLocations[centerIndex]);
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
	/// <returns>The position on slope and direciton.</returns>
	/// <param name="position">Position.</param>
	/// <param name="center">Center.</param>
	/// <param name="yIntercept">Y intercept.</param>
	/// <param name="slope">Slope.</param>
	/// <param name="centerIndex">Center index.</param>
	private Vector2 calculatePositionOnSlopeAndDirection(Vector2 position, Vector2 center, float yIntercept, float slope, int centerIndex)
	{
		// get x and y coord in direction of the wind force
		Vector2 newPosition = position;
		if(center.y == position.y)
		{
			// this is the case where the line is either going straight up
			// or striaght down
			if(center.x >= position.x)
			{
				if(this.isHighPressure(centerIndex))
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
				if(this.isHighPressure(centerIndex))
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
			if(center.y > position.y)
			{
				if(this.isHighPressure(centerIndex))
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
				if(this.isHighPressure(centerIndex))
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
	/// Sets the wind speed vector.
	/// TODO: this needs to broken up into multiple functions
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="centerIndex">Center index.</param>
	private void setWindSpeedVector(Vector2 position, int centerIndex)
	{
		Vector2 center = this.stormCenterLocations[centerIndex];
		float perpindicularSlope = VectorUtility.GetPerpindicularSlope(position, center);

		// calculate y intercept
		float yIntercept = position.y - (perpindicularSlope * position.x);

		// get position of point on the line in correct direction
		Vector2 newPosition = this.calculatePositionOnSlopeAndDirection(position, center, yIntercept, perpindicularSlope, centerIndex);

		// get angle of the wind
		float windAngle = VectorUtility.GetAngle(newPosition, position);

		// add forces to weatherInformation
		this.WeatherInformation[(int) Weather.WindSpeedX] = Mathf.Sin(windAngle) * this.WeatherInformation[(int) Weather.WindSpeedMagnitude];
		this.WeatherInformation[(int) Weather.WindSpeedY] = Mathf.Cos(windAngle) * this.WeatherInformation[(int) Weather.WindSpeedMagnitude];
}

	/// <summary>
	/// Gets the wind speed vector from the weather informaiton
	/// </summary>
	public Vector2 GetWindSpeedVector()
	{
		return new Vector2(this.WeatherInformation[(int) Weather.WindSpeedX], 
			               this.WeatherInformation[(int) Weather.WindSpeedY]);
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
	/// Gets the index of the center closest to the posiiton
	/// </summary>
	/// <returns>The center index.</returns>
	/// <param name="position">Position.</param>
	public int GetCenterIndex(Vector2 position)
	{
		// TODO: Update this for a future build
		return WeatherSystem.highPressureIndex;
	}

	/// <summary>
	/// Updates the weather array variable
	/// </summary>
	/// <param name="position">Position.</param>
	public void UpdateWeather(Vector3 positionVector)
	{
		// convert position to vector2
		Vector2 position = new Vector2(positionVector.x, positionVector.z);

		// TODO: update this for a future build
		int centerIndex = WeatherSystem.highPressureIndex;

		this.WeatherInformation[(int) Weather.Pressure]           = this.getPressure(position, centerIndex);
		this.WeatherInformation[(int) Weather.Temperature]        = this.getTemperature() + DiurnalTemperatureVariance.TemperatureVariance;
		this.WeatherInformation[(int) Weather.WindSpeedMagnitude] = this.getWindSpeed();
		this.WeatherInformation[(int) Weather.RelativeHumidity]   = this.getRelativeHumidity();
		this.WeatherInformation[(int) Weather.RelativeDewPoint]   = this.getRelativeDewPoint();
		this.WeatherInformation[(int) Weather.Precipitation]      = this.getPrecipitation();
		this.setWindSpeedVector(position, centerIndex);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="WeatherSystem"/> class.
	/// </summary>
	public WeatherSystem()
	{
		// eight is the size of the array needed 
		// based on the number of enums present in
		// Weather
		this.WeatherInformation = new float[Weather.GetNames(typeof(Weather)).Length];

		// TODO: this needs to be updated to be dynamic but
		//       for now it creates an example of how the 
		//       weather works. So please ignore the magic
		//       numbers for now
		this.stormCenterLocations    = new Vector2[2];
		this.stormCenterLocations[0] = Vector2.left*10;
		this.stormCenterLocations[0] = Vector2.right*10;

		this.StormCenterPressure = new float[2];
		this.StormCenterPressure[0] = 1000;
		this.StormCenterPressure[1] = 1013;
	}
}