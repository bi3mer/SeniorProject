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
}
