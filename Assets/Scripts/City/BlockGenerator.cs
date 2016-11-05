using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField]
    private float distanceBetweenControlPoints;

    /// <summary>
    /// Generate the blocks within the specified ditrict based on the values specified in the block generator.
    /// </summary>
    /// <param name="district">The parent district.</param>
    /// <param name="randomSeed">The seed for generating the city.</param>
    /// <returns>An array of the generated blocks.</returns>
    public CityBlock[] Generate(District district, int randomSeed)
    {
        // Save the list of blocks as we generate them so we can add them all to the district
        List<CityBlock> blocks = new List<CityBlock>();
        List<Vector2> controlPoints = new List<Vector2>();

        // keep track of number of generated control points
        int gridHeight = 0;
        int gridWidth = 0;

        // create a grid  of pointsfrom min to max
        Vector3 min = district.BoundingArea.min;
        Vector3 max = district.BoundingArea.max;
        for (float i = min.x; i < max.x; i += distanceBetweenControlPoints)
        {
            ++ gridHeight;
            gridWidth = 0;

            for (float j = min.z; j < max.z; j += distanceBetweenControlPoints)
            {
                controlPoints.Add(new Vector2(i, j));
                ++ gridWidth;
            }
        }

        // call Voroni class to get edges for seed points
        //Voroni voroniGen = new Voroni(initialSeedPoints, districtMaxSpan, districtMaxSpan * 2);
        //districtEndPoints = voroniGen.GetEdges();
        //cityCenter = voroniGen.Center;

        // iterate through control points and create blocks
        for (int i = 0; i < controlPoints.Count; ++i)
        {
            List<Vector3> verts = new List<Vector3>();
            
            // find the points;

            // check if this is inside the district because they are irregularly shaped
            if (district.ContainsPoint(controlPoints[i]))
            {
                verts.Add(new Vector3(controlPoints[i].x, 0f, controlPoints[i].y));
            }

            CityBlock block = new CityBlock(verts.ToArray());
            blocks.Add(block);
        }

        return blocks.ToArray();
    }
}
