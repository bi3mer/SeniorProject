using UnityEngine;
using System.Collections;

public class IdolBenefit 
{
	public static string HEALTH = "health";

	public static string WARMTH = "warmth";

	public static string HUNGER = "hunger";

	private string selectedBenefit;

	/// <summary>
	/// Initializes a new instance of the <see cref="IdolBenefit"/> class.
	/// </summary>
	/// <param name="benefit">Benefit.</param>
	public IdolBenefit(string benefit)
	{
		selectedBenefit = benefit;
	}

	/// <summary>
	/// Activates the benefit.
	/// </summary>
	public void ActivateBenefit()
	{
		if(selectedBenefit.Equals(HEALTH))
		{
			healthBenefit();
		}
		else if(selectedBenefit.Equals(HUNGER))
		{
			hungerBenefit();
		}
		else if(selectedBenefit.Equals(WARMTH))
		{
			warmthBenefit();
		}
	}

	/// <summary>
	/// Deactivates the benefit.
	/// </summary>
	public void DeactivateBenefit()
	{
		if(selectedBenefit.Equals(HEALTH))
		{
			stopHealthBenefit();
		}
		else if(selectedBenefit.Equals(HUNGER))
		{
			stopHungerBenefit();
		}
		else if(selectedBenefit.Equals(WARMTH))
		{
			stopWarmthBenefit();
		}
	}

	private void healthBenefit()
	{
		/// TODO: Add changes to health stat rate here
	}

	private void stopHealthBenefit()
	{
		/// TODO: Stop changes to health stat rate here
	}

	private void hungerBenefit()
	{
		/// TODO: Add changes to hunger stat rate here
	}

	private void stopHungerBenefit()
	{
		/// TODO: Stop changes to hunger stat rate here
	}

	private void warmthBenefit()
	{
		/// TODO: Add changes to warmth stat rate here
	}

	private void stopWarmthBenefit()
	{
		/// TODO: Stop changes to warmth stat rate here
	}
}
