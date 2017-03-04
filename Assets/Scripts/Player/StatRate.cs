using UnityEngine;

public class StatRate
{
	// keeps track of actual stat
	private int currentStat;

	/// <summary>
	/// Initializes a new instance of the <see cref="StatRate"/> class.
	/// </summary>
	/// <param name="units">Units.</param>
	/// <param name="perSeconds">Per seconds.</param>
	public StatRate (int units, int perSeconds)
	{
		this.Units = units;
		this.PerSeconds = perSeconds;
	}

	/// <summary>
	/// Gets or sets the number of units that should decrease or increase.
	/// </summary>
	/// <value>The units.</value>
	public int Units
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the interval of seconds that the units should change after.
	/// </summary>
	/// <value>The seconds.</value>
	public int PerSeconds 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Decreases the stat by given units.
	/// </summary>
	/// <param name="decreaseUnits">Decrease units.</param>
	public void DecreaseStatByGivenUnits(int decreaseUnits)
	{
		this.currentStat -= decreaseUnits;
	}

	/// <summary>
	/// Increases the stat by given units.
	/// </summary>
	/// <param name="increaseUnits">Increase units.</param>
	public void IncreaseStatByGivenUnits(int increaseUnits)
	{
		this.currentStat += increaseUnits;
	}

	/// <summary>
	/// Gets the current health stat.
	/// </summary>
	/// <value>The current health stat.</value>
	public int CurrentStat 
	{
		private get 
		{
			return currentStat;
		}

		private set;
	}
}


