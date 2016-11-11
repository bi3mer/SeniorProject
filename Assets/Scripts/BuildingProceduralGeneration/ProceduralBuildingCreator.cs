using UnityEngine;
using System.Collections;

public class ProceduralBuildingCreator : MonoBehaviour
{
    // All of the different districts, and all of their procedural building assets
    [SerializeField]
    private ProceduralDistrictDefinition[] district;

    //  Per building base Enums
    public enum HeightType
    {
        Stretch,
        // Not yet Implimented
        Stack
    };
    public enum BaseSize
    {
        OneByOne,
        // More Sizes to come as they are modeled.
    }

    [SerializeField]
    private ProceduralBuildingInstance ProceduralBuilding;

    // Test Variables
    public int TestDistrict;
    public BaseSize TestBaseSize;
    public float TestAttatchmentPercentage;
    public int TestStoriesTall;

    [ContextMenu("Create a test building")]
    public void TestBuildBuilding()
    {
        CreateBuilding(district[TestDistrict], TestBaseSize, TestAttatchmentPercentage, TestStoriesTall);
    }

    /// <summary>
    /// Creates a new building.
    /// </summary>
    public ProceduralBuildingInstance CreateBuilding(ProceduralDistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        ProceduralBuildingInstance newBuilding = (ProceduralBuildingInstance)Instantiate(ProceduralBuilding, Vector3.zero, Quaternion.Euler(Vector3.zero));



        return newBuilding;
    }

    /// <summary>
    /// Spawns all the aspects of a procedural building that's already been created.
    /// </summary>
    /// <param name="building"></param>
    public void ShowBuilding(ProceduralBuildingInstance building)
    {
        if(building.IsVisible == false)
        {


        }
    }    

    /// <summary>
    /// Destroys all aspects of a procedural building that's already been created.
    /// </summary>
    /// <param name="building"></param>
    public void HideBuilding(ProceduralBuildingInstance building)
    {
        if(building.IsVisible == true)
        {

        }
    }
}


