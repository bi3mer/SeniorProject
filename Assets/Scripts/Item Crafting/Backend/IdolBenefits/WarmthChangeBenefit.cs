using UnityEngine;
using System.Collections;

public class WarmthChangeBenefit : IdolBenefit 
{
	/// <summary>
	/// Gets or sets the warmth rate change units.
	/// </summary>
	/// <value>The rate change units.</value>
	public int RateChangeUnits
	{
		get;
		set;
	}

	/// <summary>
	/// Applies the benefit.
	/// </summary>
	public override void ApplyBenefit()
	{
		Game.Player.Controller.PlayerStatManager.WarmthRate.UseClothRate(RateChangeUnits);
	}

	/// <summary>
	/// Removes the benefit.
	/// </summary>
	public override void RemoveBenefit()
	{
		Game.Player.Controller.PlayerStatManager.WarmthRate.UseDefaultWarmthReductionRate();
	}
}
