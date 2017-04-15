using UnityEngine;
using System.Collections;

/// <summary>
/// Sometimes you want something to be in a prefab, but not have a parent in the heirarchy. This script helps with that.
/// </summary>
public class NullParent : MonoBehaviour
{
    /// <summary>
    /// Sets the parent to null
    /// </summary>
    void Awake()
    {
        transform.SetParent(null);
    }
}
