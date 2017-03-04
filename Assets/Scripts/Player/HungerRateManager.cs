using UnityEngine;

public class HungerRateManager : StatRate
{
	private StatRate defaultHungerRate;

	// set default values for hunger rates
	private const int defaultUnitHunger = -1;
	private const int defaultSecondsHunger = 7;

	public HungerRateManager ()
	{
		defaultHungerRate = new StatRate (defaultUnitHunger, defaultSecondsHunger);
		this.Units = defaultHungerRate.Units;
		this.PerSeconds = defaultHungerRate.PerSeconds;
	}
}


