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

    public ItemPlacementSamplePoint[] AttachmentInformation
    {
    	get;
    	set;
    }

    public GameObject[] Attachments
    {
    	get;
    	set;
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
    private Bounds calculateBounds()
    {
        Bounds bounds = new Bounds(Position, Vector3.zero);

        if (Instance == null)
        {
            return bounds;
        }

        try
        {
            Collider collider = Instance.transform.GetComponent<Collider>();
            if (collider != null)
            {
                bounds.Encapsulate(collider.bounds);
            }
        }
        catch { }

        Transform[] allChildren = Instance.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; ++i)
        {
            try
            {
                bounds.Encapsulate(allChildren[i].GetComponent<Collider>().bounds);
            }
            catch { }
        }

        return bounds;
    }

    /// <summary>
    /// Loads the instance of the building into the scene.
    /// </summary>
    public abstract void Load();

    /// <summary>
    /// Unloads the instance of the building from the scene.
    /// </summary>
    public void Unload()
    {
        if (!IsLoaded)
        {
            return;
        }

        if (Instance != null)
        {
            GameObject.Destroy(Instance);
        }

        IsLoaded = false;
    }

    /// <summary>
    /// Generates ab attachment.
    /// </summary>
    /// <returns>The attachment.</returns>
    /// <param name="attachmentTemplate">Attachment template.</param>
    /// <param name="location">Location.</param>
    /// <param name="attachedBuilding">Attached building.</param>
    /// <param name="size">Size.</param>
    /// <param name="district">District.</param>
    /// <param name="prespawnItems">If set to <c>true</c> prespawn items.</param>
    protected GameObject generateAttachment(GameObject attachmentTemplate, Vector3 location, GameObject attachedBuilding, float size, string district, bool prespawnItems)
    {
		GameObject attachment = GameObject.Instantiate(attachmentTemplate);

		attachment.SetActive(true);
		Transform attachmentTransform = attachment.transform;
		attachmentTransform.position = location;
		attachmentTransform.SetParent(attachedBuilding.transform, true);

		// a will only be rotated in 4 ways -- 0, 90, 180, and 270 degrees. A random number from 0 to 3 is generated and multiplied by 90  degrees
		attachmentTransform.rotation = Quaternion.Euler(attachmentTransform.eulerAngles.x, Random.Range(0, 4) * 90, attachmentTransform.eulerAngles.z);

		// since spawning of items may occur immediately, make sure that door is positioned properly before spawner set up is called
		ItemSpawner spawner = attachment.GetComponent<ItemSpawner>();

		spawner.SetUpSpawner(size, district);
		spawner.SpawnWithoutInteraction = prespawnItems;

		return attachment;
    }
}
