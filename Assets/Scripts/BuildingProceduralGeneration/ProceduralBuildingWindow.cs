using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to the window prefabs. Holds window specific information needed when creating buildings.
/// </summary>
public class ProceduralBuildingWindow : MonoBehaviour
{
    /// <summary>
    /// The width of this window.
    /// </summary>
    [SerializeField]
    private float width;
    public float Width
    {
        get
        {
            return width;
        }
    }
}
