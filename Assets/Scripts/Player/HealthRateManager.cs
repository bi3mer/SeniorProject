using UnityEngine;

public class HealthRateManager : StatRate
{
	// Add additional rate presets here
	private StatRate hungerZeroHealthReductionRate;
	private StatRate warmthZeroHealthReductionRate;

	// set default values for health rate
	private StatRate defaultHealthRate;
	private const int defaultUnitHealth = 0;
	private const int defaultSecondsHealth = 1;

	/// <summary>
	/// Initializes a new instance of the <see cref="HealthRateManager"/> class.
	/// </summary>
	public HealthRateManager ()
	{
		// set this instance's initial (default) rate
		defaultHealthRate = new StatRate(defaultUnitHealth, defaultSecondsHealth);
		UseDefaultHealthRate ();

		// set initial stat value
		this.CurrentStat = Game.Instance.PlayerInstance.Health;
		this.MaxStat = Game.Instance.PlayerInstance.MaxHealth;

		// set other rate presets
		hungerZeroHealthReductionRate = new StatRate(-1, 20); // as per gdd guidelines
		warmthZeroHealthReductionRate = new StatRate(-1, 1); // as per gdd guidelines
	}

	/// <summary>
	/// Uses the default health rate.
	/// </summary>
	public void UseDefaultHealthRate()
	{
		this.Units = defaultHealthRate.Units;
		this.PerSeconds = defaultHealthRate.PerSeconds;
	}

	/// <summary>
	/// Takes the fall damage.
	/// </summary>
	/// <param name="fallDamageAmount">Fall damage amount.</param>
	public void TakeFallDamage(int fallDamageAmount)
	{
		this.Units = -fallDamageAmount;
		this.PerSeconds = 1;
		this.ApplyRateToStat ();
		UseDefaultHealthRate ();
	}

	/// <summary>
	/// Uses the hunge zero health reduction rate.
	/// </summary>
	public void UseHungerZeroHealthReductionRate()
	{
		this.Units = hungerZeroHealthReductionRate.Units;
		this.PerSeconds = hungerZeroHealthReductionRate.PerSeconds;
	}

	/// <summary>
	/// Uses the warmth zero health reduction rate.
	/// </summary>
	public void UseWarmthZeroHealthReductionRate()
	{
		this.Units = warmthZeroHealthReductionRate.Units;
		this.PerSeconds = warmthZeroHealthReductionRate.PerSeconds;
	}
		
	/// <summary>
	/// Gets the current health stat.
	/// </summary>
	/// <value>The current health stat.</value>
	public int CurrentHealthStat
	{
		get 
		{
			return this.CurrentStat;
		}
	}
}


