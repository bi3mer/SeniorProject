using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class District
{
    private Bounds bounds;

    /// <summary>
    /// Creates a new district
    /// </summary>
    /// <param name="seedPoint">The seed point that created the district.</param>
    /// <param name="verticies">The edge verticies defining the district.</param>
    /// <param name="configuration">The configuration object defining district characterisitics.</param>
	public District (Vector3 seedPoint, Vector3[] verticies, DistrictConfiguration configuration)
	{
        SeedPoint = seedPoint;
        EdgeVerticies = verticies;
        Configuration = configuration;
        Blocks = new List<Block>();
	}

    /// <summary>
    /// The seed point used to generate the district.
    /// </summary>
    public Vector3 SeedPoint
    {
        get;
        private set;
    }

    /// <summary>
    /// The list of edge verticies.
    /// </summary>
    public Vector3[] EdgeVerticies
    {
        get;
        private set;
    }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
    public String Name
    {
        get
        {
            return Configuration.Name;
        }
    }

    /// <summary>
    /// The list of blocks contained in this district.
    /// </summary>
    public List<Block> Blocks
    {
        get;
        private set;
    }

    /// <summary>
    /// The bounds that conatin all of the district verticies.
    /// </summary>
    public Bounds BoundingBox
    {
        get
        {
            // only calculate the bounds once
            if (bounds.size == Vector3.zero)
            {
                bounds = calculateBounds();
            }
            return bounds;
        }
    }

    /// <summary>
    /// The configuration for constructing buildings in this district.
    /// </summary>
    public DistrictConfiguration Configuration
    {
        get;
        private set;
    }

	/// <summary>
	/// Checks whether the point is within the bounds of the district.
	/// </summary>
	/// <returns><c>true</c>, if point is within district, <c>false</c> otherwise.</returns>
	/// <param name="point">Point to be checked.</param>
	public bool ContainsPoint(Vector2 point)
	{
        Vector2[] edges = new Vector2[EdgeVerticies.Length];
        for (int i = 0; i < EdgeVerticies.Length; ++i)
        {
            edges[i] = GenerationUtility.ToAlignedVector2(EdgeVerticies[i]);
        }
        return GenerationUtility.IsPointInPolygon(point, ref edges);
	}

    /// <summary>
    /// Calculate the bound that encapsulates all the edge vertecies.
    /// </summary>
    /// <returns>Bounds conatining the ede vertecies.</returns>
    private Bounds calculateBounds()
    {
        Bounds bounds = new Bounds(EdgeVerticies[0], Vector3.zero);
        for (int i = 1; i < EdgeVerticies.Length; ++i)
        {
            bounds.Encapsulate(EdgeVerticies[i]);
        }
        return bounds;
    }
}