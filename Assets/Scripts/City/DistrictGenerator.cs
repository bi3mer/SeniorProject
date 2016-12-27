using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

public class DistrictGenerator : MonoBehaviour 
{
    [SerializeField]
    [Tooltip("Minimum distance between the seed points for poison distribution")]
    private float distanceBetweenSeedPoints;

    [SerializeField]
    [Tooltip("Array of district configurations to choose from.")]
    private DistrictConfiguration[] configurations;
    
    /// <summary>
    /// Generate the districts.
    /// 
    /// </summary>
    /// <param name="seed">The city generation seed.</param>
    /// <param name="cityBounds">The bounds defining the city's size.</param>
    /// <returns></returns>
    public District[] Generate (int seed, Bounds cityBounds)
    {
        // Check if we have at least one configuration
        if (configurations.Length < 1)
        {
            Debug.LogError("DistrcitGenerator requires at least one district configuration.");

            // Return an empty array
            return new District[0];
        }
        
        // Seed the generation
        Random.InitState(seed);

        // Distribute the seed points for the inputs to the voroni diagram
        List<Vector2> seedPoints = GenerationUtility.DistributeSeedPoints(seed, cityBounds, distanceBetweenSeedPoints);
     
        // Set up voroni diagram
        Rect bounds = new Rect(cityBounds.min.x, cityBounds.min.z, cityBounds.size.x, cityBounds.size.z);

        List<uint> colors = new List<uint>();
        for (int i = 0; i < seedPoints.Count; ++i) 
        {
            colors.Add(0);
        }

        Delaunay.Voronoi voroni = new Delaunay.Voronoi(seedPoints, colors, bounds);
        voroni.VoronoiDiagram ();
        voroni.SpanningTree(KruskalType.MINIMUM);
        voroni.DelaunayTriangulation ();

        // Create district from each set of vertecies
        List<District> districts = new List<District>();
        for (int i = 0; i < seedPoints.Count; ++i)
        {
            Vector2 seedPoint = seedPoints[i];
            
            // Get list of vertecies associated with each cell in the diagram
            List<Vector2> verts = new List<Vector2>();
            List<LineSegment> edges = voroni.VoronoiBoundaryForSite(seedPoint);
            for (int j = 0; j < edges.Count; ++j)
            {
                LineSegment edge = edges[j];
                GenerationUtility.AddNonDuplicateVertex(verts, (Vector2) edge.p0);
                GenerationUtility.AddNonDuplicateVertex(verts, (Vector2) edge.p1);
            }

            // Make sure there is more then 2 seed points so we can make a polygon
            if (verts.Count > 2)
            {   
                // Get the center of the points
                Vector2 center = GenerationUtility.GetMidpoint(verts);

                // Sort the vertecies in polygon order and add them to a list
                verts = GenerationUtility.SortVerticies(verts, center);

                List<Vector3> edgeVerticies = new List<Vector3>();
                for (int j = 0; j < verts.Count; ++j)
                {
                    edgeVerticies.Add (GenerationUtility.ToAlignedVector3 (verts[j]));
                }

                // Select a district configuration
                // Assigns configurations in order so we know we have an even distribution.
                DistrictConfiguration configuration = configurations[i % configurations.Length];

                // Construct the district
                districts.Add(new District
                    (
                        GenerationUtility.ToAlignedVector3(seedPoint), 
                        edgeVerticies.ToArray(),
                        configuration
                    ));
            }
        }

        // Return final list of districts 
        return districts.ToArray();
    }
}
