using UnityEngine;
using System.Collections;

public abstract class Building
{
    private Bounds bounds;

    /// <summary>
    /// Returns true if the building is loaded in the world.
    /// </summary>
    public bool IsLoaded
    {
        get;
        protected set;
    }

    /// <summary>
    /// Position of the root of the building.
    /// </summary>
    public Vector3 Position
    {
        get;
        protected set;
    }

    /// <summary>
    /// The gameObject representing the building in the scene.
    /// </summary>
    public GameObject Instance
    {
        get;
        protected set;
    }

    /// <summary>
    /// The parent we are parenting the buildings to.
    /// </summary>
    public Transform Parent
    {
        get;
        protected set;
    }

    /// <summary>
    /// The bounds defining the size of the builing
    /// </summary>
    public Bounds BoundingBox
    {
        get
        {
            if (bounds == null || bounds.size == Vector3.zero)
            {
                // only calculate bounds once
                bounds = calculateBounds();
            }

            return bounds;
        }
    }

    /// <summary>
    /// Calculates the bounds of the building.
    /// </summary>
    /// <returns></returns>
    protected abstract Bounds calculateBounds();

    /// <summary>
    /// Loads the instance of the building into the scene.
    /// </summary>
    public abstract void Load();

    /// <summary>
    /// Unloads the instance of the building from the scene.
    /// </summary>
    public abstract void Unload();
}
