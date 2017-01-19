using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationUtility
{
    /// <summary>
    /// Generates a list of 2D points using poison distribution, aligned perpandicular the the y-axis.
    /// </summary>
    /// <param name="seed">The generation seed.</param>
    /// <param name="bounds">The bounds of the distribution.</param>
    /// <param name="distanceBetweenPoints">The minimum distance between each seed point.</param>
    /// <returns>The list of distributed points, perpandicular to y-axis.</returns>
    public static List<Vector2> DistributeSeedPoints(int seed, Bounds bounds, float distanceBetweenPoints)
    {
        // use third party class to distribute points within bounds using poisson distribution algorithm
        PoissonDiscSampler sampler = new PoissonDiscSampler(bounds.size.x, bounds.size.z, distanceBetweenPoints, seed);

        // Get and add all the generated points
        // I used foreach because the class produces an iterator, not an array
        List<Vector2> seedPoints = new List<Vector2>();
        foreach (Vector2 sample in sampler.Samples())
        {
            // Make the points relative to the center of the bounds
            seedPoints.Add(new Vector2(sample.x - bounds.extents.x + bounds.center.x, sample.y - bounds.extents.z + bounds.center.z));
        }

        return seedPoints;
    }

    /// <summary>
    /// Converts 2D point aligned perpendicular to the y-axis to 3D point.
    /// </summary>
    /// <param name="vector">Point to convert.</param>
    /// <returns>Point in 3D space.</returns>
    public static Vector3 ToAlignedVector3(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    /// <summary>
    /// Converts a 3D point to a point in 2D point aligned perpendicular to the y-axis.
    /// </summary>
    /// <param name="vector">Point to convert.</param>
    /// <returns>Point in 2D point aligned perpendicular to the y-axis</returns>
    public static Vector3 ToAlignedVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    /// <summary>
    /// Adds a vertex to a list if it's not a duplicate of another already in the list.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="vertex">The vertex.</param>
    /// <returns>The list.</returns>
    public static List<Vector2> AddNonDuplicateVertex(List<Vector2> list, Vector2 vertex)
    {
        if (!list.Contains(vertex))
        {
            list.Add(vertex);
        }
        return list;
    }

    /// <summary>
    /// Determines if a point is inside a polygon defined by the list of verts.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="polygon"></param>
    /// <returns></returns>
    public static bool IsPointInPolygon(Vector2 point, ref Vector2[] polygon)
    {
        int polygonLength = polygon.Length;
        int i = 0;
        bool inside = false;

        float pointX = point.x;
        float pointY = point.y;

        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.y;
            inside ^= (endY > pointY ^ startY > pointY) &&
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }

        return inside;
    }

    /// <summary>
    /// Get the mimdpoint of a list of points by calculating the average.
    /// </summary>
    /// <param name="points">List of points from which to calculate the midpoint.</param>
    /// <returns>The midpoint.</returns>
    public static Vector2 GetMidpoint (List<Vector2> points)
    {
        Vector2 center = Vector2.zero;
        for (int i = 0; i < points.Count; ++i)
        {
            center += points[i];
        }

        return (center / points.Count);
    }

    /// <summary>
    /// Sort thel list of verticies clockwise around a center point.
    /// </summary>
    /// <param name="points">The list of verticies to sort.</param>
    /// <param name="center">The center points around which the verticies are sorted.</param>
    /// <returns>The sorted list of points.</returns>
    public static List<Vector2> SortVerticies (List<Vector2> points, Vector2 center)
    {
        points.Sort((v1, v2) =>
        {
            return IsClockwise(v1, v2, center);
        });

        return points;
    }

    /// <summary>
    /// Returns 1 if first comes before second in clockwise order.
    /// Returns -1 if second comes before first.
    /// Returns 0 if the points are identical.
    /// </summary>
    /// <param name="first">First.</param>
    /// <param name="second">Second.</param>
    /// <param name="origin">Origin.</param>
    public static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
    {
        if (first == second)
        {
            return 0;
        }

        Vector2 firstOffset = first - origin;
        Vector2 secondOffset = second - origin;

        float angle1 = Mathf.Atan2(firstOffset.x, firstOffset.y);
        float angle2 = Mathf.Atan2(secondOffset.x, secondOffset.y);

        if (angle1 < angle2)
        {
            return 1;
        }

        if (angle1 > angle2)
        {
            return -1;
        }

        // Check to see which point is closest
        return (firstOffset.sqrMagnitude < secondOffset.sqrMagnitude) ? 1 : -1;
    }

    /// <summary>
    /// Get the location of the vertex that is shared by the most districts.
    /// Does not garuntee any order.
    /// </summary>
    /// <param name="districts">List of populated districts.</param>
    /// <returns>Location of vertex shared by most districts.</returns>
    public static Vector3 GetMostCommonVertex(District[] districts)
    {
        // Tally the vertex frequencies
        Dictionary<Vector3, int> frequencies = new Dictionary<Vector3, int>();
        for (int i = 0; i < districts.Length; ++i)
        {
            District district = districts[i];

            for (int j = 0; j < district.EdgeVerticies.Length; ++j)
            {
                Vector3 vert = district.EdgeVerticies[j];

                int frequency;
                if (frequencies.TryGetValue(vert, out frequency))
                {
                    frequencies[vert] = frequency + 1;
                }
                else
                {
                    frequencies.Add(vert, 1);
                }
            }
        }

        // Return one of the most popular, doesn't matter which
        int max = 0;
        Vector3 mostPopular = Vector3.zero;
        foreach (Vector3 key in frequencies.Keys)
        {
            if (frequencies[key] > max)
            {
                max = frequencies[key];
                mostPopular = key;
            }
        }

        return mostPopular;
    }
}
