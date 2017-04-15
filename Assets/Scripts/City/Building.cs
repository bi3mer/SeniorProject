using UnityEngine;
using System.Collections;

public class Building
{
    private Bounds bounds;
    private bool isVisible;
    private bool isCollidible;

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
    /// List of all colliders contained within the building.
    /// </summary>
    public Collider[] Colliders
    {
        get;
        private set;
    }

    /// <summary>
    /// The information needed to reconstrauct the building.
    /// </summary>
    public ProceduralBuildingInstance ProceduralInstance
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

    /// <summary>
    /// Gets the height of the building.
    /// </summary>
    public float Height
    {
        get;
        private set;
    }

    /// <summary>
    /// The building is shown.
    /// </summary>
    public bool IsVisible
    {
        get
        {
            return isVisible;
        }
        set
        {
            // Only update if the value changes
            if (isVisible != value)
            {
                this.Instance.SetActive(value);
            }
            isVisible = value;
        }
    }

    /// <summary>
    /// The building collider is active.
    /// </summary>
    public bool IsCollidible
    {
        get
        {
            return isCollidible;
        }
        set
        {
            // Only update if the value changes
            if (isCollidible != value)
            {
                for (int i = 0; i < this.Colliders.Length; ++i)
                {
                    this.Colliders[i].enabled = value;
                }
            }
            isCollidible = value;
        }
    }

    /// <summary>
    /// Creates a new building.
    /// </summary>
    /// <param name="position">The location of the root of the building.</param>
    /// <param name="buildingInstance">The instatiated game object.</param>
    /// <param name="proceduralInstance">The procedural building instance used to create this building.</param>
    /// <param name="height">The height of the building to instantiate.</param>
    public Building (Vector3 position, GameObject buildingInstance, ProceduralBuildingInstance proceduralInstance, float height)
    {
        RootPosition = position;
        Instance = buildingInstance;
        ProceduralInstance = proceduralInstance;
        Height = height;
        isCollidible = true;
        isVisible = true;

        Colliders = buildingInstance.GetComponentsInChildren<Collider>();
    }

    /// <summary>
    /// Creates a new video.
    /// </summary>
    /// <param name="position">The location of the root of the building.</param>
    /// <param name="buildingInstance">The instatiated game object.</param>
    public Building (Vector3 position, GameObject buildingInstance)
    {
        RootPosition = position;
        Instance = buildingInstance;
        ProceduralInstance = null;
        Height = 0;
        isCollidible = true;
        isVisible = true;

        Colliders = buildingInstance.GetComponentsInChildren<Collider>();
    }

    /// <summary>
    /// Calculates the bounds of the building.
    /// </summary>
    /// <returns></returns>
    private Bounds calculateBounds()
    {
        // TODO: Actually calculate bounds
        return new Bounds(Vector3.zero, new Vector3(3f, Height * 2f + 10f, 3f));
    }
}
