using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to the roof prefabs. Holds roof specific information needed when creating buildings.
/// </summary>
public class ProceduralBuildingRoof : MonoBehaviour
{
    /// <summary>
    /// If the roof can be rotated at 90 degree intervals when it's created.
    /// </summary>
    [SerializeField]
    private bool canRotate;
    public bool CanRotate
    {
        get
        {
            return canRotate;
        }
    }
}
