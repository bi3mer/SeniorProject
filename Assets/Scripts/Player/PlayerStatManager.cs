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
	/// Starts the player's stat updates.
	/// </summary>
	public void StartStatUpdates()
	{
		this.StopStats = false;
	}

	/// <summary>
	/// Stops the player's stat updates.
	/// </summary>
	public void StopStatUpdates()
	{
		this.StopStats = true;
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
		private set;
	}
}


