using UnityEngine;

public class StatRate
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StatRate"/> class.
	/// </summary>
	/// <param name="units">Units.</param>
	/// <param name="perSeconds">Per seconds.</param>
	public StatRate (int units, int perSeconds)
	{
		this.ChangeRateValues (units, perSeconds);
	}

	/// <summary>
	/// Changes the rate values.
	/// </summary>
	/// <param name="newUnits">New units.</param>
	/// <param name="newSeconds">New seconds.</param>
	public void ChangeRateValues(int newUnits, int newSeconds)
	{
		this.Units = newUnits;
		this.PerSeconds = newSeconds;
	}

	/// <summary>
	/// Applies the rate to stat.
	/// </summary>
	public void ApplyRateToStat()
	{
		if (this.Units < 0) 
		{
			DecreaseStatByGivenUnits (this.Units);
		} 
		else if (this.Units > 0) 
		{
			IncreaseStatByGivenUnits (this.Units);
		}
	}

	/// <summary>
	/// Decreases the stat by given units.
	/// </summary>
	/// <param name="decreaseUnits">Decrease units.</param>
	protected void DecreaseStatByGivenUnits(int decreaseUnits)
	{
		// don't let stat fall below 0
		if (this.CurrentStat - decreaseUnits <= 0) 
		{
			this.CurrentStat = 0;
		} 
		else 
		{
			this.CurrentStat -= decreaseUnits;
		}
	}

	/// <summary>
	/// Increases the stat by given units.
	/// </summary>
	/// <param name="increaseUnits">Increase units.</param>
	protected void IncreaseStatByGivenUnits(int increaseUnits)
	{
		// don't let stat exceed maximum
		if (this.CurrentStat + increaseUnits >= this.MaxStat) 
		{
			this.CurrentStat = this.MaxStat;
		} 
		else 
		{
			this.CurrentStat += increaseUnits;
		}
	}

	/// <summary>
	/// Gets the number of units that should decrease or increase.
	/// </summary>
	/// <value>The units.</value>
	public int Units
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the interval of seconds that the units should change after.
	/// </summary>
	/// <value>The seconds.</value>
	public int PerSeconds 
	{ 
		get; 
		private set; 
	}

	/// <summary>
	/// Gets the current health stat.
	/// </summary>
	/// <value>The current health stat.</value>
	protected int CurrentStat 
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets or sets the max stat.
	/// </summary>
	/// <value>The max stat.</value>
	protected int MaxStat 
	{
		get;
		private set;
	}
}


