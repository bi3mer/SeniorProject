using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Delaunay;
using Delaunay.Geo;

public class BlockGenerator : MonoBehaviour
{
    // TODO: Make this value specific to district type?
    // TODO: Split this into North/South distance East/West distance if we want rectangular blocks
    [SerializeField]
    [Tooltip("Distance between the control points for each block.")]
    private float distanceBetweenControlPoints;

    [SerializeField]
    [Tooltip("Amount to jitter the spacing of the control points. A value of 0 is regularly spaced.")]
    [Range(0, 1)]
    private float jitter;

    /// <summary>
    /// Generate the blocks within the specified ditrict based on the values specified in the block generator.
    /// </summary>
    /// <param name="seed">The generation seed.</param>
    /// <param name="district">The district to generate blocks within.</param>
    /// <returns>An array of the generated blocks.</returns>
    public Block[] Generate (int seed, District district)
    {
        // Seed random
        Random.InitState(seed);

        List<Block> blocks = new List<Block>();
        List<Vector2> controlPoints = new List<Vector2>();
        Vector3 min = district.BoundingBox.min;
        Vector3 max = district.BoundingBox.max;
        Vector3 size = district.BoundingBox.size;

        // create a grid of points inside the bounds along the y-axis
        for (int i = Mathf.FloorToInt(min.x); i < max.x; i += Mathf.FloorToInt(distanceBetweenControlPoints))
        {
            for (int j = Mathf.FloorToInt(min.z); j < max.z; j += Mathf.FloorToInt(distanceBetweenControlPoints))
            {
                // Offset this value by the jitter amount
                float xOffset = Random.Range(0, jitter) * distanceBetweenControlPoints;
                float yOffset = Random.Range(0, jitter) * distanceBetweenControlPoints;

                Vector2 point = new Vector2 (i + xOffset, j + yOffset);

                // Check if control point is within the actual polygon
                if (district.ContainsPoint(point))
                {
                    controlPoints.Add(point);
                }
            }
        }

        // Set up voroni diagram
        Rect bounds = new Rect (min.x, min.z, size.x, size.z);

        List<uint> colors = new List<uint>();
        for (int i = 0; i < controlPoints.Count; ++i)
        {
            colors.Add(0);
        }

        Delaunay.Voronoi voroni = new Delaunay.Voronoi(controlPoints, colors, bounds);
        voroni.VoronoiDiagram();
        voroni.SpanningTree(KruskalType.MINIMUM);
        voroni.DelaunayTriangulation();

        // Create block from each set of verticies
        for (int i = 0; i < controlPoints.Count; ++i)
        {
            Vector2 controlPoint = controlPoints[i];

            // Get list of vertecies associated with each cell in the diagram
            List<Vector2> verts = new List<Vector2>();
            List<LineSegment> edges = voroni.VoronoiBoundaryForSite(controlPoint);
            for (int j = 0; j < edges.Count; ++j)
            {
                LineSegment edge = edges[j];
                Vector2 a = (Vector2) edge.p0;
                Vector2 b = (Vector2) edge.p1;

                if (district.ContainsPoint(a))
                {
                    GenerationUtility.AddNonDuplicateVertex(verts, a);
                }

                if (district.ContainsPoint(b))
                {
                    GenerationUtility.AddNonDuplicateVertex(verts, b);
                }
            }
            
            // Make sure there is more then 2 seed points so we can make a polygon
            if (verts.Count > 2)
            {
                // Sort the verticies
                GenerationUtility.SortVerticies(verts, controlPoint);

                List<Vector3> edgeVerticies = new List<Vector3>();
                for (int j = 0; j < verts.Count; ++j)
                {
                    edgeVerticies.Add(GenerationUtility.ToAlignedVector3(verts[j]));
                }

                // TODO: Determine other block features, if any
                // TODO: Save the streets?

                // Construct the Block
                blocks.Add(new Block
                    (
                        GenerationUtility.ToAlignedVector3(controlPoint),
                        edgeVerticies.ToArray()
                    ));
            }
        }

        // Return final list of blocks
        return blocks.ToArray();
    }
}
