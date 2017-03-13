using UnityEngine;

public class StatRate
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StatRate"/> class.
	/// </summary>
	protected StatRate() { }

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
		this.CurrentStat = Mathf.Clamp (this.CurrentStat + this.Units, 0, this.MaxStat);
	}

	/// <summary>
	/// Gets the number of units that should decrease or increase.
	/// </summary>
	/// <value>The units.</value>
	public int Units
	{
		get;
		protected set;
	}

	/// <summary>
	/// Gets the interval of seconds that the units should change after.
	/// </summary>
	/// <value>The seconds.</value>
	public int PerSeconds 
	{ 
		get; 
		protected set; 
	}

	/// <summary>
	/// Gets the current health stat.
	/// </summary>
	/// <value>The current health stat.</value>
	public int CurrentStat 
	{
		get;
		protected set;
	}

	/// <summary>
	/// Gets or sets the max stat.
	/// </summary>
	/// <value>The max stat.</value>
	protected int MaxStat 
	{
		get;
		set;
	}
}


