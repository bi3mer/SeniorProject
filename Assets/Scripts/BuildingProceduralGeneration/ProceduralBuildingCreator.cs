using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProceduralBuildingCreator : MonoBehaviour
{
    // All of the different districts, and all of their procedural building assets
    [SerializeField]
    private ProceduralDistrictDefinition[] district;

    // Per building base Enums
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
    private ProceduralBuildingInstance proceduralBuilding;

    // The number of material instances to make per material.
    [SerializeField]
    private int materialInstances = 1;

    // Height of a single story
    private const float storyHeightUnits = .75f;
    // Height of 1 base building unit on the grid.
    private const float OneBaseLengthUnits = 3f;
    // Start rotation modifier of all attachments
    private const float attachmentRotationMod = 0f;
    // Rotation mod of all windows.
    private const float attachmentWindowRotationMod = 90f;

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
       // ShowBuilding(newbuilding);
       NewestBuilding = newbuilding;
    }
    [ContextMenu("Hide the newest building")]
    public void HideTestBuilding()
    {
        // HideBuilding(NewestBuilding);
    }
    [ContextMenu("Show the newest building")]
    public void ShowTestBuilding()
    {
        // ShowBuilding(NewestBuilding);
    }


    //Creates materials that all buildings in a district will use.
    [ContextMenu("CreateNewMaterials")]
    public void CreateMaterialsForAllDistricts()
    {
        for (int i = 0; i < district.Length; ++i)
        {
            CreateMaterialsForDistrict(district[i]);
        }
    }

    void CreateMaterialsForDistrict(ProceduralDistrictDefinition district)
    {
        district.districtProceduralMaterials.Clear();
        district.districtProceduralWindowMaterials.Clear();

        for (int i = 0; i < district.DistrictMaterials.Length; ++i)
        {
            for (int j = 0; j < materialInstances; ++j)
            {
                Material newMaterial = new Material(district.DistrictMaterials[i].shader);
                newMaterial.CopyPropertiesFromMaterial(district.DistrictMaterials[i]);
                newMaterial.color = materialAlbedoColors.Evaluate( j * 1f/materialInstances);
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
    /// Populates a new proceduralbuilding class and spawns it in the world.
    /// </summary>
    public ProceduralBuildingInstance CreateBuilding(ProceduralDistrictDefinition district, BaseSize size, float attatchmentPercentage, int storiesTall)
    {
        if(district.districtProceduralMaterials.Count == 0 || district.districtProceduralWindowMaterials.Count == 0)
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
        buildingMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
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
            buildingMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
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
        if(district.DistrictAttachments.Length > 0 && attatchmentPercentage > 0f && newBuilding.BuildingBase.AttachmentPoints.Length > 0)
        {
            int numberOfAttachments = Mathf.RoundToInt(newBuilding.BuildingBase.AttachmentPoints.Length * attatchmentPercentage/100f);
        
            newBuilding.BuildingAttachments = new ProceduralBuildingAttachment[numberOfAttachments];

            // Create a list of random numbers the length of the attatchment points.
            List<int> randomPoint = Enumerable.Range(0, newBuilding.BuildingBase.AttachmentPoints.Length).ToList<int>();


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
                buildingMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());

                newAttachment = newObject.GetComponent<ProceduralBuildingAttachment>();
                newAttachment.AttachmentPoint = randomPoint[i];
                newObject.transform.localScale = new Vector3(1f, storiesTall, 1f); //NEEDS TO SUPPORT SEPERATE STORIES VALUES IN FUTURE
                newObject.transform.SetParent(newBuilding.BuildingBase.AttachmentPoints[newAttachment.AttachmentPoint].transform);
                newObject.transform.localPosition = Vector3.zero;
                // Rotate the attatchment based off the modelling/prefab setup.
                newObject.transform.localRotation = Quaternion.Euler(0f, attachmentRotationMod, 0f);
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
                    buildingMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
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

        // grab a window to use for all the windows on the building.
        ProceduralBuildingWindow newWindow = district.DistrictWindows[Random.Range(0, district.DistrictWindows.Length)];
        float windowWidth = newWindow.Width;
        float windowSpacing = Random.Range(district.DistrictMinWindowSpacings, district.DistrictMaxWindowSpacings);

        // Check to see if we need to add windows to the base.
        if (newBuilding.BuildingBase.AddWindows)
        {
            // List of the attachment points I can't use
            List<int> usedPoints = new List<int>();
            if (newBuilding.BuildingAttachments != null)
            { 
                for (int k = 0; k < newBuilding.BuildingAttachments.Length; k++)
                {
                    usedPoints.Add(newBuilding.BuildingAttachments[k].AttachmentPoint);
                }
            } 

            // Mathmatic Window placement
            if (newBuilding.BuildingBase.WindowPoints.Length == 0)
            {
                // get the length of the size of the base.
                float baseSizeLength = OneBaseLengthUnits;

                // Number of windows per side of building base.
                int numberOfWindowsPerSide = Mathf.FloorToInt(baseSizeLength / (windowWidth + windowSpacing));

                for (int i = 0; i < newBuilding.BuildingBase.AttachmentPoints.Length; ++i)
                {
                    // For each side check if it does not have an attachment.
                    if (!usedPoints.Contains(i))
                    {
                        // Repeat spawning for each story
                        for (int l = 0; l < storiesTall; ++l)
                        {
                            // Repeat spawning for number of windows on each side
                            for (int j = 0; j < numberOfWindowsPerSide; ++j)
                            {
                                // With no attachment, windows can be added.                    
                                newObject = (GameObject)Instantiate(newWindow.gameObject);
                                windowMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
                                newObject.transform.parent = newBuilding.transform;
                                newObject.transform.position = newBuilding.BuildingBase.AttachmentPoints[i].transform.position;
                                newObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, attachmentRotationMod, 0f) + newBuilding.BuildingBase.AttachmentPoints[i].transform.eulerAngles);

                                // Set window position
                                newObject.transform.position += newObject.transform.forward * ((windowSpacing + windowWidth) * j - (baseSizeLength / 2f) + windowWidth + windowSpacing /2f);
                                newObject.transform.position += new Vector3(0f, (l * storyHeightUnits) + storyHeightUnits /2f, 0f);
                            }
                        }
                    }
                }
            }
            // Node based window placement
            else
            {

                // Repeat spawning for each story
                for (int l = 0; l < storiesTall; ++l)
                {
                    // For each window attatchment, make sure it's not parented to a used attachmentpoint
                    for (int i = 0; i < newBuilding.BuildingBase.WindowPoints.Length; i++)
                    {
                        if (newBuilding.BuildingBase.WindowPoints[i].transform.parent.GetComponentInChildren<ProceduralBuildingAttachment>() == null)
                        {
                            // With no attachment, windows can be added.                    
                            newObject = (GameObject)Instantiate(newWindow.gameObject);
                            windowMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
                            newObject.transform.parent = newBuilding.transform;
                            newObject.transform.position = newBuilding.BuildingBase.WindowPoints[i].transform.position;
                            newObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, attachmentRotationMod, 0f) + newBuilding.BuildingBase.WindowPoints[i].transform.eulerAngles);
                            
                            // Set window height                         
                            newObject.transform.position += new Vector3(0f, (l * storyHeightUnits) + storyHeightUnits / 2f, 0f);       
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
                        
                        // get the length of the size of the base.
                        float baseSizeLength = OneBaseLengthUnits;
    
                        // Number of windows per side of building base.
                        int numberOfWindowsPerSide = Mathf.FloorToInt(baseSizeLength / (windowWidth + windowSpacing));
                       
                        // Three sides to spawn windows on
                        for (int m = 0; m < 3; ++m)
                        {
                            Vector3 centerPosition = Vector3.zero;
                            if (m == 0)
                            {
                                centerPosition = newBuilding.BuildingAttachments[i].transform.position + (newBuilding.BuildingAttachments[i].transform.right * baseSizeLength / 2f) + (newBuilding.BuildingAttachments[i].transform.forward * baseSizeLength / 2f);      
                            }
                            else if ( m == 1)
                            {
                                centerPosition = newBuilding.BuildingAttachments[i].transform.position + (newBuilding.BuildingAttachments[i].transform.right * baseSizeLength);
                            }
                            else if (m == 2)
                            {
                                centerPosition = newBuilding.BuildingAttachments[i].transform.position + (newBuilding.BuildingAttachments[i].transform.right * baseSizeLength / 2f) - (newBuilding.BuildingAttachments[i].transform.forward * baseSizeLength / 2f);
                            }

                            // Repeat spawning for each story
                            for (int l = 0; l < storiesTall; ++l)
                            {
                                // Repeat spawning for number of windows on each side
                                for (int j = 0; j < numberOfWindowsPerSide; ++j)
                                {                    
                                    newObject = (GameObject)Instantiate(newWindow.gameObject);
                                    windowMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
                                    newObject.transform.parent = newBuilding.BuildingAttachments[i].transform;
                                    newObject.transform.position = centerPosition;
                                    newObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, attachmentWindowRotationMod * (m+3), 0f));

                                    // Set window position
                                    newObject.transform.position += newObject.transform.forward * ((windowSpacing + windowWidth) * j - (baseSizeLength / 2f) + windowWidth + windowSpacing / 2f);
                                    newObject.transform.position += new Vector3(0f, (l * storyHeightUnits) + storyHeightUnits / 2f, 0f);
                                }
                            }
                        }
                    }
                    // Node based window placement
                    else
                    {
                        // Repeat spawning for each story
                        for (int l = 0; l < storiesTall; ++l)
                        {
                            // For each window attatchment, make sure it's not parented to a used attachmentpoint
                            for (int p = 0; p < newBuilding.BuildingAttachments[i].WindowPoints.Length; p++)
                            {
                                // With no attachment, windows can be added.                    
                                newObject = (GameObject)Instantiate(newWindow.gameObject);
                                windowMeshes.AddRange(newObject.GetComponentsInChildren<MeshRenderer>());
                                newObject.transform.parent = newBuilding.BuildingAttachments[i].WindowPoints[p].transform;
                                newObject.transform.position = newBuilding.BuildingAttachments[i].WindowPoints[p].transform.position;
                                newObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, attachmentRotationMod - attachmentWindowRotationMod, 0f));

                                // Set window height                         
                                newObject.transform.position += new Vector3(0f, (l * storyHeightUnits) + storyHeightUnits / 2f, 0f);
                            }
                        }
                    }
                }
            }
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
            for (int i = 0; i < windowMeshes.Count; ++i)
            {
                windowMeshes[i].material = windowMaterial;
            }
        }
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

        }
        else
        {
            Debug.LogError("Building " + building + " was already hidden");
        }
    }
}


