using UnityEngine;

public class HungerRateManager : StatRate
{
	// set default values for hunger rate
	private StatRate defaultHungerRate;
	private const int defaultUnitHunger = -1;
	private const int defaultSecondsHunger = 7;

	/// <summary>
	/// Initializes a new instance of the <see cref="HungerRateManager"/> class.
	/// </summary>
	public HungerRateManager ()
	{
		// set this instance's initial (default) rate
		defaultHungerRate = new StatRate (defaultUnitHunger, defaultSecondsHunger);
		UseDefaultHungerReductionRate ();

		// set initial stat value
		this.CurrentStat = Game.Instance.PlayerInstance.Hunger;
		this.MaxStat = Game.Instance.PlayerInstance.MaxHunger;
	}

	/// <summary>
	/// Uses the default hunger reduction rate.
	/// </summary>
	public void UseDefaultHungerReductionRate()
	{
		this.Units = defaultHungerRate.Units;
		this.PerSeconds = defaultHungerRate.PerSeconds;
	}

	/// <summary>
	/// Uses the food energy.
	/// </summary>
	/// <param name="amountOfHungerUnitsAffected">Amount of hunger units affected.</param>
	public void UseFoodEnergy(int amountOfHungerUnitsAffected)
	{
		this.Units = amountOfHungerUnitsAffected;
		this.PerSeconds = 1;
		this.ApplyRateToStat ();
		UseDefaultHungerReductionRate (); // change back to default reduction rate after consuming food and accounting for it's energy
	}
}


