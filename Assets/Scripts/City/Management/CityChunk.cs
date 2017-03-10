using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages loading and unloading a chunk of buildings
/// </summary>
public class CityChunk
{
    /// <summary>
    /// Creates a instance of CityChunk
    /// </summary>
    /// <param name="bounds">Bounds defining chunk coverage.</param>
    public CityChunk (Bounds bounds)
    {
        BoundingBox = bounds;
        IsLoaded = false;
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

        for (int i = 0; i < Buildings.Count; ++i)
        {
            // Load building
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

        for (int i = 0; i < Buildings.Count; ++i)
        {
            // unload building
            yield return null;
        }

        // Unloading is finished
        CurrentCoroutine = null;
    }
}
