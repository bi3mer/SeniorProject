using UnityEngine;
using System.Collections;

public class City
{
    /// <summary>
    /// Creates a city.
    /// </summary>
    /// <param name="districts">The list of districts conatined in the city.</param>
    /// <param name="boundingBox">The bounding box that defines the size of the city.</param>
    public City (District[] districts, Bounds boundingBox)
    {
        Districts = districts;
        BoundingBox = boundingBox;
    }

    /// <summary>
    /// The list of districts contained in the city.
    /// </summary>
    public District[] Districts
    {
        get;
        private set;
    }

    /// <summary>
    /// The bounding box that defines the size of the city.
    /// </summary>
    public Bounds BoundingBox
    {
        get;
        private set;
    }

    // TODO: Implement city center
}
