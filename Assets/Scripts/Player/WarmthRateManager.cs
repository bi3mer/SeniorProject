using UnityEngine;

public class WarmthRateManager : StatRate
{
	// Add additional rate presets here
	private StatRate inWaterWarmthReductionRate;
	private const int speedOfWarmthRateDecreaseInWater = 2;
	private StatRate nearHeatSourceWarmthIncreaseRate;
	private const int heatSourceUnit = 2;
	private const int heatSourceSeconds = 1;

	private const int secondsForByFireWithCloth = 1;
	private const int secondsForInWaterWithCloth = 3;
	private int unitsInShelter = 0;

	// set default values for warmth rates
	private StatRate defaultWarmthReductionRate;

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

	/// <summary>
	/// Initializes a new instance of the <see cref="WarmthRateManager"/> class.
	/// </summary>
	public WarmthRateManager ()
	{
		// set the default rate initially (only based on temperature)
		UseDefaultWarmthReductionRate ();

		// set initial stat value
		this.CurrentStat = Game.Instance.PlayerInstance.Warmth;
		this.MaxStat = Game.Instance.PlayerInstance.MaxWarmth;

		// set up other rate presets as per gdd guidelines
		nearHeatSourceWarmthIncreaseRate = new StatRate(heatSourceUnit, heatSourceSeconds);
	}

	public int WarmthDelay
	{
		get 
		{

		}
	}

	/// <summary>
	/// Uses the default warmth reduction rate.
	/// </summary>
	public void UseDefaultWarmthReductionRate()
	{
		// recalculate default rate based on current temperature
		defaultWarmthReductionRate = GetTemperatureBasedWarmthRate();
		this.Units = defaultWarmthReductionRate.Units;
		this.PerSeconds = defaultWarmthReductionRate.PerSeconds;
	}

	/// <summary>
	/// Uses the water warmth reduction rate.
	/// </summary>
	public void UseWaterWarmthReductionRate()
	{
		// compute water warmth reduction rate based on current temperature
		StatRate rateBasedOnTemp = GetTemperatureBasedWarmthRate();
		int newSeconds = rateBasedOnTemp.PerSeconds / speedOfWarmthRateDecreaseInWater; // as per gdd guidelines
		inWaterWarmthReductionRate = new StatRate(rateBasedOnTemp.Units, newSeconds);

		this.Units = inWaterWarmthReductionRate.Units;
		this.PerSeconds = inWaterWarmthReductionRate.PerSeconds;
	}

	/// <summary>
	/// Uses the heat source warmth increase rate.
	/// </summary>
	public void UseHeatSourceWarmthIncreaseRate()
	{
		// fire in shelter or fire outside
		if (Game.Instance.PlayerInstance.Controller.IsByFire) 
		{
			this.Units = GetTemperatureBasedWarmthRate ().Units + nearHeatSourceWarmthIncreaseRate.Units;
			this.PerSeconds = nearHeatSourceWarmthIncreaseRate.PerSeconds;
		} 
		// just in shelter
		else if (Game.Instance.PlayerInstance.Controller.IsInShelter && !Game.Instance.PlayerInstance.Controller.IsByFire) 
		{
			this.Units = unitsInShelter;
			this.PerSeconds = nearHeatSourceWarmthIncreaseRate.PerSeconds;
		}
	}

	/// <summary>
	/// Uses the cloth rate.
	/// </summary>
	/// <param name="units">Units.</param>
	public void UseClothRate(int clothUnits)
	{
		this.Units = this.Units + clothUnits;
		if (Game.Instance.PlayerInstance.Controller.IsByFire)
		{
			this.PerSeconds = secondsForByFireWithCloth;
		} 
		else if (Game.Instance.PlayerInstance.Controller.IsInWater) 
		{
			this.PerSeconds = secondsForInWaterWithCloth;
		} 
		else 
		{
			this.PerSeconds = defaultWarmthReductionRate.PerSeconds;
		}
	}

	/// <summary>
	/// Gets the temperature based warmth rate.
	/// </summary>
	/// <returns>The temperature based warmth rate.</returns>
	private StatRate GetTemperatureBasedWarmthRate()
	{
		// get game instance's current temperature
		float currentTemperature = Game.Instance.WeatherInstance.WeatherInformation [(int)Weather.Temperature];
		currentTemperature = 10.1f; // temporarily hard-coded value because temperature from weather system instance shows up as 0 for some reason.

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

    public void SetUnitsInShelter(int unit)
    {
        unitsInShelter = unit;
    }
}


