using UnityEngine;

public class PlayerStatManager
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerStatManager"/> class.
	/// </summary>
	public PlayerStatManager ()
	{
		// set current rates to the default ones initially
		this.HungerRate = new HungerRateManager ();
		this.WarmthRate = new WarmthRateManager ();
		this.HealthRate = new HealthRateManager ();
	}

	/// <summary>
	/// Applies the correct health reduction rate based on hunger and warmth stats.
	/// </summary>
	public void ApplyCorrectHealthReductionRate()
	{
		if (this.WarmthRate.CurrentWarmthStat == 0) 
		{
			this.HealthRate.UseWarmthZeroHealthReductionRate ();
		} 
		else if (this.HungerRate.CurrentHungerStat == 0) 
		{
			this.HealthRate.UseHungerZeroHealthReductionRate ();
		}
		else if (this.HealthRate.CurrentHealthStat == 0)
		{
			// you are dead af
		}
	}

	/// <summary>
	/// Gets the warmth rate.
	/// </summary>
	/// <value>The warmth rate.</value>
	public WarmthRateManager WarmthRate 
	{
		get ;
		private set;
	}

	/// <summary>
	/// Gets the hunger rate.
	/// </summary>
	/// <value>The hunger rate.</value>
	public HungerRateManager HungerRate 
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the health rate.
	/// </summary>
	/// <value>The health rate.</value>
	public HealthRateManager HealthRate 
	{
		get;
		private set;
	}
}


