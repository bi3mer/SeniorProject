using UnityEngine;

public class CityBoundaries 
{
	/// <summary>
	/// The city bounds.
	/// </summary>
	public Bounds CityBounds;

	/// <summary>
	/// Initializes a new instance of the <see cref="CityBoundaries"/> class.
	/// </summary>
	public CityBoundaries()
	{
#if UNITY_EDITOR
		// in a scene that doens't have city generation give this a default value
		// so other portions of the game will run
		this.CityBounds = new Bounds(new Vector3(0,0,0), new Vector3(100,100,100));
#endif
	}

	/// <summary>
	/// Gets a random Vector2 within the city's boundaries.
	/// </summary>
	/// <value>The random vector.</value>
	public Vector2 RandomVector2d
	{
		get
		{
			return new Vector2(Random.Range(this.CityBounds.min.x, this.CityBounds.max.x),
			                   Random.Range(this.CityBounds.min.y, this.CityBounds.max.y));
		}
	}

	/// <summary>
	/// Gets a random Vector3 within the city's boundaries.
	/// </summary>
	/// <value>The random vector3d.</value>
	public Vector3 RandomVector3d
	{
		get
		{
			return new Vector3(Random.Range(this.CityBounds.min.x, this.CityBounds.max.x),
			                   Random.Range(this.CityBounds.min.y, this.CityBounds.max.y),
			                   Random.Range(this.CityBounds.min.z, this.CityBounds.max.z));
		}
	}

	/// <summary>
	/// Takes a vector2 and clamps its values so it cannot extend outside
	/// of the city's boundaries. 
	/// </summary>
	/// <returns>The vector.</returns>
	/// <param name="vector">Vector.</param>
	public Vector2 BoundVector2d(Vector2 vector)
	{
		return new Vector2(Mathf.Clamp(vector.x, this.CityBounds.min.x, this.CityBounds.max.x),
		                   Mathf.Clamp(vector.y, this.CityBounds.min.y, this.CityBounds.max.y));
	}

	/// <summary>
	/// Takes a vector3 and clamps its values so it cannot extend outside
	/// of the city's boundaries.
	/// </summary>
	/// <returns>The vector3d.</returns>
	/// <param name="vector">Vector.</param>
	public Vector3 BoundVector3d(Vector3 vector)
	{
		return new Vector3(Mathf.Clamp(vector.x, this.CityBounds.min.x, this.CityBounds.max.x),
	                       Mathf.Clamp(vector.y, this.CityBounds.min.y, this.CityBounds.max.y),
						   Mathf.Clamp(vector.z, this.CityBounds.min.z, this.CityBounds.max.z));
	}
}
