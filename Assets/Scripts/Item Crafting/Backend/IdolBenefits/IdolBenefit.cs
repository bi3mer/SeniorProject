using UnityEngine;
using System.Collections;

public abstract class IdolBenefit 
{
	/// <summary>
	/// Applies the benefit of the idol to the player.
	/// </summary>
	public abstract void ApplyBenefit();

	/// <summary>
	/// Removes the benefit.
	/// </summary>
	public abstract void RemoveBenefit();
}
