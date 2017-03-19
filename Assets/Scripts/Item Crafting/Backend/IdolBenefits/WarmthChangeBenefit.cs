using UnityEngine;
using System.Collections;

public class WarmthChangeBenefit : IdolBenefit 
{
	public int RateChangeUnits
	{
		get;
		set;
	}

	public override void ApplyBenefit()
	{
		Game.Player.Controller.PlayerStatManager.WarmthRate.UseClothRate(RateChangeUnits);
	}

	public override void RemoveBenefit()
	{
		Game.Player.Controller.PlayerStatManager.WarmthRate.UseDefaultWarmthReductionRate();
	}
}
