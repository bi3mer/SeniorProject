using UnityEngine;

public class HealthRateManager : StatRate
{
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
		this.Units = defaultHealthRate.Units;
		this.PerSeconds = defaultHealthRate.PerSeconds;

		// set initial stat value
		this.CurrentStat = Game.Instance.PlayerInstance.Health;
		this.MaxStat = Game.Instance.PlayerInstance.MaxHealth;
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


