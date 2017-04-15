using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages loading and unloading a chunk of buildings
/// </summary>
public class CityChunk
{
    /// <summary>
    /// Creates in instance of CityChunk.
    /// </summary>
    /// <param name="x">x-axis coordinate of grid.</param>
    /// <param name="y">z-axis coordinate of grid.</param>
    /// <param name="bounds">Area covered by chunk.</param>
    public CityChunk (int x, int y, Bounds bounds)
    {
        Location = new Tuple<int, int>(x, y);
        BoundingBox = bounds;
        IsLoaded = true;
        Buildings = new List<Building>();
    }

    /// <summary>
    /// Location of this chunk in the chunk grid.
    /// </summary>
    public Tuple<int, int> Location
    {
        get;
        private set;
    }

    /// <summary>
    /// Bounds defining chunk coverage
    /// </summary>
    public Bounds BoundingBox
    {
        get;
        private set;
    }
    
    /// <summary>
    /// List of buildings managed by this chunk.
    /// </summary>
    public List<Building> Buildings
    {
        get;
        private set;
    }

    /// <summary>
    /// True of the chunk has been set to be loaded.
    /// </summary>
    public bool IsLoaded
    {
        get;
        private set;
    }

    /// <summary>
    /// Current loading or unloading co-routine. Returns null if there is no current co-routine.
    /// </summary>
    public IEnumerator CurrentCoroutine
    {
        get;
        set;
    }

    /// <summary>
    /// Asynchronously loads buildings.
    /// </summary>
    public IEnumerator Load()
    {
        IsLoaded = true;

        SystemLogger.Write("Loading chunk (" + Location.X + ", " + Location.Y + ")");

        for (int i = 0; i < Buildings.Count; ++i)
        {
            Buildings[i].Load();
            yield return null;
        }

        // Loading is finished
        CurrentCoroutine = null;
    }

    /// <summary>
    /// Asynchronously unloads buildings.
    /// </summary>
    public IEnumerator Unload()
    {
        IsLoaded = false;

        SystemLogger.Write("Unloading chunk (" + Location.X + ", " + Location.Y + ")");

        for (int i = 0; i < Buildings.Count; ++i)
        {
            Buildings[i].Unload();
            yield return null;
        }

        // Unloading is finished
        CurrentCoroutine = null;
    }
}
