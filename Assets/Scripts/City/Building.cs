using UnityEngine;
using System.Collections;

public class Building
{
    //private ProceduralBuildingInstance buildingInstance;
    private Bounds bounds;

    /// <summary>
    /// Position of the root of the building.
    /// </summary>
    public Vector3 RootPosition
    {
        get;
        private set;
    }

    /// <summary>
    /// The gameObject representing the building in the scene.
    /// </summary>
    public GameObject Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// The bounds defining the size of the builing
    /// </summary>
    public Bounds BoundingBox
    {
        get
        {
            if (bounds.size == Vector3.zero)
            {
                // only calculate bounds once
                bounds = calculateBounds();
            }

            return bounds;
        }
    }

    public float Height
    {
        get;
        private set;
    }

    public Building (Vector3 position, GameObject buildingInstance, float height)
    {
        RootPosition = position;
        Instance = buildingInstance;
        Height = height;
    }

    private Bounds calculateBounds()
    {
        // TODO: Actually calculate bounds
        return new Bounds(Vector3.zero, new Vector3(5f, Height * 2f, 5f));
    }
}
