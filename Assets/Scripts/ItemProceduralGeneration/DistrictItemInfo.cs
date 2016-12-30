using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistrictItemInfo
{
	private List<WeightedPair> weightedPairs;

	public DistrictItemInfo()
	{
		weightedPairs = new List<WeightedPair>();
	}

	public List<GameObject> ItemTemplates
	{
		get;
		set;
	}

	public List<float> DoorExtents
	{
		get;
		set;
	}

	public List<float> ItemExtents
	{
		get;
		set;
	}

	public List<GameObject> DoorTemplates
	{
		get;
		set;
	}

	public int GetWeightedRandomIndex()
	{
		int fair = Random.Range(0, weightedPairs.Count);
		float flip = Random.Range(0f, 1f);

		if(flip < weightedPairs[fair].Threshold)
		{
			return weightedPairs[fair].First;
		}

		return weightedPairs[fair].Second;
	}

	public void SetUpVoseAlias(List<float> rarityWeights)
	{
		List<float> weights = new List<float>();

		weightedPairs = new List<WeightedPair>();
		List<int> largeIndices = new List<int>();
		List<int> smallIndices = new List<int>();

		float cumulative = 0;
		float referencePerfectWeight;

		for(int i = 0; i < rarityWeights.Count; ++i)
		{
			cumulative += rarityWeights[i];
		}

		for(int j = 0; j < rarityWeights.Count; ++j)
		{
			weights.Add(rarityWeights[j]/cumulative);
		}

		referencePerfectWeight = cumulative / rarityWeights.Count;

		for(int k = 0; k < weights.Count; ++k)
		{
			if(weights[k] == referencePerfectWeight)
			{
				weightedPairs.Add(new WeightedPair(k, k, 0));
			}
			else if(weights[k] < referencePerfectWeight)
			{
				smallIndices.Add(k);
			}
			else
			{
				largeIndices.Add(k);
			}
		}

		while(smallIndices.Count > 0 || largeIndices.Count > 0)
		{
			if(smallIndices.Count > 0 && largeIndices.Count >0)
			{
				int small = smallIndices[0];
				int large = largeIndices[0];

				smallIndices.RemoveAt(0);
				largeIndices.RemoveAt(0);

				weightedPairs.Add(new WeightedPair(small, large, weights[small]));
				float remainder = weights[small] + weights[large] - referencePerfectWeight;

				if(remainder > referencePerfectWeight)
				{
					largeIndices.Add(large);
				}
				else
				{
					smallIndices.Add(large);
				}
			}
			else if(smallIndices.Count > 0)
			{
				if(smallIndices.Count > 1)
				{
					weightedPairs.Add(new WeightedPair(smallIndices[0], smallIndices[1], weights[smallIndices[0]]));
					smallIndices.RemoveAt(0);
					smallIndices.RemoveAt(1);
				}
				else
				{
					weightedPairs.Add(new WeightedPair(smallIndices[0], smallIndices[0], 0));
					smallIndices.RemoveAt(0);
				}
			}
			else if(largeIndices.Count > 0)
			{
				if(largeIndices.Count > 1)
				{
					weightedPairs.Add(new WeightedPair(largeIndices[0], largeIndices[1], weights[largeIndices[0]]));
					largeIndices.RemoveAt(0);
					largeIndices.RemoveAt(1);
				}
				else
				{
					weightedPairs.Add(new WeightedPair(largeIndices[0], largeIndices[0], 0));
					largeIndices.RemoveAt(0);
				}
			}
		}
	}
}