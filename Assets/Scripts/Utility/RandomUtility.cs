using UnityEngine;

public class RandomUtility
{
	/// <summary>
	/// Gets a random true or false.
	/// </summary>
	/// <value><c>true</c> if random bool; otherwise, <c>false</c>.</value>
	public static bool RandomBool
	{
		get
		{
			return Random.value > .5;
		}
	}

    /// <summary>
    /// Get a random float between 0 and 1.
    /// </summary>
    public static float RandomPercent
    {
        get
        {
            return Random.Range(0f, 1f);
        }
    }

    /// <summary>
    /// Get a random float between 0 and 100.
    /// </summary>
    public static float RandomHundredPercent
    {
        get
        {
            return Random.Range(0f, 100f);
        }
    }

    /// <summary>
    /// Randoms binomial with higher likelihood of 0
    /// </summary>
    /// <returns>The binomial.</returns>
    public static float RandomBinomial
    {
    	get
		{
			return RandomUtility.RandomPercent - RandomUtility.RandomPercent;
		}
    }

    /// <summary>
	/// Generates a random Vector2
    /// </summary>
    /// <returns>The vector.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static Vector2 RandomVector2d(Vector2 min, Vector2 max)
    {
    	return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.x));
    }

    /// <summary>
	/// Generates a random Vector2
    /// </summary>
    /// <returns>The vector.</returns>
    /// <param name="max">Max magnitude.</param>
    public static Vector2 RandomVector2d(float max)
    {
        float root2 = Mathf.Sqrt(2);
        return new Vector2(Random.Range(-max/root2, max/root2), Random.Range(-max/root2, max/root2));
    }

    /// <summary>
	/// Generates a random Vector3
    /// </summary>
    /// <returns>The vector3d.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    public static Vector3 RandomVector3d(Vector3 min, Vector3 max)
    {
    	return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}
