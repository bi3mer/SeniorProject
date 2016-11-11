using UnityEngine;
using System.Collections;

/// <summary>
/// This class holds all the info needed to make a procedural building. 
/// It's attached to an empty gameobject. The building gets placed under the gameobject and can be destroyed and recreated as needed.
/// </summary>
public class ProceduralBuildingInstance : MonoBehaviour
{
    public bool IsVisible;

    private int buildingStories;
    public int BuildingStories
    {
        get
        {
            return buildingStories;
        }
        set
        {
            if (buildingStories == 0)
            {
                buildingStories = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }

    private ProceduralBuildingBase buildingBase;
    public ProceduralBuildingBase BuildingBase
    {
        get
        {
            return buildingBase;
        }
        set
        {
            if(buildingBase == null)
            {
                buildingBase = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }

    private ProceduralBuildingWindow windows;
    public ProceduralBuildingWindow Windows
    {
        get
        {
            return windows;
        }
        set
        {
            if (windows == null)
            {
                windows = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }

    private float windowSpacing;
    public float WindowSpacing
    {
        get
        {
            return windowSpacing;
        }
        set
        {
            if(windowSpacing == 0f)
            {
                windowSpacing = value;
            }
        }
    }

    private ProceduralBuildingRoof buildingRoof;
    public ProceduralBuildingRoof BuildingRoof
    {
        get
        {
            return buildingRoof;
        }
        set
        {
            if (buildingRoof == null)
            {
                buildingRoof = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }

    private ProceduralBuildingAttachment[] buildingAttatchments;
    public ProceduralBuildingAttachment[] BuildingAttatchments
    {
        get
        {
            return buildingAttatchments;
        }
        set
        {
            if (BuildingAttatchments == null)
            {
               buildingAttatchments = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }

    private Material buildingMaterial;
    public Material BuildingMaterial
    {
        get
        {
            return buildingMaterial;
        }
        set
        {
            if (buildingMaterial == null)
            {
                buildingMaterial = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }
}
