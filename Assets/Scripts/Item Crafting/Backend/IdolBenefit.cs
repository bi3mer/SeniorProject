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
	}

	/// <summary>
	/// Deactivates the benefit.
	/// </summary>
	public void DeactivateBenefit()
	{
	}
}
