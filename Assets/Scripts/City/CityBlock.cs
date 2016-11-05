using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityBlock
{
    /// <summary>
    /// Constructor for a city block
    /// </summary>
    /// <param name="vertices">A list of four vertices to define the edges of the blocks.</param>
    /// <param name="buildings">The list of buildings contained by the block.</param>
    public CityBlock (Vector3[] vertices)
    {
        Vertices = vertices;
        Buildings = new List<Building>(); ;
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

    /// <summary>
    /// Add a building to the block.
    /// </summary>
    /// <param name="building"></param>
    public void AddBuilding (Building building)
    {
        Buildings.Add(building);
    }
}
