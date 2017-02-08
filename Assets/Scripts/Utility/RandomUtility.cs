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
}
