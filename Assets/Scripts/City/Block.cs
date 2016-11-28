using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block
{
    private Bounds bounds;

    /// <summary>
    /// Constructor for a city block
    /// </summary>
    /// <param name="controlPoint">Control point used to construct the district.</param>
    /// <param name="vertices">A list of four vertices to define the edges of the blocks.</param>
    public Block (Vector3 controlPoint, Vector3[] vertices)
    {
        Center = controlPoint;
        Verticies = vertices;
        Buildings = new List<Building>();
    }

    /// <summary>
    /// Center of the district.
    /// </summary>
    public Vector3 Center
    {
        get;
        private set;
    }

    /// <summary>
    /// The vertices that define the edges of the blocks.
    /// </summary>
    public Vector3[] Verticies
    {
        get;
        private set;
    }

    /// <summary>
    /// The list of buildings in this block.
    /// </summary>
    public List<Building> Buildings
    {
        get;
        private set;
    }

    /// <summary>
    /// The bounds that conatin all of the block verticies.
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
    /// Checks whether the point is within the bounds of the block.
    /// </summary>
    /// <returns><c>true</c>, if point is within block, <c>false</c> otherwise.</returns>
    /// <param name="point">Point to be checked.</param>
    public bool ContainsPoint(Vector2 point)
    {
        Vector2[] edges = new Vector2[Verticies.Length];
        for (int i = 0; i < Verticies.Length; ++i)
        {
            edges[i] = GenerationUtility.ToAlignedVector2(Verticies[i]);
        }
        return GenerationUtility.IsPointInPolygon(point, edges);
    }

    /// <summary>
    /// Calculate the bound that encapsulates all the vertecies.
    /// </summary>
    /// <returns>Bounds conatining the ede vertecies.</returns>
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
