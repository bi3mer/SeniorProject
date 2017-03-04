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

	public void ApplyCorrectHealthReductionRate()
	{
		if (this.WarmthRate.CurrentWarmthStat == 0) 
		{
			this.HealthRate.UseWarmthZeroHealthReductionRate ();
		} 
		else if (this.HungerRate.CurrentHungerStat == 0) 
		{
			this.HealthRate.UseHungeZeroHealthReductionRate ();
		}
		else if (this.HealthRate.CurrentHealthStat == 0)
		{
			// you are dead af
		}
	}

	//[Tooltip("The warmth rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets the warmth rate.
	/// </summary>
	/// <value>The warmth rate.</value>
	public WarmthRateManager WarmthRate 
	{
		get;
	}

	//[Tooltip("The hunger rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets the hunger rate.
	/// </summary>
	/// <value>The hunger rate.</value>
	public HungerRateManager HungerRate 
	{
		get;
	}

	//[Tooltip("The health rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets the health rate.
	/// </summary>
	/// <value>The health rate.</value>
	public HealthRateManager HealthRate 
	{
		get;
	}
}


