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
}


