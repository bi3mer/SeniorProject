using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    /// The district configuration used to construct the building.
    /// </summary>
    public DistrictConfiguration Configuration
    {
        get;
        protected set;
    }

    /// <summary>
    /// Gets or sets the door and shelter attachment information.
    /// </summary>
    /// <value>The attachment information.</value>
    public List<ItemPlacementSamplePoint> AttachmentInformation
    {
    	get;
    	set;
    }

    /// <summary>
    /// Gets or sets the attachments. These include doors and shelters.
    /// </summary>
    /// <value>The attachments.</value>
    public List<GameObject> Attachments
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
    /// Generates an attachment.
    /// </summary>
    /// <returns>The attachment.</returns>
    /// <param name="attachmentTemplate">Attachment template.</param>
    /// <param name="location">Location.</param>
    /// <param name="size">Size.</param>
    /// <param name="district">District.</param>
    /// <param name="prespawnItems">If set to <c>true</c> prespawns items in the world without need for user to interact with spawner.</param>
    protected GameObject generateAttachment(GameObject attachmentTemplate, Vector3 location, float size, string district, bool prespawnItems)
    {
		GameObject attachment = GameObject.Instantiate(attachmentTemplate);

		attachment.SetActive(true);
		Transform attachmentTransform = attachment.transform;
		attachmentTransform.position = location;
		attachmentTransform.SetParent(Instance.transform, true);

		// a will only be rotated in 4 ways -- 0, 90, 180, and 270 degrees. A random number from 0 to 3 is generated and multiplied by 90  degrees
		attachmentTransform.rotation = Quaternion.Euler(attachmentTransform.eulerAngles.x, Random.Range(0, 4) * 90, attachmentTransform.eulerAngles.z);

		// since spawning of items may occur immediately, make sure that door is positioned properly before spawner set up is called
		ItemSpawner spawner = attachment.GetComponent<ItemSpawner>();

        
		spawner.SetUpSpawner(size, district);
		spawner.SpawnWithoutInteraction = prespawnItems;

        if(spawner.PosterPositions.Length != 0)
        { 
            GameObject poster = GameObject.Instantiate(Configuration.DistrictPosters[Random.Range(0, Configuration.DistrictPosters.Length)]);
            int posterPos = Random.Range(0, spawner.PosterPositions.Length);
            poster.transform.position = spawner.PosterPositions[posterPos].position;
            poster.transform.SetParent(spawner.gameObject.transform);
            poster.transform.rotation = spawner.PosterPositions[posterPos].rotation;
            poster.transform.eulerAngles += new Vector3(0f, 0f, Random.Range(-spawner.PosterRotationModMax, spawner.PosterRotationModMax));
        }
        return attachment;
    }

    /// <summary>
    /// Loads the doors and shelters onto the roof.
    /// </summary>
    public void LoadAttachments()
    {
		bool prespawn;

		// prespawn means that the shelter will have items spawned around it without the player needing to interact with the shelter
		for(int i = 0; i < Attachments.Count; ++i)
        {
			prespawn = (AttachmentInformation[i].Type == ItemPlacementSamplePoint.PointType.SHELTER);
        	generateAttachment(Attachments[i], AttachmentInformation[i].WorldSpaceLocation, AttachmentInformation[i].Size, 
        	                   AttachmentInformation[i].District, prespawn);
        }
    }
}
