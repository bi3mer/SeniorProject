using UnityEngine;
using System.Collections;

public class TempBuildingCreator : MonoBehaviour 
{
    public GameObject BuildingTemplete;
    public float heightPerFloor;

    public GameObject CreateBuilingInstance (Vector3 position, int floors)
    {
        GameObject instance = Instantiate(BuildingTemplete) as GameObject;

        float height = floors * heightPerFloor;
        instance.transform.localScale = new Vector3(1f, height, 1f);
        instance.transform.position = new Vector3(position.x, height/2, position.z);

        return instance;
    }
}
