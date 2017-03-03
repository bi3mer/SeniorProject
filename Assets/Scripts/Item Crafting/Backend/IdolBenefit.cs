using UnityEngine;
using System.Collections;

public class IdolBenefit 
{
	public static string Health = "health";

	public static string Warmth = "warmth";

	public static string Hunger = "hunger";

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
