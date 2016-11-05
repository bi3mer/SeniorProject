using UnityEngine;
using System.Collections;

public class Building 
{
    public Building (Bounds dimensions, Vector3 position)
    {
        Dimensions = dimensions;
        Position = position;
    }
    
    /// <summary>
    /// The dimensions of the building (for now just a cube)
    /// </summary>
    public Bounds Dimensions
    {
        get;
        private set;
    }

    /// <summary>
    /// The position of the building in the world.
    /// </summary>
    public Vector3 Position
    {
        get;
        private set;
    }

    // TODO: Houdini model parameters,
    // TODO: Handle features like doors and climable objects
    // TODO: Populate conents of building if there is a door
}
