using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to the window prefabs. Holds window specific information needed when creating buildings.
/// </summary>
public class ProceduralBuildingWindow : MonoBehaviour
{
    [SerializeField]
    private float width;
    /// <summary>
    /// The width of this window.
    /// </summary>
    public float Width
    {
        get
        {
            return width;
        }
    }
}
