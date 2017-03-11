using UnityEngine;
using System.Collections;

/// <summary>
/// This class holds all the info that comes with making a procedural building
/// </summary>
public class ProceduralBuildingInstance : MonoBehaviour
{
    public ProceduralBuildingBase BuildingBase;
    public ProceduralBuildingWindow Windows;
    public float WindowSpacing;
    public ProceduralBuildingRoof BuildingRoof;
    public ProceduralBuildingAttachment[] BuildingAttachments;
    public Material BuildingMaterial;

    /// <summary>
    /// Used during item placement, returns all of the roof objects
    /// </summary>
    /// <returns> Returns all of the roofs as gameobjects</returns>
    public GameObject[] GetRoofs()
    {
        // The length of the roofs a building has is 1 for the base's roof + all of the attachments roofs
        GameObject[] roofs = new GameObject[1 + BuildingAttachments.Length];

        roofs[0] = BuildingRoof.gameObject;

        for (int i = 0; i < BuildingAttachments.Length; ++i)
        {
            // Stagger all placement by 1 becasue the base's roof is already in the array at position 0
            roofs[i + 1] = BuildingAttachments[i].HasRoof.gameObject;
        }

        return roofs;
    }
    
    /// <summary>
    /// Used during item placement, returns all of the roof object meshes.
    /// </summary>
    /// <returns>the meshes of all the roofs.</returns>
    public Mesh[] GetRoofMeshes()
    {
        GameObject[] roofs = GetRoofs();
        Mesh[] roofMeshes = new Mesh[roofs.Length];
  
        for (int i = 0; i < roofs.Length; ++i)
        {
            roofMeshes[i] = roofs[i].GetComponent<MeshFilter>().sharedMesh;
        }

        return roofMeshes;   
    }
}
