using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class District
{
	private Vector2[] edgeVerticies;
	private Vector2   cityCenter;
	private String 	  districtName;
    private Bounds    bounds;

	/// <summary>
	/// Initializes a new instance of the <see cref="AssemblyCSharp.District"/> class.
	/// </summary>
	/// <param name="center">Center point of the city as determined by DistrictsGenerator.</param>
	public District (Vector2 center)
	{
		cityCenter = center;
        Blocks = new List<CityBlock>();
	}
		
	/// <summary>
	/// Gets or sets the verticies.
	/// </summary>
	/// <value>The verticies.</value>
	public Vector2[] Verticies
	{ 
		get
		{ 
			return edgeVerticies;
		} 

		set 
		{
			edgeVerticies = value;
		}
	}

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
	public String Name
	{
		get 
		{
			return districtName;
		}

		set
		{
			districtName = value;
		}
	}

    /// <summary>
    /// The list of blocks contained in this district.
    /// </summary>
    public List<CityBlock> Blocks
    {
        get;
        private set;
    }

    public void AddBlock (CityBlock block)
    {
        Blocks.Add(block);
    }

    /// <summary>
    /// The bounds that conatin all of the district verticies.
    /// </summary>
    public Bounds BoundingArea
    {
        get
        {
            // only calculate the bounds once
            if (bounds == null)
            {
                bounds = calculateBounds();
            }
            return bounds;
        }
    }

	/// <summary>
	/// Checks whether the point is within the bounds of the district.
	/// </summary>
	/// <returns><c>true</c>, if point is within district, <c>false</c> otherwise.</returns>
	/// <param name="point">Point to be checked.</param>
	public bool ContainsPoint(Vector2 point)
	{

		// check whether this point's x value is between the district's edges & the city center
		if (point.x < edgeVerticies [0].x && point.x < edgeVerticies [1].x && point.x < cityCenter.x) 
		{
			return false;
		}
		if (point.x > edgeVerticies [0].x && point.x > edgeVerticies [1].x && point.x > cityCenter.x) 
		{
			return false;
		}

		// check whether this point's x value is between the district's edges & the city center
		if (point.y < edgeVerticies [0].y && point.y < edgeVerticies [1].y && point.y < cityCenter.y) 
		{
			return false;
		}

		if (point.y > edgeVerticies [0].y && point.y > edgeVerticies [1].y && point.y > cityCenter.y) 
		{
			return false;
		}

		return true;
	}

    private Bounds calculateBounds()
    {
        Bounds bounds = new Bounds(Verticies[0], Vector3.zero);
        for (int i = 1; i < Verticies.Length; ++i)
        {
            bounds.Encapsulate(Verticies[i]);
        }
        return bounds;
    }
}