using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Defines all the buildings for each district and allows for the creation of builings in those districts.
/// </summary>
public class ProceduralBuildingCreator : MonoBehaviour
{
    // All of the different districts, and all of their procedural building assets
    [SerializeField]
    private DistrictDefinition[] districts;

    // Per building base Enums
    public enum HeightType
    {
        Stretch,
        Stack
    };

    public enum BaseSize
    {
        OneByOne,
        TwoByTwo,
        ThreeByThree
    };
    
    public enum BuildingParts
    {
        Base,
        Roof
    };

    public struct buildingIndex
    {
       public int BuildingSize;
       public int PartNumber;
    };

    // Aditional colors building's albedo can take on to increase building variability.
    [SerializeField]
    private Gradient materialAlbedoColors;

    [SerializeField]
    private ProceduralBuildingInstance proceduralBuilding;

    // The number of material instances to make per material.
    [SerializeField]
    private int materialInstances = 1;

    // Height of a single story
    private const float storyHeightUnits = .75f;
    // Length of 1 base building unit on the grid.
    private const float OneBaseLengthUnits = 3f;
    // Half of above.
    private const float HalfBaseLengthUnits = OneBaseLengthUnits / 2f;
    // Start rotation modifier of all attachments
    private const float attachmentRotationMod = 0f;
    // Rotation mod of all windows.
    private const float attachmentWindowRotationMod = 90f;

    // The string name to access the standard shader's smoothness value
    private const string StandardShaderSmoothness = "_GlossMapScale";

    // Test Variables.
    public int TestDistrict;
    public BaseSize TestBaseSize;
    public float TestAttatchmentPercentage;
    public int TestStoriesTall;
    public string TestDistrictName;
    public ProceduralBuildingInstance NewestBuilding;

    /// <summary>
    /// Function used to test making buildings using the above test variables.
    /// </summary>
    [ContextMenu("Create a test building")]
    public void TestBuildBuilding()
    {
       ProceduralBuildingInstance newbuilding = CreateBuilding(TestDistrictName, TestBaseSize, TestAttatchmentPercentage, TestStoriesTall);
       NewestBuilding = newbuilding;
    }


    /// <summary>
    /// Creates materials that all buildings in a district will use.
    /// </summary>
    [ContextMenu("CreateNewMaterials")]
    public void CreateMaterialsForAllDistricts()
    {
        for (int i = 0; i < districts.Length; ++i)
        {
            CreateMaterialsForDistrict(districts[i]);
        }
    }

    /// <summary>
    /// Creates all the procedural materials for a single district.
    /// </summary>
    /// <param name="district"></param>
    void CreateMaterialsForDistrict(DistrictDefinition district)
    {
        district.districtProceduralMaterials.Clear();
        district.districtProceduralWindowMaterials.Clear();

        for (int i = 0; i < district.DistrictMaterials.Length; ++i)
        {
            for (int j = 0; j < materialInstances; ++j)
            {
                Material newMaterial = new Material(district.DistrictMaterials[i].shader);
                newMaterial.CopyPropertiesFromMaterial(district.DistrictMaterials[i]);
                // Setting all the material's smoothess to 5 to reduce glossiness.
                newMaterial.SetFloat(StandardShaderSmoothness, .5f);
                district.districtProceduralMaterials.Add(newMaterial);
            }
        }

        for (int i = 0; i < district.DistrictWindowMaterials.Length; ++i)
        {
            for (int j = 0; j < materialInstances; ++j)
            {
                Material newMaterial = new Material(district.DistrictWindowMaterials[i].shader);
                newMaterial.CopyPropertiesFromMaterial(district.DistrictWindowMaterials[i]);
                newMaterial.color = materialAlbedoColors.Evaluate(Random.Range(0f, 1f));
                district.districtProceduralWindowMaterials.Add(newMaterial);
            }
        }
    }

    /// <summary>
    /// Creates a building in a district by its name.
    /// </summary>
    /// <returns></returns>
    public ProceduralBuildingInstance CreateBuilding(string districtName, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        for (int i = 0; i < districts.Length; ++i)
        {
            if(districts[i].DistrictName == districtName)
            {
                return CreateBuilding(districts[i], size, attatchmentPercentage, storiesTall);
            }
        }

        Debug.LogError("No district with name: " + districtName + " defaulting to district 1");
        return CreateBuilding(districts[1], size, attatchmentPercentage, storiesTall);
    }

    /// <summary>
    /// Creates a building in a district by its district number.
    /// </summary>
    public ProceduralBuildingInstance CreateBuilding(int districtNumber, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        if(districtNumber  <= districts.Length)
        {
            return CreateBuilding(districts[districtNumber], size, attatchmentPercentage, storiesTall);
        }

        Debug.LogError("No district with number "+ districtNumber +" defaulting to the first district");
        return CreateBuilding(districts[0], size, attatchmentPercentage, storiesTall);
    }


    /// <summary>
    /// Populates a new proceduralbuilding class and spawns it in the world.
    /// </summary>
    public ProceduralBuildingInstance CreateBuilding(DistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        // Check to see if we need new materials.
        if (district.districtProceduralMaterials.Count == 0 || district.districtProceduralWindowMaterials.Count == 0)
        {
            CreateMaterialsForDistrict(district);
        }
        
        ProceduralBuildingInstance newBuilding = (ProceduralBuildingInstance)Instantiate(proceduralBuilding, Vector3.zero, Quaternion.Euler(Vector3.zero));
        newBuilding.IsVisible = true;

        // Choose a material for all the building bits
        Material buildingMaterial = (Material) district.districtProceduralMaterials[Random.Range(0, district.districtProceduralMaterials.Count)];

        // Choose a window material to use for all the materials
        Material windowMaterial = (Material) district.districtProceduralWindowMaterials[Random.Range(0, district.districtProceduralWindowMaterials.Count)];

        List<MeshRenderer> buildingMeshes = new List<MeshRenderer>();
        List<MeshRenderer> windowMeshes = new List<MeshRenderer>();

        createBase(newBuilding, district, size, attatchmentPercentage, storiesTall, buildingMeshes);
        createAttachments(newBuilding, district, size, attatchmentPercentage, storiesTall, buildingMeshes);
        addWindows(newBuilding, district, size, attatchmentPercentage, storiesTall, windowMeshes);

        // Set all the materials we added to lists while making the meshes.
        if (buildingMeshes.Count > 1)
        {
            for (int i = 0; i < buildingMeshes.Count; ++i)
            {
                buildingMeshes[i].material = buildingMaterial;
            }
        }
        if (windowMeshes.Count > 1)
        {
            for (int i = 0; i < windowMeshes.Count; ++i)
            {
                windowMeshes[i].material = windowMaterial;
            }
        }
        return newBuilding;
    }

    /// <summary>
    /// Called during building creation, spawns a base building and roof.
    /// </summary>
    private void createBase(ProceduralBuildingInstance newBuilding, DistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall, List<MeshRenderer> buildingMeshes)
    {
        // Set buildingbase
        ProceduralBuildingBase newBase = district.SizeXDefinitions[(int)size].DistrictSizeXBases[GetNewBuilding(district, size, BuildingParts.Base)];         
      
        // Create the base
        GameObject newObject = (GameObject)Instantiate(newBase.gameObject, newBuilding.transform);
        newBase = newObject.GetComponent<ProceduralBuildingBase>();
        newObject.transform.position = Vector3.zero;

        if (newBase.HeightType == HeightType.Stretch)
        {
            newObject.transform.localScale = new Vector3(1f, storiesTall, 1f);
        }
        else if (newBase.HeightType == HeightType.Stack)
        {
            if (newBase.StackableObject == null)
            {
                Debug.LogError("Building base: " + newBase + "is set to Stack but does not have a stackable object set.");
            }
            else
            {
                GameObject newObjectA = newObject;
                GameObject newObjectB = newObject;
                for (int i = 0; i < storiesTall; ++i)
                {
                    newObjectA = Instantiate(newBase.StackableObject);
                    newObjectA.transform.position = new Vector3(0f, storyHeightUnits * i, 0f);
                    newObjectA.transform.SetParent(newObjectB.transform);
                    newObjectB = newObjectA;
                }
                // Move the roof node up.
                print(new Vector3(0f, storyHeightUnits * storiesTall, 0f));
                newBase.RoofLocation.position = new Vector3(0f, storyHeightUnits * storiesTall, 0f);
            }
        }
        // Set the new base to the instance class.
        newBuilding.BuildingBase = newObject.GetComponent<ProceduralBuildingBase>();
        buildingMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());

        // Set the roof
        newBuilding.BuildingRoof = placeRoof(newBuilding.BuildingBase.RoofLocation, district, size, storiesTall, buildingMeshes, newBuilding.BuildingBase.HasRoof);
    }

    /// <summary>
    /// Returns a random building piece by district, size and type
    /// </summary>
    private int GetNewBuilding(DistrictDefinition thisDistrict, BaseSize size, BuildingParts part)
    {
        for (int i = 0; i < thisDistrict.SizeXDefinitions.Length; ++i)
        {
            if((int)size == i && thisDistrict.SizeXDefinitions[i].GetTypeLengthByType(part) > 0)
            {
                return thisDistrict.SizeXDefinitions[i].getRandomEntryByType(size, part).PartNumber;
            }
        }
        Debug.LogError(districts + " has no buildings of size " + size + ". Your building was not properly created!");
        return -1;
    } 

    /// <summary>
    /// Called during building creation, spawns attachments for a building base and spawns roofs for all attachements.
    /// </summary>
    private void createAttachments(ProceduralBuildingInstance newBuilding, DistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall, List<MeshRenderer> buildingMeshes)
    {
        // Set Attatchments
        if (district.DistrictAttachments.Length > 0 && attatchmentPercentage > 0f && newBuilding.BuildingBase.AttachmentPoints.Length > 0)
        {
            // Because attachment percentage is defined as an int, devide it by 100 to bring it into a proper percent.
            int numberOfAttachments = Mathf.RoundToInt(newBuilding.BuildingBase.AttachmentPoints.Length * attatchmentPercentage / 100f);

            newBuilding.BuildingAttachments = new ProceduralBuildingAttachment[numberOfAttachments];

            // Create a list of numbers the length of the attatchment points. 
            // By moving through a list of randomly sorted, non-duplicate ints we insure that we don't place an attachment on the same point more than once.
            // As opposed to using Random.Range() every time which has the potential to repeat entries.
            List<int> randomPoint = Enumerable.Range(0, newBuilding.BuildingBase.AttachmentPoints.Length).ToList<int>();

            // Shuffle the list to make it random
            for (int i = 0; i < randomPoint.Count; ++i)
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
                GameObject newObject = (GameObject)Instantiate(newAttachment.gameObject);
                buildingMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());

                newAttachment = newObject.GetComponent<ProceduralBuildingAttachment>();
                newAttachment.AttachmentPoint = randomPoint[i];
                newObject.transform.localScale = new Vector3(1f, storiesTall, 1f);
                newObject.transform.SetParent(newBuilding.BuildingBase.AttachmentPoints[newAttachment.AttachmentPoint].transform);
                newObject.transform.localPosition = Vector3.zero;
                // Rotate the attatchment based off the modelling/prefab setup.
                newObject.transform.localRotation = Quaternion.Euler(0f, attachmentRotationMod, 0f);
                newBuilding.BuildingAttachments[i] = newAttachment;
            }

            // give the new attachments roofs if they need them, set the roof's scales properly if they don't.
            for (int i = 0; i < newBuilding.BuildingAttachments.Length; ++i)
            {
                if (newBuilding.BuildingAttachments[i] != null)
                { 
                    newBuilding.BuildingAttachments[i].HasRoof = placeRoof(newBuilding.BuildingAttachments[i].RoofLocation, district, BaseSize.OneByOne, storiesTall, buildingMeshes, newBuilding.BuildingAttachments[i].HasRoof);
                }
            }
        }
    }

    /// <summary>
    /// handles the placing, and / or addition of roofs to attachments and to building bases.
    /// </summary>
    private ProceduralBuildingRoof placeRoof(Transform roofLocation, DistrictDefinition district, BaseSize roofSize, int storiesTall, List<MeshRenderer> buildingMeshes, ProceduralBuildingRoof hasRoof = null)
    {
        ProceduralBuildingRoof newRoof;
        GameObject roofObject;
        if (hasRoof == null)
        {
            newRoof = district.SizeXDefinitions[(int)roofSize].DistrictSizeXRoofs[GetNewBuilding(district, roofSize, BuildingParts.Roof)];

            // Create the roof
            roofObject = (GameObject)Instantiate(newRoof.gameObject);
            buildingMeshes.AddRange(roofObject.GetComponentsInChildren<MeshRenderer>());
            roofObject.transform.localScale = Vector3.one;
            roofObject.transform.SetParent(roofLocation);
            roofObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            newRoof = hasRoof;
            roofObject = newRoof.gameObject;
            // To set a global scale of an object you need to have it unparented to everything.
            roofObject.transform.SetParent(null);
            roofObject.transform.localScale = Vector3.one;
            roofObject.transform.SetParent(roofLocation);
            roofObject.transform.localPosition = Vector3.zero;
        }

        // If the new roof can be rotated rotate it! But only at 90 degree intervals.
        if (newRoof.CanRotate == true)
        {
            // At 90 degree intervals, the building can only rotate up to 3 times before it's back to where it was.
            roofObject.transform.Rotate(new Vector3(0f, Random.Range(0, 3) * 90, 0f));
        }

        // Return the script on the gameobject so that we're not returning part of a prefab.
        return roofObject.GetComponent<ProceduralBuildingRoof>();
    }

    /// <summary>
    /// Called during building creation, adds windows to either a base or to attachments.
    /// </summary>
    private void addWindows(ProceduralBuildingInstance newBuilding, DistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall, List<MeshRenderer> windowMeshes)
    {
        // grab a window to use for all the windows on the building.
        ProceduralBuildingWindow newWindow = district.DistrictWindows[Random.Range(0, district.DistrictWindows.Length)];
        newBuilding.Windows = newWindow;
        float windowWidth = newWindow.Width;
        float windowSpacing = Random.Range(district.DistrictMinWindowSpacings, district.DistrictMaxWindowSpacings);
        newBuilding.WindowSpacing = windowSpacing;

        // Check to see if we need to add windows to the base.
        if (newBuilding.BuildingBase.AddWindows)
        {
            // List of the attachment points I can't add windows to because there is an attachment there.
            List<int> usedPoints = new List<int>();
            if (newBuilding.BuildingAttachments != null)
            {
                for (int i = 0; i < newBuilding.BuildingAttachments.Length; ++i)
                {
                    usedPoints.Add(newBuilding.BuildingAttachments[i].AttachmentPoint);
                }
            }

            // Mathmatic Window placement
            if (newBuilding.BuildingBase.WindowPoints.Length == 0)
            {
                // Number of windows per side of building base.
                int numberOfWindowsPerSide = Mathf.FloorToInt(OneBaseLengthUnits / (windowWidth + windowSpacing));

                for (int i = 0; i < newBuilding.BuildingBase.AttachmentPoints.Length; ++i)
                {
                    // For each side check if it does not have an attachment.
                    if (!usedPoints.Contains(i))
                    {
                        // Repeat spawning for each story.
                        for (int j = 0; j < storiesTall; ++j)
                        {
                            // Repeat spawning for number of windows on each side.
                            for (int k = 0; k < numberOfWindowsPerSide; ++k)
                            {
                                placeMathmaticWindow(newBuilding, newBuilding.BuildingBase.transform, newBuilding.BuildingBase.AttachmentPoints[i].transform.position, windowMeshes, i, j, k, 0);
                            }
                        }
                    }
                }
            }
            // Node based window placement
            else
            {   
                for (int i = 0; i < newBuilding.BuildingBase.WindowPoints.Length; ++i)
                {
                    // For each window attatchment, make sure it's not parented to a used attachmentpoint.
                    if (!usedPoints.Contains(i))
                    {
                        // Repeat spawning for each story
                        for (int j = 0; j < storiesTall; ++j)
                        {
                            placeNodeBasedWindow(newBuilding, newBuilding.transform, newBuilding.BuildingBase.WindowPoints[i].transform.position, windowMeshes, i, j, 0);
                        }
                    }
                }
            }
        }

        // Check the attatchment windows
        if (newBuilding.BuildingAttachments != null)
        {
            for (int i = 0; i < newBuilding.BuildingAttachments.Length; ++i)
            {
                if (newBuilding.BuildingAttachments[i] != null && newBuilding.BuildingAttachments[i].AddWindows)
                {
                    // Mathmatic Window placement
                    if (newBuilding.BuildingAttachments[i].WindowPoints.Length == 0)
                    {
                        // Number of windows per side of building base.
                        int numberOfWindowsPerSide = Mathf.FloorToInt(OneBaseLengthUnits / (windowWidth + windowSpacing));

                        // Three sides to spawn windows on
                        for (int m = 0; m < 3; ++m)
                        {
                            Vector3 centerPosition = Vector3.zero;
                            if (m == 0)
                            { 
                                centerPosition = newBuilding.BuildingAttachments[i].transform.position 
                                                + (newBuilding.BuildingAttachments[i].transform.right * HalfBaseLengthUnits)
                                                + (newBuilding.BuildingAttachments[i].transform.forward * HalfBaseLengthUnits);
                            }
                            else if (m == 1)
                            {
                                centerPosition = newBuilding.BuildingAttachments[i].transform.position 
                                                  + (newBuilding.BuildingAttachments[i].transform.right * OneBaseLengthUnits);
                            }
                            else if (m == 2)
                            {
                                centerPosition = newBuilding.BuildingAttachments[i].transform.position 
                                                + (newBuilding.BuildingAttachments[i].transform.right * HalfBaseLengthUnits) 
                                                - (newBuilding.BuildingAttachments[i].transform.forward * HalfBaseLengthUnits);
                            }

                            // Repeat spawning for each story
                            for (int j = 0; j < storiesTall; ++j)
                            {
                                // Repeat spawning for number of windows on each side
                                for (int k = 0; k < numberOfWindowsPerSide; ++k)
                                {
                                    placeMathmaticWindow(newBuilding, newBuilding.BuildingAttachments[i].transform, centerPosition, windowMeshes, 0, j, k, m+1);                       
                                }
                            }
                        }
                    }
                    // Node based window placement
                    else
                    {
                        // Repeat spawning for each story
                        for (int j = 0; j < storiesTall; ++j)
                        {
                            for (int k = 0; k < newBuilding.BuildingAttachments[i].WindowPoints.Length; ++k)
                            {
								placeNodeBasedWindow(newBuilding, newBuilding.BuildingAttachments[i].WindowPoints[k].transform, newBuilding.BuildingAttachments[i].WindowPoints[k].transform.position, windowMeshes, k, j, 1);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Places a window on a building using the mathmatic system.
    /// </summary>
    private void placeMathmaticWindow(ProceduralBuildingInstance newBuilding, Transform windowParent, Vector3 startPosition, List<MeshRenderer> windowMeshes, int attachmentPoint, int buildingStory, int windowNumber, int currentSpawnSide)
    {
        // With no attachment, windows can be added.                    
        GameObject newObject = (GameObject)Instantiate(newBuilding.Windows.gameObject);
        windowMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
        newObject.transform.parent = windowParent;
        newObject.transform.position = startPosition;
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, attachmentRotationMod + attachmentWindowRotationMod * currentSpawnSide, 0f) + newBuilding.BuildingBase.AttachmentPoints[attachmentPoint].transform.eulerAngles);

        // Set window position, devide lengths by 2 so we do calculations considering their center, not their ends.
        newObject.transform.position += newObject.transform.forward * ((newBuilding.WindowSpacing + newBuilding.Windows.Width) * windowNumber - HalfBaseLengthUnits
                                        + newBuilding.Windows.Width + newBuilding.WindowSpacing / 2f);

        raiseWindow(newObject, buildingStory);
    }

    /// <summary>
    /// Places a window on a building using the node based system
    /// </summary>
	private void placeNodeBasedWindow(ProceduralBuildingInstance newBuilding, Transform windowParent, Vector3 startPosition, List<MeshRenderer> windowMeshes, int windowNumber, int buildingStory, int isAttachment)
    {
        GameObject newObject = (GameObject)Instantiate(newBuilding.Windows.gameObject);
        windowMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
        newObject.transform.parent = windowParent;
        newObject.transform.position = startPosition;
        newObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, attachmentRotationMod - attachmentWindowRotationMod * isAttachment, 0f));
		if (isAttachment == 0)
		{
			newObject.transform.localRotation = Quaternion.Euler(newObject.transform.localRotation.eulerAngles + newBuilding.BuildingBase.WindowPoints [windowNumber].transform.eulerAngles);
		}
        raiseWindow(newObject, buildingStory);
    }

    /// <summary>
    /// Raises a window to match the proper height for the story it's on.
    /// </summary>
    private void raiseWindow(GameObject window, int buildingStory)
    {
        // Move the window up based on the story of the building we're putting windows on. 
        // Use half of storyHeightUnits so that the windows get placed in the middle of stories and not on the top of a story.
        window.transform.position += new Vector3(0f, (buildingStory * storyHeightUnits) + storyHeightUnits / 2f, 0f);
    }
}


