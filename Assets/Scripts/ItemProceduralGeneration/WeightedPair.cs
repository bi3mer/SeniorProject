using UnityEngine;
using System.Collections;

public class WeightedPair
{
	/// <summary>
	/// Initializes a new instance of the <see cref="WeightedPair"/> class.
	/// </summary>
	/// <param name="first">First index value.</param>
	/// <param name="second">Second index value.</param>
	/// <param name="threshold">A higher threshold value will result in a lower chance of the second value being selected.</param>
	public WeightedPair(int first, int second, float threshold)
	{
		First = first;
		Second = second;
		Threshold = threshold;
	}

	/// <summary>
	/// Gets or sets the first value of the weighted pair.
	/// </summary>
	/// <value>The first.</value>
	public int First
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the second value of the weighted pair.
	/// </summary>
	/// <value>The second.</value>
	public int Second
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the threshold value that determines whether the first or second value is selected. The higher the threshold, the more likely
	/// the first value is selected.
	/// </summary>
	/// <value>The threshold.</value>
	public float Threshold
	{
		get;
		set;
	}
}
