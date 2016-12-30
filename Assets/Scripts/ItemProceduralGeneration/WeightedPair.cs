using UnityEngine;
using System.Collections;

public class WeightedPair
{
	public WeightedPair(int first, int second, float threshold)
	{
		First = first;
		Second = second;
		Threshold = threshold;
	}

	public int First
	{
		get;
		set;
	}

	public int Second
	{
		get;
		set;
	}

	public float Threshold
	{
		get;
		set;
	}
}
