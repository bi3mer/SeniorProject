using UnityEngine;
using System.Collections;

public class ProceduralBuildingRoof : MonoBehaviour
{
    [SerializeField]
    private bool canRotate;
    public bool CanRotate
    {
        get
        {
            return canRotate;
        }
    }

    private int roofRotation;
    private bool roofRotationSet;
    public int RoofRotation
    {
        get
        {
            return roofRotation;
        }
        set
        {
            if (roofRotationSet == false)
            {
                roofRotationSet = true;
                roofRotation = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
        }
    }
}
