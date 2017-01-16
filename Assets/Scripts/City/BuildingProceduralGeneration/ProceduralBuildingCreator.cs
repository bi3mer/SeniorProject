using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum HeightType
{
    Stretch,
    Stack
}

public enum BaseSize
{
    OneByOne,
    TwoByTwo,
    ThreeByThree
}

public enum BuildingParts
{
    Base,
    Roof
}

public struct buildingIndex
{
    public int BuildingSize;
    public int PartNumber;
}

/// <summary>
/// Defines all the buildings for each district and allows for the creation of builings in those districts.
/// </summary>
public class ProceduralBuildingCreator : MonoBehaviour
{

    [SerializeField]
    private ProceduralBuildingInstance proceduralBuildingTemplate;

    [Tooltip("The number of material instances to make per material, higher numbers lead to more varied cities but more memory usage.")]
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
    //How smooth building materials should be.
    [Range(0f, 1f)]
    private float materialSmoothness = .5f;

    // The number of tries to try any potentially failing random generation.
    private const int numberOfTries = 5;

    // A float representing a distance taller than roofs, to insure we can start a raycast from above them.
    private const float aboveRoofHeight = 5f;
    // A float for if an object needs to be rotated to be placed properly.
    private const float adjustmentAngle = 90f;



    public DistrictConfiguration TestDistrict;
    public BaseSize TestSize;
    public int TestStoriesTall;
    public float TestAttatchmentPercentage;

    [ContextMenu("CreateTestBuilding")]
    public void CreateTestBuilding()
    {
        CreateBuilding(TestDistrict, TestSize, TestAttatchmentPercentage, TestStoriesTall);
    }


    /// <summary>
    /// Creates all the procedural materials for a single district.
    /// </summary>
    /// <param name="district"></param>
    void CreateMaterialsForDistrict(DistrictConfiguration district)
    {
        //15 is a fairly large number for material instances. We'll probably settle around 5-10.
        if(materialInstances > 15)
        {
            Debug.LogWarning("materialInstances may be too high. Consider lowering it to improve performance");
        }

        district.districtProceduralMaterials.Clear();
        district.districtProceduralWindowMaterials.Clear();

        for (int i = 0; i < district.DistrictMaterials.Length; ++i)
        {
            for (int j = 0; j < materialInstances; ++j)
            {
                Material newMaterial = new Material(district.DistrictMaterials[i].shader);
                newMaterial.CopyPropertiesFromMaterial(district.DistrictMaterials[i]);
                newMaterial.SetFloat(StandardShaderSmoothness, materialSmoothness);
                newMaterial.color = district.MaterialAlbedoColors.Evaluate(Random.value);
                district.districtProceduralMaterials.Add(newMaterial);
            }
        }

        for (int i = 0; i < district.DistrictWindowMaterials.Length; ++i)
        {
            for (int j = 0; j < materialInstances; ++j)
            {
                Material newMaterial = new Material(district.DistrictWindowMaterials[i].shader);
                newMaterial.CopyPropertiesFromMaterial(district.DistrictWindowMaterials[i]);
                newMaterial.SetFloat(StandardShaderSmoothness, materialSmoothness);
                newMaterial.color = district.MaterialAlbedoColors.Evaluate(Random.value);
                district.districtProceduralWindowMaterials.Add(newMaterial);
            }
        }
    }

    /// <summary>
    /// Populates a new proceduralbuilding class and spawns it in the world.
    /// </summary>
    public ProceduralBuildingInstance CreateBuilding(DistrictConfiguration district, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        // Check to see if we need new materials.
        if (district.districtProceduralMaterials.Count == 0 || district.districtProceduralWindowMaterials.Count == 0)
        {
            CreateMaterialsForDistrict(district);
        }
        
        ProceduralBuildingInstance newBuilding = (ProceduralBuildingInstance)Instantiate(proceduralBuildingTemplate, Vector3.zero, Quaternion.Euler(Vector3.zero));
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
        if (district.DistrictWindowWashers.Length > 0)
        {
            addWindowWasher(newBuilding, district, storiesTall);
        }

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
            MeshFilter[] windowMeshFilters = new MeshFilter[windowMeshes.Count];
            for (int i = 0; i < windowMeshes.Count; ++i)
            {
                windowMeshFilters[i] = windowMeshes[i].GetComponent<MeshFilter>();
            }

            // Combine all the window meshes
            CombineInstance[] combine = new CombineInstance[windowMeshFilters.Length];
            for (int i = 0; i < windowMeshFilters.Length; ++i)
            {
                combine[i].mesh = windowMeshFilters[i].sharedMesh;
                combine[i].transform = windowMeshes[i].transform.localToWorldMatrix;
                windowMeshFilters[i].gameObject.SetActive(false);
            }
            newBuilding.gameObject.AddComponent<MeshFilter>().sharedMesh = new Mesh();
            MeshRenderer windowRenderer = newBuilding.gameObject.AddComponent<MeshRenderer>();
            windowRenderer.material = windowMaterial;
            newBuilding.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
            newBuilding.transform.gameObject.SetActive(true);

            // Delete old windows
            for (int i = windowMeshes.Count - 1; i >= 0; --i)
            {
                DestroyImmediate(windowMeshes[i].gameObject);
            }
        }
        return newBuilding;
    }

    /// <summary>
    /// Called during building creation, spawns a base building and roof.
    /// </summary>
    private void createBase(ProceduralBuildingInstance newBuilding, DistrictConfiguration district, BaseSize size, float attatchmentPercentage, int storiesTall, List<MeshRenderer> buildingMeshes)
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
    private int GetNewBuilding(DistrictConfiguration district, BaseSize size, BuildingParts part)
    {
        for (int i = 0; i < district.SizeXDefinitions.Length; ++i)
        {
            if((int)size == i &&  district.SizeXDefinitions[i].GetTypeLengthByType(part) > 0)
            {
                return district.SizeXDefinitions[i].GetRandomEntryByType(size, part).PartNumber;
            }
        }
        Debug.LogError(district.Name + " has no buildings of size " + size + ". Your building was not properly created!");
        return -1;
    } 

    /// <summary>
    /// Called during building creation, spawns attachments for a building base and spawns roofs for all attachements.
    /// </summary>
    private void createAttachments(ProceduralBuildingInstance newBuilding, DistrictConfiguration district, BaseSize size, float attatchmentPercentage, int storiesTall, List<MeshRenderer> buildingMeshes)
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
    private ProceduralBuildingRoof placeRoof(Transform roofLocation, DistrictConfiguration district, BaseSize roofSize, int storiesTall, List<MeshRenderer> buildingMeshes, ProceduralBuildingRoof hasRoof = null)
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
    private void addWindows(ProceduralBuildingInstance newBuilding, DistrictConfiguration district, BaseSize size, float attatchmentPercentage, int storiesTall, List<MeshRenderer> windowMeshes)
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

    /// <summary>
    /// Adds a window washer on the building at an available spot if any.
    /// </summary>
    private void addWindowWasher(ProceduralBuildingInstance newBuilding, DistrictConfiguration district, int storiesTall)
    {
        WindowWasher newWashersPrefab = district.DistrictWindowWashers[Random.Range(0, district.DistrictWindowWashers.Length - 1)];
        // Check to see if a window washer is needed.
        // Since WindowWasherChance is a float between 0-100, we use that range for generating a random float.
        if (Random.Range(0f, 100f) <= district.WindowWasherChance)
        {
            int washerLocation = 0;
            Vector3 washerRotation = Vector3.zero;
            Vector3 washerPosition = Vector3.zero;
            float washerPositionMod = 0;

            // Pick a location for the washer to go.
            // We need to exclude putting the washer on the base of the building if it has no free sides.
            if (newBuilding.BuildingBase.AttachmentPoints.Length == newBuilding.BuildingAttachments.Length)
            {
                washerLocation = Random.Range(0, newBuilding.BuildingAttachments.Length);
            }
            else
            {
                // -1 is the base, 0+ is the corresponding attachment point in the base's attachment point array.
                washerLocation = Random.Range(-1, newBuilding.BuildingAttachments.Length);
            }

            // Placing on the building base
            if (washerLocation == -1)
            {
                // If the base uses node based windows default to trying one of them as the location.
                if (newBuilding.BuildingBase.WindowPoints.Length > 0)
                {
                    for (int i = 0; i < numberOfTries; ++i)
                    {
                        int windowPos = Random.Range(0, newBuilding.BuildingBase.WindowPoints.Length);

                        // Check to make sure the window point isn't on an attachment
                        if (newBuilding.BuildingBase.WindowPoints[windowPos].transform.parent.GetComponentInChildren<ProceduralBuildingAttachment>() == null)
                        {
                            washerPosition = newBuilding.BuildingBase.WindowPoints[windowPos].position;
                            washerRotation = newBuilding.BuildingBase.WindowPoints[windowPos].localEulerAngles;
                            break;
                        }
                    }
                }
                // Use mathmatical placement
                else
                {
                    // Pick a random unused attachment point to start with.
                    if (newBuilding.BuildingBase.AttachmentPoints.Length > 0)
                    {
                        int attachmentPoint = Random.Range(0, newBuilding.BuildingBase.AttachmentPoints.Length);
                        washerPosition = newBuilding.BuildingBase.AttachmentPoints[attachmentPoint].position;
                        washerRotation = newBuilding.BuildingBase.AttachmentPoints[attachmentPoint].transform.localEulerAngles + new Vector3(0f, adjustmentAngle, 0f);
                        washerPositionMod = Random.Range(-(HalfBaseLengthUnits - newWashersPrefab.WasherBaseLength), HalfBaseLengthUnits - newWashersPrefab.WasherBaseLength);
                    }
                }
            }
            // Placing on an attachment
            else
            {
                // If the attachment uses node based windows
                if (newBuilding.BuildingAttachments[washerLocation].WindowPoints.Length > 0)
                {
                    int windowPos = Random.Range(0, newBuilding.BuildingAttachments[washerLocation].WindowPoints.Length);
                    washerPosition = newBuilding.BuildingAttachments[washerLocation].WindowPoints[windowPos].position;
                    washerRotation = newBuilding.BuildingAttachments[washerLocation].WindowPoints[windowPos].localEulerAngles
                        + newBuilding.BuildingAttachments[washerLocation].transform.eulerAngles;
                }
                // Use mathmatical placment
                else
                {
                    washerPosition = newBuilding.BuildingBase.AttachmentPoints[washerLocation].position;
                    washerPosition += newBuilding.BuildingBase.AttachmentPoints[washerLocation].right * OneBaseLengthUnits;
                    washerRotation = newBuilding.BuildingBase.AttachmentPoints[washerLocation].transform.localEulerAngles + new Vector3(0f, adjustmentAngle, 0f);
                    washerPositionMod = Random.Range(-(HalfBaseLengthUnits - newWashersPrefab.WasherBaseLength), HalfBaseLengthUnits - newWashersPrefab.WasherBaseLength);
                }
            }

            // If a position and rotation was found then spawn the prefab.
            if (washerPosition != Vector3.zero && washerRotation != Vector3.zero)
            {
                // Pick a window washer prefab to use
                WindowWasher newWasher;
                newWasher = (WindowWasher)Instantiate(newWashersPrefab);
                newWasher.transform.SetParent(newBuilding.transform);
                newWasher.transform.eulerAngles = washerRotation;
                newWasher.transform.position += newWasher.transform.right * washerPositionMod;
                newWasher.transform.position = washerPosition;

                RaycastHit placementHit;
                // Raycast above the building where the washer is straight down 
                if (Physics.Raycast(newWashersPrefab.PlacementCenter.transform.position + newWasher.transform.position
                    + new Vector3(0f, storiesTall * (storyHeightUnits + aboveRoofHeight), 0f), Vector3.down, out placementHit))
                {
                    newWasher.transform.position += new Vector3(0f, placementHit.point.y, 0f);
                }
                else
                {
                    newWasher.transform.position += new Vector3(0f, storiesTall * (storyHeightUnits), 0f);
                }

                // Set max lower distance based on the animation curve.
                // Again this curve is only evaluated between 0 and 1 on the X axis.
                newWasher.MaxLowerDistance = district.WindowWasherMaxLengthCurve.Evaluate(Random.value);
                // Set if the new washer starts up or not. This is a percent chance so we use a range of 0-100 to evaluate.
                if (Random.Range(0f, 100f) <= district.WindowWasherStartUpChance)
                {
                    newWasher.StartUp = true;
                }
                else
                {
                    newWasher.StartUp = false;
                }
            }
        }
    }
}


