using UnityEngine;

public class HealthRateManager : StatRate
{
	// Add additional rate presets here
//	private StatRate hungerZeroHealthReductionRate;
//	private const int hungerZeroUnit = -1;
//	private const int hungerZeroSeconds = 20;
//	private StatRate warmthZeroHealthReductionRate;
//	private const int warmthZeroUnit = -1;
//	private const int warmthZeroSeconds = 1;
//
//	// set default values for health rate
//	private StatRate defaultHealthRate;
//	private const int defaultUnitHealth = 0;
//	private const int defaultSecondsHealth = 1;

	/// <summary>
	/// Initializes a new instance of the <see cref="HealthRateManager"/> class.
	/// </summary>
//	public HealthRateManager ()
//	{
////		// set this instance's initial (default) rate
////		defaultHealthRate = new StatRate(defaultUnitHealth, defaultSecondsHealth);
////		UseDefaultHealthRate ();
////
////		// set initial stat value
////		this.CurrentStat = Game.Instance.PlayerInstance.Health;
////		this.MaxStat = Game.Instance.PlayerInstance.MaxHealth;
////
////		// set other rate presets as per gdd guidelines
////		hungerZeroHealthReductionRate = new StatRate(hungerZeroUnit, hungerZeroSeconds); 
////		warmthZeroHealthReductionRate = new StatRate(warmthZeroUnit, warmthZeroSeconds);
//	}

	/// <summary>
	/// Gets the health delay.
	/// </summary>
	/// <value>The health delay.</value>
	public int HealthDelay
	{
		get 
		{
			int delay = 0;
			if (Game.Instance.PlayerInstance.Hunger == 0) 
			{
				delay += Game.Instance.PlayerInstance.Controller.StatSettings.ZeroHungerDelay;
			}
			if (Game.Instance.PlayerInstance.Warmth == 0) 
			{
				delay += Game.Instance.PlayerInstance.Controller.StatSettings.ZeroWarmthDelay;
			}
			else
			{
				delay = Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDelay;
			}

			return delay;
		}
	}

	/// <summary>
	/// Gets the health amount.
	/// </summary>
	/// <value>The health amount.</value>
	public int HealthAmount 
	{
		get 
		{
			int healthAmount = 0;
			if (Game.Instance.PlayerInstance.Hunger == 0)  
			{
				healthAmount -= Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDecrease;
			}
			if (Game.Instance.PlayerInstance.Warmth == 0) 
			{
				healthAmount -= Game.Instance.PlayerInstance.Controller.StatSettings.DefaultHealthDecrease;
			}

			return healthAmount;
		}
	}

//	/// <summary>
//	/// Uses the default health rate.
//	/// </summary>
//	public void UseDefaultHealthRate()
//	{
//		this.Units = defaultHealthRate.Units;
//		this.PerSeconds = defaultHealthRate.PerSeconds;
//	}

	/// <summary>
	/// Takes the fall damage.
	/// </summary>
	/// <param name="fallDamageAmount">Fall damage amount.</param>
	public void TakeFallDamage(int fallDamageAmount)
	{
//		this.Units = -fallDamageAmount;
//		this.PerSeconds = 1;
//		this.ApplyRateToStat ();
//		UseDefaultHealthRate ();
		Game.Instance.PlayerInstance.Health -= fallDamageAmount;
	}

//	/// <summary>
//	/// Uses the hunger zero health reduction rate.
//	/// </summary>
//	public void UseHungerZeroHealthReductionRate()
//	{
////		this.Units = hungerZeroHealthReductionRate.Units;
////		this.PerSeconds = hungerZeroHealthReductionRate.PerSeconds;
//	}
//
//	/// <summary>
//	/// Uses the warmth zero health reduction rate.
//	/// </summary>
//	public void UseWarmthZeroHealthReductionRate()
//	{
////		this.Units = warmthZeroHealthReductionRate.Units;
////		this.PerSeconds = warmthZeroHealthReductionRate.PerSeconds;
//	}

	/// <summary>
	/// Uses the health energy.
	/// </summary>
	/// <param name="amountOfHealthUnitsAffected">Amount of health units affected.</param>
	public void UseHealthEnergy(int amountOfHealthUnitsAffected)
	{
		Game.Instance.PlayerInstance.Health += amountOfHealthUnitsAffected;
//		this.Units = amountOfHealthUnitsAffected;
//		this.PerSeconds = 1;
//		this.ApplyRateToStat ();
//		UseDefaultHealthRate (); // change back to default reduction rate after consuming food and accounting for it's energy
	}
}


