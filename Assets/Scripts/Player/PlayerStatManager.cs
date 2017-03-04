using UnityEngine;

public class PlayerStatManager
{
	private Player player;
	// possibly get unity events from player controller

	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerStatManager"/> class.
	/// </summary>
	/// <param name="playerInstance">Player instance.</param>
	public PlayerStatManager (Player playerInstance)
	{
		player = playerInstance;

		// set current rates to the default ones initially
		this.HungerRate = new HungerRateManager ();
		this.WarmthRate = new WarmthRateManager ();
		this.HealthRate = new HealthRateManager ();
	}

	//[Tooltip("The warmth rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets or sets the warmth rate (can increase or decrease in units(+/-) per seconds).
	/// </summary>
	/// <value>The warmth rate.</value>
	public StatRate WarmthRate 
	{
		get;
		set;
	}

	//[Tooltip("The hunger rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets or sets the hunger rate (can increase or decrease in units(+/-) per seconds).
	/// </summary>
	/// <value>The hunger rate.</value>
	public StatRate HungerRate 
	{
		get;
		set;
	}

	//[Tooltip("The health rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets or sets the health rate (can increase or decrease in units(+/-) per seconds).
	/// </summary>
	/// <value>The health rate.</value>
	public StatRate HealthRate 
	{
		get;
		set;
	}
}


