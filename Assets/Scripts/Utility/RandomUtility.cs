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
    /// Get a random decimal
    /// </summary>
    /// <value>The decimal.</value>
    public static float Decimal
    {
    	get
    	{
    		return Random.Range(0f, 1f);
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
			return RandomUtility.Decimal - RandomUtility.Decimal;
		}
    }
}