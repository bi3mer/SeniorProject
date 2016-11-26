using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block
{
    /// <summary>
    /// Constructor for a city block
    /// </summary>
    /// <param name="controlPoint">Control point used to construct the district.</param>
    /// <param name="vertices">A list of four vertices to define the edges of the blocks.</param>
    public Block (Vector3 controlPoint, Vector3[] vertices)
    {
        Center = controlPoint;
        Vertices = vertices;
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
    /// The four vertices that define the edges of the blocks.
    /// </summary>
    public Vector3[] Vertices
    {
        get;
        private set;
    }

    /// <summary>
    /// The list of buildings contained in the block.
    /// </summary>
    public List<Building> Buildings
    {
        get;
        private set;
    }
}
