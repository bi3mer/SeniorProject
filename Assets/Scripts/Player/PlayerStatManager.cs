using UnityEngine;

public class PlayerStatManager
{
	private Player player;
	// possibly get unity events from player controller

	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerStatManager"/> class.
	/// </summary>
	public PlayerStatManager ()
	{
		player = Game.Instance.PlayerInstance;

		// set current rates to the default ones initially
		this.HungerRate = new HungerRateManager ();
		this.WarmthRate = new WarmthRateManager ();
		this.HealthRate = new HealthRateManager ();
	}

	//[Tooltip("The warmth rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets the warmth rate.
	/// </summary>
	/// <value>The warmth rate.</value>
	public StatRate WarmthRate 
	{
		get;
	}

	//[Tooltip("The hunger rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets the hunger rate.
	/// </summary>
	/// <value>The hunger rate.</value>
	public StatRate HungerRate 
	{
		get;
	}

	//[Tooltip("The health rate that increases or decreases in units(+/-) per seconds")]
	/// <summary>
	/// Gets the health rate.
	/// </summary>
	/// <value>The health rate.</value>
	public StatRate HealthRate 
	{
		get;
	}
}


