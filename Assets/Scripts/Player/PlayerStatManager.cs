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
		if (Game.Instance.PlayerInstance.Warmth == 0) 
		{
			this.HealthRate.UseWarmthZeroHealthReductionRate ();
		} 
		else if (Game.Instance.PlayerInstance.Hunger == 0) 
		{
			this.HealthRate.UseHungerZeroHealthReductionRate ();
		}
	}

	/// <summary>
	/// Starts the player's stat updates.
	/// </summary>
	public void StartStatUpdates()
	{
		this.StopStats = false;
//		this.WarmthRate.UseDefaultWarmthReductionRate ();
//		this.HungerRate.UseDefaultHungerReductionRate ();
		this.HealthRate.UseDefaultHealthRate ();
	}

	/// <summary>
	/// Stops the player's stat updates.
	/// </summary>
	public void StopStatUpdates()
	{
		this.StopStats = true;
//		this.WarmthRate.ChangeRateValues (0, 1);
//		this.HungerRate.ChangeRateValues (0, 1);
		this.HealthRate.ChangeRateValues (0, 1);
//		this.WarmthRate.ApplyRateToStat ();
//		this.HungerRate.ApplyRateToStat ();
		this.HealthRate.ApplyRateToStat ();
	}

	/// <summary>
	/// Gets the warmth rate.
	/// </summary>
	/// <value>The warmth rate.</value>
	public WarmthRateManager WarmthRate 
	{
		get;
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

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="PlayerStatManager"/> stop stats.
	/// </summary>
	/// <value><c>true</c> if stop stats; otherwise, <c>false</c>.</value>
	public bool StopStats 
	{
		get;
		set;
	}
}


