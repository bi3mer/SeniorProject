using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Pressure constants that access readonly array pressureConstants
/// in PressureSystem.
/// </summary>
public enum PressureConstants 
{
	PressureHighMax,
	PressureHighMin,
	PressureLowMax,
	PressureLowMin,
	PressureHurricane
};

public class PressureSystems 
{
	// Values found at: http://www.theweatherprediction.com/habyhints2/410/ and can 
	// be accessed by enumeration PressureConstants
	private readonly float[] pressureConstants = {1086f, 1000f, 1000f, 980f, 870f};

	// TODO temporary
	private const float vectorMultiplier = 10f;
	private const float angleIncrementer = 0.01f;
	private float twoPi;

	public List<PressureSystem> LocalPressureSystems
	{
		get;
		private set;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PressureSystem"/> class.
	/// </summary>
	public PressureSystems()
	{
		this.twoPi = 2 * Mathf.PI;

		// initialize pressure systems
		this.LocalPressureSystems = new List<PressureSystem>();

		// TODO temporary stuff here. Ignore it for now
		PressureSystem t1 = new PressureSystem();
		PressureSystem t2 = new PressureSystem();
		t1.Position = Vector2.right * PressureSystems.vectorMultiplier;
		t2.Position = Vector2.left  * PressureSystems.vectorMultiplier;
		t1.IsHighPressure = true;
		t2.IsHighPressure = false;
		t1.Pressure = this.pressureConstants[(int) PressureConstants.PressureHighMin];
		t2.Pressure = this.pressureConstants[(int) PressureConstants.PressureLowMin];
		t1.Angle = 0f;
		t2.Angle = Mathf.PI;

		this.LocalPressureSystems.Add(t1);
		this.LocalPressureSystems.Add(t2);
	}

	/// <summary>
	/// Subscribed to clock delegate for every second to update. This method
	/// will move pressure systems around the world.
	/// </summary>
	public void UpdatePressureSystem()
	{
		// update positions of pressure system
		// TODO temporary solution to showing moving pressure systems. 
		//      Over the winter the position will be based on something
		//      more substantial than a circle
		for(int i = 0; i < this.LocalPressureSystems.Count; ++i)
		{	
			if(this.LocalPressureSystems[i].Angle >= this.twoPi)
			{
				this.LocalPressureSystems[i].Angle = 0;
			}
			else
			{
				float percentage = (this.LocalPressureSystems[i].Angle / this.twoPi) + PressureSystems.angleIncrementer;
				this.LocalPressureSystems[i].Angle = Mathf.Lerp(0, this.twoPi, percentage);
			}

			this.LocalPressureSystems[i].Position = new Vector2(Mathf.Cos(this.LocalPressureSystems[i].Angle),
			                                                    Mathf.Sin(this.LocalPressureSystems[i].Angle))
			                                      * PressureSystems.vectorMultiplier; 
		}
	}

	/// <summary>
	/// Gets the closest pressure system.
	/// </summary>
	/// <returns>The closest pressure system.</returns>
	/// <param name="position">Position.</param>
	public PressureSystem GetClosestPressureSystem(Vector2 position)
	{
		PressureSystem closest = this.LocalPressureSystems[0];
		float dist             = Vector2.Distance(closest.Position, position);

		for(int i = 1; i < this.LocalPressureSystems.Count; ++i)
		{
			// calculate distance
			float newDist = Vector2.Distance(this.LocalPressureSystems[i].Position, position);

			// check if closer pressure system is found
			if(newDist < dist)
			{
				dist    = newDist;
				closest = this.LocalPressureSystems[i]; 
			}
		}

		return closest;
	}
}