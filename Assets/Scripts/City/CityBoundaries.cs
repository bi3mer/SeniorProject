using UnityEngine;

public class CityBoundaries 
{
	/// <summary>
	/// The city bounds.
	/// </summary>
	public Bounds CityBounds;

    private Vector2[] edges;

    /// <summary>
    /// Get list of edges on the xz plane.
    /// </summary>
    public Vector2[] Edges
    {
        get
        {
            if (this.edges == null)
            {
                // calculate the edges if we haven't already with a size of 4 (four corners)
                this.edges = new Vector2[4];

                // set each corner
                this.edges[0] = new Vector3(this.CityBounds.min.x, this.CityBounds.min.z);
                this.edges[1] = new Vector3(this.CityBounds.min.x, this.CityBounds.max.z);
                this.edges[2] = new Vector3(this.CityBounds.max.x, this.CityBounds.min.z);
                this.edges[3] = new Vector3(this.CityBounds.max.x, this.CityBounds.max.z);
            }

            return this.edges;
        }
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
