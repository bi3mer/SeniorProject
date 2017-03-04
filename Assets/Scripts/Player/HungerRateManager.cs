using UnityEngine;

public class HungerRateManager : StatRate
{
	// keeps track of actual stat 
	private int currentHungerStat;

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
		this.Units = defaultHungerRate.Units;
		this.PerSeconds = defaultHungerRate.PerSeconds;
	}

	/// <summary>
	/// Gets the current hunger stat.
	/// </summary>
	/// <value>The current hunger stat.</value>
	public int CurrentHungerStat
	{
		get 
		{
			return currentHungerStat;
		}
	}
}


