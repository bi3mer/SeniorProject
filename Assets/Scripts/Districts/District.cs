using System;
using UnityEngine;
using System.Collections;

public class District
{
	[SerializeField]
	private Vector2[] edgeVerticies;

	[SerializeField]
	private Vector2   cityCenter;

	[SerializeField]
	private String 	  districtName;

	/// <summary>
	/// Initializes a new instance of the <see cref="AssemblyCSharp.District"/> class.
	/// </summary>
	/// <param name="center">Center point of the city as determined by DistrictsGenerator.</param>
	public District (Vector2 center)
	{
		cityCenter = center;
	}

	/// <summary>
	/// Sets the verticies of the edges of the district.
	/// </summary>
	/// <param name="verts">Vector of verticies to set.</param>
	public void setVerticies(Vector2[] verts)
	{
		edgeVerticies = verts;
	}

	/// <summary>
	/// Gets the verticies that define the edges of this district.
	/// </summary>
	/// <returns>The verticies that make of this district's edges.</returns>
	public Vector2[] getVerticies()
	{
		return edgeVerticies;
	}

	/// <summary>
	///  Sets the name of the district.
	/// </summary>
	/// <param name="name">Name to be set.</param>
	public void setName(String name)
	{
		districtName = name;
	}

	/// <summary>
	///  Gets the name of the district.
	/// </summary>
	/// <returns>The name of the district.</returns>
	public String getName()
	{
		return districtName;
	}

	/// <summary>
	/// Checks whether the point is within the bounds of the district.
	/// </summary>
	/// <returns><c>true</c>, if point is within district, <c>false</c> otherwise.</returns>
	/// <param name="point">Point to be checked.</param>
	public bool containsPoint(Vector2 point)
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
}