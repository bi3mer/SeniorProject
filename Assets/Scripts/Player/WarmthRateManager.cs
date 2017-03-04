using UnityEngine;

public class WarmthRateManager : StatRate
{	
	private StatRate defaultWarmthRate;

	// default temperature checks and seconds intervals (sets warmth rate based on whether or not current temperature is ><= these values)
	private const int defaultUnitWarmth = -1;

	private const float tempTier6 = 60.0f;
	private const float tempTier5 = 50.0f;
	private const float tempTier4 = 40.0f;
	private const float tempTier3 = 30.0f;
	private const float tempTier2 = 20.0f;
	private const float tempTier1 = 10.0f;
	private const float tempTier0 = 0.0f;
	private const float tempTierNeg = 0.0f;

	private const int secondsTier6 = 10;
	private const int secondsTier5 = 9;
	private const int secondsTier4 = 8;
	private const int secondsTier3 = 7;
	private const int secondsTier2 = 6;
	private const int secondsTier1 = 5;
	private const int secondsTier0 = 4;
	private const int secondsTierNeg = 3;
	
	public WarmthRateManager ()
	{
		// set the default rate initially (only based on temperature)
		defaultWarmthRate = GetTemperatureBasedWarmthRate();
		this.Units = defaultWarmthRate.Units;
		this.PerSeconds = defaultWarmthRate.PerSeconds;
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
		else 
		{
			return null;
		}
	}
}


