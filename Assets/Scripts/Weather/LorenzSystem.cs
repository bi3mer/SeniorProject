using UnityEngine;
using System.Collections;

public class LorenzSystem 
{
	// Default lorenz attractor constants
	public const float S  = 10.0f;
	public const float R  = 28.0f;
	public const float B  = 2.66666666667f; // 8/3
	public const float DT = 0.01f;

	private float currentS;
	private float currentR;
	private float currentB;
	private float currentDT;

	/// <summary>
	/// Calcualte Lorenz Point
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="coordinate">Coordinate.</param>
	public Vector3 LorenzPoint(Vector3 coordinate)
	{
		// Initialzie array
		Vector3 lorenzPoint = Vector3.zero;

		// calculate coordiantes
		lorenzPoint.x = this.currentS * (coordinate.y - coordinate.x);
		lorenzPoint.y = (this.currentR * coordinate.x) - coordinate.y - (coordinate.x * coordinate.z);
		lorenzPoint.z = (coordinate.x * coordinate.y) - (this.currentB * coordinate.z);

		return lorenzPoint;
	}

	/// <summary>
	/// Gets the lorenz centers for the y and z plane. These
	/// are the centers of the low and high pressure systems. 
	/// For hurricane systems only the second element of the 
	/// array returned will be used.
	/// </summary>
	/// <returns>The lorenz centers.</returns>
	public Vector2[] GetLorenzCenters()
	{
		Vector2[] lorenzCenters = new Vector2[2];

		// Calcualte y and z coordinates
		float y = Mathf.Sqrt(this.currentB * (this.currentR - 1));
		float z = this.currentR - 1;

		// Update array
		lorenzCenters[0] = new Vector2(y, -y);
		lorenzCenters[1] = new Vector2(y, z);

		return lorenzCenters;
	}
}
