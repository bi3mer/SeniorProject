using UnityEngine;

public class PlayerStatManager
{
	private Player player;
	private StatRate defaultHungerRate;
	private StatRate defaultWarmthRate;
	private StatRate defaultHealthRate;

	// set default values for hunger, warmth, and health rates
	private const int defaultUnitHunger = -1;
	private const int defaultSecondsHunger = 7;
	private const int defaultUnitHealth = 0;
	private const int defaultSecondsHealth = 1;
	private const int defaultUnitWarmth = -1;

	// default temperature checks and seconds intervals (sets warmth rate based on whether or not current temperature is ><= these values)
	private const float tempTier6 = 60.0;
	private const float tempTier5 = 50.0;
	private const float tempTier4 = 40.0;
	private const float tempTier3 = 30.0;
	private const float tempTier2 = 20.0;
	private const float tempTier1 = 10.0;
	private const float tempTier0 = 0.0;
	private const float tempTierNeg = 0.0;

	private const int secondsTier6 = 10;
	private const int secondsTier5 = 9;
	private const int secondsTier4 = 8;
	private const int secondsTier3 = 7;
	private const int secondsTier2 = 6;
	private const int secondsTier1 = 5;
	private const int secondsTier0 = 4;
	private const int secondsTierNeg = 3;

	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerStatManager"/> class.
	/// </summary>
	/// <param name="playerInstance">Player instance.</param>
	public PlayerStatManager (Player playerInstance)
	{
		player = playerInstance;

		// set initial default rates based on the values listed in gdd
		defaultHungerRate = new StatRate(defaultUnitHunger, defaultSecondsHunger);
		defaultHealthRate = new StatRate(defaultUnitHealth, defaultSecondsHealth);
		defaultWarmthRate = GetTemperatureBasedWarmthRate ();

		// set current rates to the default ones initially
		this.HungerRate = defaultHungerRate;
		this.WarmthRate = defaultWarmthRate;
		this.HealthRate = defaultHealthRate;
	}



	/// <summary>
	/// Gets the temperature based warmth rate.
	/// </summary>
	/// <returns>The temperature based warmth rate.</returns>
	private StatRate GetTemperatureBasedWarmthRate()
	{
		// get game instance's current temperature
		float currentTemperature = Game.Instance.WeatherInstance.WeatherInformation [(int)Weather.Temperature];

		Debug.Log (currentTemperature); // DEBUG LOG STATEMENT

		if (currentTemperature >= tempTier6)
		{
			return new StatRate (defaultUnitWarmth, secondsTier6);
		}
		else if (currentTemperature >= tempTier5)
		{
			return new StatRate (defaultUnitWarmth, secondsTier5);
		}
		else if (currentTemperature >= tempTier4)
		{
			return new StatRate (defaultUnitWarmth, secondsTier4);
		}
		else if (currentTemperature >= tempTier3)
		{
			return new StatRate (defaultUnitWarmth, secondsTier3);
		}
		else if (currentTemperature >= tempTier2)
		{
			return new StatRate (defaultUnitWarmth, secondsTier2);
		}
		else if (currentTemperature >= tempTier1)
		{
			return new StatRate (defaultUnitWarmth, secondsTier1);
		}
		else if (currentTemperature >= tempTier0)
		{
			return new StatRate (defaultUnitWarmth, secondsTier0);
		}
		else if (currentTemperature >= tempTierNeg)
		{
			return new StatRate (defaultUnitWarmth, secondsTierNeg);
		}
	}


	//[Tooltip("The warmth rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets or sets the warmth rate (can increase or decrease in units(+/-) per seconds).
	/// </summary>
	/// <value>The warmth rate.</value>
	public StatRate WarmthRate 
	{
		get;
		set;
	}

	//[Tooltip("The hunger rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets or sets the hunger rate (can increase or decrease in units(+/-) per seconds).
	/// </summary>
	/// <value>The hunger rate.</value>
	public StatRate HungerRate 
	{
		get;
		set;
	}

	//[Tooltip("The health rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets or sets the health rate (can increase or decrease in units(+/-) per seconds).
	/// </summary>
	/// <value>The health rate.</value>
	public StatRate HealthRate 
	{
		get;
		set;
	}
}


