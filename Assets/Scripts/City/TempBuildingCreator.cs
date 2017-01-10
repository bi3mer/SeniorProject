using UnityEngine;
using System.Collections;

 /// <summary>
 /// Temporary class to be replaced with actual building generation code.
 /// </summary>
public class TempBuildingCreator : MonoBehaviour 
{
    public GameObject BuildingTemplete;
    public float heightPerFloor;

    public GameObject CreateBuilingInstance (Vector3 position, int floors)
    {
        GameObject instance = Instantiate(BuildingTemplete) as GameObject;

        float height = floors * heightPerFloor;
        instance.transform.localScale = new Vector3(3f, height, 3f);
        instance.transform.position = new Vector3(position.x, height/2, position.z);

        return instance;
    }
}
