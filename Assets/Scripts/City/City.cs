﻿using UnityEngine;
using System.Collections;

public class City
{
    private Vector3 cityCenter;

    /// <summary>
    /// Creates a city.
    /// </summary>
    /// <param name="districts">The list of districts conatined in the city.</param>
    /// <param name="boundingBox">The bounding box that defines the size of the city.</param>
    /// <param name="cityCenter">The location of the city center.</param>
    public City (District[] districts, Bounds boundingBox, Vector3 cityCenter)
    {
        Districts = districts;
        BoundingBox = boundingBox;
        Center = cityCenter;
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

    /// <summary>
    /// The position of the center of the city.
    /// </summary>
    public Vector3 Center
    {
        get;
        private set;
    }
}
