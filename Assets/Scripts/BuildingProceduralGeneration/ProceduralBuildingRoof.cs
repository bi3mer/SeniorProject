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
}
