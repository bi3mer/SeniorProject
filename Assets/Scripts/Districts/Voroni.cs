using System;
using UnityEngine;
using System.Collections;
	
public class Voroni
{
	private Vector2[] initialSeedPoints;
	private int maxSpan;
	private int minSpan;
	private Vector2 center;

	/// <summary>
	/// Initializes a new instance of the <see cref="Voroni"/> class.
	/// </summary>
	/// <param name="seeds">Seeds.</param>
	/// <param name="max">Max.</param>
	/// <param name="min">Minimum.</param>
	public Voroni (Vector2[] seeds, int max, int min)
	{
		initialSeedPoints = seeds;
		maxSpan = max;
		minSpan = min;
	}

	/// <summary>
	/// Gets or sets the center of the Voroni diagram.
	/// </summary>
	/// <value>The center.</value>
	public Vector2 Center
	{ 
		get
		{ 
			return center; 
		} 
		set 
		{
			center = value;
		}
	}

	/// <summary>
	/// Gets the edges based on the initial seeds and max/min span for this instance.
	/// </summary>
	/// <returns>The edges.</returns>
	public Vector2[] GetEdges()
	{
		int numSeeds = initialSeedPoints.Length;
		Vector2[] edges = new Vector2[numSeeds];
		Vector2[] seedMidPoints = new Vector2[numSeeds];

		// calculate midpoints of all initial seeds
		for (int i = 0; i < numSeeds; ++i)
		{
			float x = 0;
			float y = 0;

			if (i == numSeeds - 1) 
			{
				x = (initialSeedPoints [i].x + initialSeedPoints [0].x) / 2;
				y = (initialSeedPoints [i].y + initialSeedPoints [0].y) / 2;
			} 
			else
			{
				x = (initialSeedPoints [i].x + initialSeedPoints [i+1].x) / 2;
				y = (initialSeedPoints [i].y + initialSeedPoints [i+1].y) / 2;
			}

			seedMidPoints [i].x = x;
			seedMidPoints [i].y = y;
		}

		// using these midpoints, get the center of the city
		float cityCenterX = 0;
		float cityCenterY = 0;

		for (int i = 0; i < numSeeds; ++i)
		{
			cityCenterX += seedMidPoints[i].x;
			cityCenterY += seedMidPoints[i].y;
		}

		center.x = cityCenterX/numSeeds;
		center.y = cityCenterY/numSeeds;

		float[] slopes = new float[initialSeedPoints.Length];

		// for each point, get the slope, its length based districtSpan, and its position relative to city center
		for (int i = 0; i < numSeeds; ++i)
		{
			float slope = (center.y-seedMidPoints[i].y)/(center.x-seedMidPoints[i].x);
			slopes [i] = slope;
			float k = maxSpan/(Mathf.Sqrt(1+Mathf.Pow(slope, 2.0f)));

			// account for position of point relative in space when assigning end point
			float currentEndpointX = 0.0f;
			float currentEndpointY = 0.0f;

			if (seedMidPoints [i].x < cityCenterX) {
				currentEndpointX = center.x - k;
			} 
			else 
			{
				currentEndpointX = center.x + k;
			}

			if (seedMidPoints [i].y < cityCenterY) {
				currentEndpointY = center.y - (k * slope);
			} 
			else 
			{
				currentEndpointY = center.y + (k * slope);
			}

			edges [i].x = currentEndpointX;
			edges [i].y = currentEndpointY;

		}

		return edges;
	}
}

