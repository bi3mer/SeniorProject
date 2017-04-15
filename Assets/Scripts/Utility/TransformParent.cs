using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Can Parent one transform to another while ignoring specific axis.
/// </summary>
public class TransformParent : MonoBehaviour
{
    public bool UsePositionX;
    public bool UsePositionY;
    public bool UsePositionZ;

    public Transform Parent;


    /// <summary>
    /// Update the position based on the bools
    /// </summary>
    void Update ()
    {
        transform.position = new Vector3(Parent.position.x * Convert.ToInt32(UsePositionX), Parent.position.y * Convert.ToInt32(UsePositionY), Parent.position.z * Convert.ToInt32(UsePositionZ));
	}
}
