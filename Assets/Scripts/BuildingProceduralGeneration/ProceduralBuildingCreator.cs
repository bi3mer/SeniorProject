using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProceduralBuildingCreator : MonoBehaviour
{
    // All of the different districts, and all of their procedural building assets
    [SerializeField]
    private ProceduralDistrictDefinition[] district;

    //  Per building base Enums
    public enum HeightType
    {
        Stretch,
        // The below is not yet Implimented
        Stack
    };
    public enum BaseSize
    {
        OneByOne,
        // The below is not yet Implimented
        TwoByTwo,
        ThreeByThree
    }

    // Aditional colors building's albedo can take on to increase building variability.
    [SerializeField]
    private Gradient materialAlbedoColors;

    [SerializeField]
    private ProceduralBuildingInstance ProceduralBuilding;

    // Test Variables
    public int TestDistrict;
    public BaseSize TestBaseSize;
    public float TestAttatchmentPercentage;
    public int TestStoriesTall;
    public ProceduralBuildingInstance NewestBuilding;

    [ContextMenu("Create a test building")]
    public void TestBuildBuilding()
    {
       ProceduralBuildingInstance newbuilding = CreateBuilding(district[TestDistrict], TestBaseSize, TestAttatchmentPercentage, TestStoriesTall);
       //ShowBuilding(newbuilding);
       NewestBuilding = newbuilding;
    }
    [ContextMenu("Hide the newest building")]
    public void HideTestBuilding()
    {
   //     HideBuilding(NewestBuilding);
    }
    [ContextMenu("Show the newest building")]
    public void ShowTestBuilding()
    {
 //      ShowBuilding(NewestBuilding);
    }

    /// <summary>
    /// Populates a new proceduralbuilding class and spawns it in the world.
    /// </summary>
    public ProceduralBuildingInstance CreateBuilding(ProceduralDistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        ProceduralBuildingInstance newBuilding = (ProceduralBuildingInstance)Instantiate(ProceduralBuilding, Vector3.zero, Quaternion.Euler(Vector3.zero));
        newBuilding.IsVisible = true;

        // Set buildingbase
        ProceduralBuildingBase newBase = district.OneXOneDistrictBases[0];
        
        if (size == BaseSize.OneByOne && district.OneXOneDistrictBases.Length > 0)
        {
            newBase = district.OneXOneDistrictBases[Random.Range(0, district.OneXOneDistrictBases.Length -1)];
        }
        else if (size == BaseSize.TwoByTwo && district.TwoXTwoDistrictBases.Length > 0)
        {
            newBase = district.TwoXTwoDistrictBases[Random.Range(0, district.TwoXTwoDistrictBases.Length - 1)];
        }
        else if(size == BaseSize.ThreeByThree && district.ThreeXThreeDistrictBases.Length > 0)
        {
            newBase = district.ThreeXThreeDistrictBases[Random.Range(0, district.ThreeXThreeDistrictBases.Length - 1)];
        }
        else
        {
            Debug.LogError(district +" has no buildings of size " + size + ". Your building was not properly created!");
        }

        // Create the base
        GameObject newObject = (GameObject)Instantiate(newBase.gameObject, newBuilding.transform);
        newObject.transform.localScale = new Vector3(1f, storiesTall, 1f);
        newObject.transform.position = Vector3.zero;

        // Set the new base to the instance class.
        newBuilding.BuildingBase = newObject.GetComponent<ProceduralBuildingBase>();

        // Set the roof if need be set.
        if (newBuilding.BuildingBase.HasRoof == null)
        {
            ProceduralBuildingRoof newRoof = district.OneXOneDistrictRoofs[0];
            // Set buildingbase
            if (size == BaseSize.OneByOne && district.OneXOneDistrictRoofs.Length > 0)
            {
                newRoof = district.OneXOneDistrictRoofs[Random.Range(0, district.OneXOneDistrictRoofs.Length)];
            }
            else if (size == BaseSize.TwoByTwo && district.TwoXTwoDistrictRoofs.Length > 0)
            {
                newRoof = district.TwoXTwoDistrictRoofs[Random.Range(0, district.TwoXTwoDistrictRoofs.Length)];
            }
            else if (size == BaseSize.ThreeByThree && district.ThreeXThreeDistrictRoofs.Length > 0)
            {
                newRoof = district.ThreeXThreeDistrictRoofs[Random.Range(0, district.ThreeXThreeDistrictRoofs.Length)];
            }
            else
            {
                Debug.LogError(district + " has no buildings of size " + size + ". Your building was not properly created!");
            }

            // Create the roof
            newObject = (GameObject)Instantiate(newRoof.gameObject);
            newObject.transform.localScale = new Vector3(1f, 1f, 1f);
            newObject.transform.SetParent(newBuilding.BuildingBase.RoofLocation);
            newObject.transform.localPosition = Vector3.zero;
            newBuilding.BuildingRoof = newObject.GetComponent<ProceduralBuildingRoof>();

            // If the new roof can be rotated rotate it! But only at 90 degree intervals.
            if (newBuilding.BuildingRoof.CanRotate == true)
            {
                newBuilding.BuildingRoof.RoofRotation = Random.Range(0, 3) * 90;
                newObject.transform.Rotate(new Vector3(0f, newBuilding.BuildingRoof.RoofRotation, 0f));
            }
        }

        // Set Attatchments
        if(district.DistrictAttachments.Length > 0 && attatchmentPercentage > 0f && newBuilding.BuildingBase.AttatchmentPoints.Length > 0)
        {
            int numberOfAttachments = Mathf.RoundToInt(newBuilding.BuildingBase.AttatchmentPoints.Length * attatchmentPercentage/100f);

            newBuilding.BuildingAttachments = new ProceduralBuildingAttachment[numberOfAttachments];

            // Create a list of random numbers the length of the attatchment points.
            List<int> randomPoint = Enumerable.Range(0, newBuilding.BuildingBase.AttatchmentPoints.Length - 1).ToList<int>();


            // Shuffle the list
            for (int i = 0; i < randomPoint.Count; i++)
            {
                int temp = randomPoint[i];
                int randomIndex = Random.Range(i, randomPoint.Count);
                randomPoint[i] = randomPoint[randomIndex];
                randomPoint[randomIndex] = temp;
            }
            ProceduralBuildingAttachment newAttachment;
            for (int i = 0; i < numberOfAttachments; ++i)
            {   
                newAttachment = district.DistrictAttachments[Random.Range(0, district.DistrictAttachments.Length)];

                // Create the attatchment object
                newObject = (GameObject)Instantiate(newAttachment.gameObject);
                newAttachment = newObject.GetComponent<ProceduralBuildingAttachment>();
                newAttachment.AttachmentPoint = randomPoint[i];
                newObject.transform.localScale = new Vector3(1f, storiesTall, 1f); //NEEDS TO SUPPORT SEPERATE STORIES VALUES IN FUTURE
                newObject.transform.SetParent(newBuilding.BuildingBase.AttatchmentPoints[newAttachment.AttachmentPoint].transform);
                newObject.transform.localPosition = Vector3.zero;
                //Rotate the attatchment based off the modelling/prefab setup.
                newObject.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);

                newBuilding.BuildingAttachments[i] = newAttachment;
            }
                     
            // give the new attachments roofs if they need them, set the roof's scales properly if they don't.
            for (int i = 0; i <= newBuilding.BuildingAttachments.Length - 1; ++i)
            {
                ProceduralBuildingRoof newRoof = district.OneXOneDistrictRoofs[0];
                if (newBuilding.BuildingAttachments[i] != null && newBuilding.BuildingAttachments[i].HasRoof == null)
                {
                    newRoof = district.OneXOneDistrictRoofs[Random.Range(0, district.OneXOneDistrictRoofs.Length - 1)];
                    
                    // Create the roof
                    newObject = (GameObject)Instantiate(newRoof.gameObject);
                    newRoof = newObject.GetComponent<ProceduralBuildingRoof>();

                    newObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    newObject.transform.SetParent(newBuilding.BuildingAttachments[i].RoofLocation);
                    newObject.transform.localPosition = Vector3.zero;
                    newBuilding.BuildingAttachments[i].HasRoof = newRoof;

                    // If the new roof can be rotated rotate it! But only at 90 degree intervals.
                    if (newBuilding.BuildingAttachments[i].HasRoof.CanRotate == true)
                    {
                        newBuilding.BuildingAttachments[i].HasRoof.RoofRotation = Random.Range(0, 3) * 90;
                        newObject.transform.Rotate(new Vector3(0f, newBuilding.BuildingRoof.RoofRotation, 0f));
                    }
                    newBuilding.BuildingAttachments[i].HasRoof = newRoof;
                }
                else if(newBuilding.BuildingAttachments[i] != null)
                {
                    // Scale the roof the prefab came with properly.

                    newBuilding.BuildingAttachments[i].HasRoof.transform.SetParent(null);
                    newBuilding.BuildingAttachments[i].HasRoof.transform.localScale = new Vector3(1f, 1f, 1f);
                    newBuilding.BuildingAttachments[i].HasRoof.transform.SetParent(newBuilding.BuildingAttachments[i].RoofLocation);
                    newBuilding.BuildingAttachments[i].HasRoof.transform.localPosition = Vector3.zero;

                    // If the roof can be rotated rotate it! But only at 90 degree intervals.
                    if (newBuilding.BuildingAttachments[i].HasRoof.CanRotate == true)
                    {
                        newBuilding.BuildingAttachments[i].HasRoof.RoofRotation = Random.Range(0, 3) * 90;
                        newBuilding.BuildingAttachments[i].HasRoof.transform.Rotate(new Vector3(0f, newBuilding.BuildingAttachments[i].HasRoof.RoofRotation, 0f));
                    }
                }
            }
 
        }

        // Windows

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
            building.IsVisible = true;

            GameObject newObject;
            // Create the base
            newObject = (GameObject)Instantiate(building.BuildingBase.gameObject, building.transform);
            newObject.transform.localScale = new Vector3(1f, building.BuildingStories, 1f);
            newObject.transform.position = Vector3.zero;

            // Base roof
            if(building.BuildingBase.HasRoof != null)
            {
                building.BuildingBase.HasRoof.transform.SetParent(building.BuildingBase.RoofLocation);
                building.BuildingBase.HasRoof.transform.position = Vector3.zero;
                building.BuildingBase.HasRoof.transform.localScale = Vector3.one;
            }
            else
            {
                newObject = (GameObject)Instantiate(building.BuildingRoof.gameObject, building.BuildingBase.RoofLocation);
                newObject.transform.position = Vector3.zero;
                newObject.transform.localScale = Vector3.one;
            }

        }
        else
        {
            Debug.LogError("Building " + building + " was already visible");
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
            building.IsVisible = false;

         // List<GameObject> children = new List<GameObject>();
         //  int buildingChildCount = building.transform.childCount - 1;
            for (int i = building.transform.childCount; i > 0; --i)
            {
                Destroy(building.transform.GetChild(i));
            }
        }
        else
        {
            Debug.LogError("Building " + building + " was already hidden");
        }
    }
}


