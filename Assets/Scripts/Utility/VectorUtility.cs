using UnityEngine;

public class VectorUtility 
{
	/// <summary>
	/// Gets the slope.
	/// </summary>
	/// <returns>The slope.</returns>
	/// <param name="posOne">Position one.</param>
	/// <param name="posTwo">Position two.</param>
	public static float GetSlope(Vector2 posOne, Vector2 posTwo)
	{
		// ensure that divide by 0 case doesn't occur
		float divisor = posTwo.x - posOne.x;
		if(divisor == 0)
		{
			return Mathf.Infinity;
		}

		return (posTwo.y - posOne.y) / divisor;
	}

	/// <summary>
	/// Gets the perpindicular slope.
	/// </summary>
	/// <returns>The perpindicular slope.</returns>
	/// <param name="posOne">Position one.</param>
	/// <param name="posTwo">Position two.</param>
	public static float GetPerpindicularSlope(Vector2 posOne, Vector2 posTwo)
	{
		float divisor = VectorUtility.GetSlope(posOne, posTwo);
		if(divisor == 0)
		{
			return Mathf.Infinity;
		}

		return -1 / divisor;
	}


	/// <summary>
	/// Gets the tangent angle between two points
	/// </summary>
	/// <returns>The wind angle.</returns>
	/// <param name="posOne">Position one.</param>
	/// <param name="posTwo">Position two.</param>
	public static float GetAngle(Vector2 posOne, Vector2 posTwo)
	{
		float adjacent = posTwo.y - posOne.y;
		float opposite = posTwo.x - posOne.x;

		return Mathf.Atan2(opposite, adjacent);
	}

	/// <summary>
	/// Convert a vector3 to vector2 using the z axis
	/// instead of the y
	/// </summary>
	/// <param name="vector">Vector.</param>
	public static Vector2 XZ(Vector3 vector)
	{
		return new Vector2(vector.x, vector.z);
	}

	/// <summary>
	/// Convert a 2d vector to 3d with a y value
	/// </summary>
	/// <returns>The dimensional3d.</returns>
	/// <param name="vector">Vector.</param>
	/// <param name="y_val">Y value.</param>
	public static Vector3 twoDimensional3d(Vector2 vector, float yValue = 0)
	{
		return new Vector3(vector.x, yValue, vector.y); 
	}

	/// <summary>
	/// Initialize a zero 3d vector with the y value defined
	/// </summary>
	/// <param name="y">The y coordinate.</param>
	public static Vector3 HeightVector3d(float y)
	{
		return new Vector3(0,y,0);
	}

	/// <summary>
	/// Initialize a zero 2d vector with the y value defined
	/// </summary>
	/// <param name="y">The y coordinate.</param>
	public static Vector2 HeightVector2d(float y)
	{
		return new Vector2(0, y);
	}
}
