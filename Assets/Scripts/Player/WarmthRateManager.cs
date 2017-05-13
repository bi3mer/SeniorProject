using UnityEngine;

public class WarmthRateManager
{
	private const float tempTier6 = 60.0f;
	private const float tempTier5 = 50.0f;
	private const float tempTier4 = 40.0f;
	private const float tempTier3 = 30.0f;
	private const float tempTier2 = 20.0f;
	private const float tempTier1 = 10.0f;
	private const float tempTier0 = 0.0f;
	private const float tempTierNeg = 0.0f;
	private int currentClothUnits;
	private bool isClothOn;

	/// <summary>
	/// Gets the warmth delay.
	/// </summary>
	/// <value>The warmth delay.</value>
	public float WarmthDelay 
	{
		get 
		{
			float delay;
			if (Game.Instance.PlayerInstance.Controller.IsByFire
			    || Game.Instance.PlayerInstance.Controller.IsInShelter) 
			{
				delay = Game.Instance.PlayerInstance.Controller.StatSettings.DefaultWarmthDelay;
			} 
			else 
			{
				delay = GetTemperatureBasedWarmthDelay ();
			}

			return delay;
		}
	}

	/// <summary>
	/// The amount to change the Warmth by on each update.
	/// </summary>
	/// <value>The warmth amount.</value>
	public int WarmthAmount 
	{
		get 
		{
			int warmthAmount = -Game.Instance.PlayerInstance.Controller.StatSettings.DefaultWarmthDecrease;
			if (Game.Instance.PlayerInstance.Controller.IsByFire) 
			{
				warmthAmount = Game.Instance.PlayerInstance.Controller.StatSettings.HeatSourceRateMultiplier;
			}
			if (Game.Instance.PlayerInstance.Controller.IsInShelter) 
			{
				warmthAmount = Game.Instance.PlayerInstance.Controller.StatSettings.ShelterRateMultiplier;
			}
			if (Game.Instance.PlayerInstance.Controller.IsInWater) 
			{
				warmthAmount *= Game.Instance.PlayerInstance.Controller.StatSettings.WaterRateMultiplier;
			}
			if (Game.Instance.PlayerInstance.HealthStatus == PlayerHealthStatus.Pneumonia)
	        {
				warmthAmount *= Game.Instance.PlayerInstance.Controller.StatSettings.PneumoniaRateMultiplier;
	        }
			while (isClothOn) 
			{
				WarmthAmount += currentClothUnits;
			}

			return warmthAmount;
		}
		private set 
		{
			this.WarmthAmount = value;
		}
	}
		
	/// <summary>
	/// Uses the cloth rate.
	/// </summary>
	/// <param name="clothUnits">Cloth units.</param>
	public void UseClothRate(int clothUnits)
	{
		isClothOn = true;
		currentClothUnits = clothUnits;
	}

	/// <summary>
	/// Stops using the cloth rate.
	/// </summary>
	public void StopUsingClothRate()
	{
		isClothOn = false;
	}

	/// <summary>
	/// Gets the temperature based warmth rate.
	/// </summary>
	/// <returns>The temperature based warmth rate.</returns>
	private float GetTemperatureBasedWarmthDelay()
	{
		// get game instance's current temperature
		float currentTemperature = Game.Instance.WeatherInstance.WeatherInformation [(int)Weather.Temperature];

		if (currentTemperature >= tempTier6) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier6Delay;
		} 
		else if (currentTemperature >= tempTier5) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier5Delay;
		} 
		else if (currentTemperature >= tempTier4) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier4Delay;
		} 
		else if (currentTemperature >= tempTier3) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier3Delay;
		} 
		else if (currentTemperature >= tempTier2) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier2Delay;
		} 
		else if (currentTemperature >= tempTier1) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier1Delay;
		} 
		else if (currentTemperature >= tempTier0) 
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.Tier0Delay;
		} 
		else
		{
			return Game.Instance.PlayerInstance.Controller.StatSettings.TierNegativeDelay;
		}
	}
}


