using UnityEngine;
using System.Collections;


public class DistrictsGenerator : MonoBehaviour 
{
	[SerializeField]
	private Vector2[] districtEndPoints;

	[SerializeField]
	private Vector2 cityCenter;

	[SerializeField]
	private District[] districts;

	[SerializeField]
	private DebugDistricts debugger;

	public int NumInitialSeeds = 3;
	public int MaxDistrictSpan = 500;
	public int MinVerts = 10;
	public int MaxVerts = 30;
	public int MinDistFromCenter = 300;
	public int MaxDistFromCenter = 800;

	/// <summary>
	/// Entry point of the generator.
	/// </summary>
	void Start () 
	{
		debugger = GetComponent<DebugDistricts> ();
		generateDistrictPoints (NumInitialSeeds, MaxDistrictSpan);
	}

	/// <summary>
	/// Generates city center and district edges. Edges of districts are defined as (cityCenter)->(districtEndPoints[i])
	/// </summary>
	/// <param name="numDistricts">Number of districts desired -- currently supports 3.</param>
	/// <param name="districtMaxSpan">District max span for one side from center point to edge.</param>
	private void generateDistrictPoints(int numDistricts, int districtMaxSpan)
	{
		Vector2[] initialSeedPoints = new Vector2[numDistricts];
		Vector2[] seedMidPoints		= new Vector2[numDistricts];

		districtEndPoints = new Vector2[numDistricts];
		cityCenter 	    = new Vector2();
		districts 		= new District[numDistricts];

		// TODO: use one seed for all procedural generation
		// TODO: abstract out voroni implementation into its own

		// generate initial random seed points -- range is districtMaxSpan to districtMaxSpan * 2 to prevent negatives.
		for (int i = 0; i < numDistricts; ++i) 
		{
			initialSeedPoints[i].x = Random.Range(districtMaxSpan*i, districtMaxSpan*(i+1));
			initialSeedPoints[i].y = Random.Range(districtMaxSpan*i, districtMaxSpan*(i+1));

			// this code ensures that neither the x nor the y values of this random point
			// are too close to another existing random point. it will re-randomize if so.
			if (initialSeedPoints.Length > 0)
			{
				for (int j = 0; j < initialSeedPoints.Length; ++j)
				{
					if (initialSeedPoints[i].x != initialSeedPoints[j].x)
					{
						if (Mathf.Abs(initialSeedPoints[i].x - initialSeedPoints[j].x) < districtMaxSpan/2 || Mathf.Abs(initialSeedPoints[i].y - initialSeedPoints[j].y) < districtMaxSpan/2)
						{
							while(Mathf.Abs(initialSeedPoints[i].x - initialSeedPoints[j].x) < districtMaxSpan/2 || Mathf.Abs(initialSeedPoints[i].y - initialSeedPoints[j].y) < districtMaxSpan/2)
							{
								initialSeedPoints[i].x = Random.Range(districtMaxSpan, districtMaxSpan*2);
								initialSeedPoints[i].y = Random.Range(districtMaxSpan, districtMaxSpan*2);
							}
						}
					}
				}
			}
		}

		// calculate midpoints of all initial seeds
		for (int i = 0; i < numDistricts; ++i)
		{
			float x = 0;
			float y = 0;

			if (i == numDistricts - 1) 
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

		for (int i = 0; i < numDistricts; ++i)
		{
			cityCenterX += seedMidPoints[i].x;
			cityCenterY += seedMidPoints[i].y;
		}

		cityCenter.x = cityCenterX/numDistricts;
		cityCenter.y = cityCenterY/numDistricts;

		float[] slopes = new float[NumInitialSeeds];

		// for each point, get the slope, its length based districtSpan, and its position relative to city center
		for (int i = 0; i < numDistricts; ++i)
		{
			float slope = (cityCenter.y-seedMidPoints[i].y)/(cityCenter.x-seedMidPoints[i].x);
			slopes [i] = slope;
			float k = districtMaxSpan/(Mathf.Sqrt(1+Mathf.Pow(slope, 2.0f)));

			// account for position of point relative in space when assigning end point
			float currentEndpointX = 0.0f;
			float currentEndpointY = 0.0f;

			if (seedMidPoints [i].x < cityCenterX) {
				currentEndpointX = cityCenter.x - k;
			} 
			else 
			{
				currentEndpointX = cityCenter.x + k;
			}

			if (seedMidPoints [i].y < cityCenterY) {
				currentEndpointY = cityCenter.y - (k * slope);
			} 
			else 
			{
				currentEndpointY = cityCenter.y + (k * slope);
			}
			districtEndPoints [i].x = currentEndpointX;
			districtEndPoints [i].y = currentEndpointY;

		}

		// assign each district its first two verticies based on the endpoints of its edges
		for (int i = 0; i < numDistricts; ++i) 
		{
			districts [i] = new District (cityCenter);

			Vector2[] districtPoints = new Vector2[2];

			// if it's the last district, its edges are that of the first and the last districts
			if (i == numDistricts-1)
			{
				districtPoints[0] = districtEndPoints[districtEndPoints.Length-1];
				districtPoints[1] = districtEndPoints[0];
			} 
			else 
			{
				districtPoints[0] = districtEndPoints[i];
				districtPoints[1] = districtEndPoints[i+1];
			}

			districts [i].Verticies = districtPoints;
		}

		// generate verticies for each district to allow for more natural edges
		generateCityEdges (MinVerts, MaxVerts, MaxDistFromCenter, MinDistFromCenter);
	}

	/// <summary>
	/// Generates the city edge vertices.
	/// </summary>
	/// <param name="minVerts">Minimum # of vertices for edges of city.</param>
	/// <param name="maxVerts">Maximum # of vertices for edges of city</param>
	/// <param name="maxDistFromCenter">Maximum distance of all verticies from center of city.</param>
	/// <param name="minDistFromCenter">Minimum distance of all verticies from center of city.</param>
	private void generateCityEdges(int minVerts, int maxVerts, float maxDistFromCenter, float minDistFromCenter)
	{
		// for each district, generate random # of randomly angled points within the current area of the district
		for (int i = 0; i < districts.Length; ++i)
		{
			int numVerts = Random.Range (minVerts, maxVerts);
			Vector2[] newDistrictVerts = new Vector2 [numVerts+2];	// must allow for two initial points
			Vector2[] currentDistrictVerts = districts [i].Verticies;

			// add initial district edges to list of verticies
			newDistrictVerts [0] = currentDistrictVerts [0];
			newDistrictVerts [1] = currentDistrictVerts [1]; 

			for (int j = 0; j < numVerts; ++j)
			{
				// determine distance of point from center of city using perlin noise
				float percentageLength = Mathf.PerlinNoise (cityCenter.x, cityCenter.y);
				float distanceFromCenter = percentageLength * maxDistFromCenter;

				// compute a random point within the district to use as reference for angle from center
				float newX = Random.Range (currentDistrictVerts [0].x, currentDistrictVerts [1].x);
				float newY = Random.Range (currentDistrictVerts [0].y, currentDistrictVerts [1].y);
				Vector2 newRayPoint = new Vector2 (newX, newY);

				// new district vert is endpoint of line extending from city center through new random point at perlin length
				float angleFromCenter = Vector2.Angle (cityCenter, newRayPoint);
				newDistrictVerts [j].x = cityCenter.x + (distanceFromCenter) * Mathf.Cos (angleFromCenter);
				newDistrictVerts [j].y = cityCenter.y + (distanceFromCenter) * Mathf.Sin (angleFromCenter);
			}

			districts [i].Verticies = newDistrictVerts;
		}

		debugger.Verts = districts [0].Verticies;
	}

}
