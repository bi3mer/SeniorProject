using UnityEngine;
using System.Collections.Generic;

public class DistrictItemRarityConfiguration
{
	private List<WeightedPair> weightedPairs;
	private const float floatingPointAccuracy = 0.0001f;

	public DistrictItemRarityConfiguration()
	{
		weightedPairs = new List<WeightedPair>();
	}

	/// <summary>
	/// Gets a random index corresponding to an item that is weighted according to the rarity of the item.
	/// </summary>
	/// <returns>A weighted random index.</returns>
	public int GetWeightedRandomItemIndex()
	{
		// randomly chooses a weighted pair to "flip"
		int fair = Random.Range(0, weightedPairs.Count);

		// random number between 0 and 1 which will determine which of the weighted pair will be selected
		// liken to a coin flip
		float flip = Random.value;

		if(flip < weightedPairs[fair].Threshold)
		{
			return weightedPairs[fair].First;
		}

		return weightedPairs[fair].Second;
	}

	/// <summary>
	/// Sets up the information needed to use vose alias for weighted random generation.
	/// Random generation using this method involves generating random numbers at unequal rates depending on the weights assigned to the number.
	/// In this case, the index number associated with an item, and the weights are dependent on the rarity level of the item.
	/// To achieve this in constant time, weighted pairs are created and put into a list. Then a random weighted pair is picked from that list,
	/// and one of the parts of the weighted pair is returned.
	/// For a more thorough explanation of this method please go to this link: http://www.keithschwarz.com/darts-dice-coins/
	/// </summary>
	/// <param name="rarityWeights">The values assigned to rarities. The smaller the rarer.</param>
	public void SetUpVoseAlias(List<float> rarityWeights)
	{
		List<int> largeIndices = new List<int>();
		List<int> smallIndices = new List<int>();

		float cumulative = 0;
		float referencePerfectWeight;

		for(int i = 0; i < rarityWeights.Count; ++i)
		{
			cumulative += rarityWeights[i];
		}

		// the desired total value that each weighted pair should have
		referencePerfectWeight = cumulative / (float)rarityWeights.Count;

		// splits off into rarity weights into 3 categories
		// weights that are too small and need to be added to reach the desired total
		// weights that are too large and need to be subtracted from
		// weights that are already at the desired total, and thus will make up a weighted pair by themselves
		for(int k = 0; k < rarityWeights.Count; ++k)
		{
			if(rarityWeights[k] == referencePerfectWeight)
			{
				weightedPairs.Add(new WeightedPair(k, k, 1));
			}
			else if(rarityWeights[k] < referencePerfectWeight)
			{
				smallIndices.Add(k);
			}
			else
			{
				largeIndices.Add(k);
			}
		}

		// creates weighted pairs
		while(smallIndices.Count > 0 || largeIndices.Count > 0)
		{
			if(smallIndices.Count > 0 && largeIndices.Count > 0)
			{
				int small = smallIndices[0];
				int large = largeIndices[0];

				smallIndices.RemoveAt(0);
				largeIndices.RemoveAt(0);

				weightedPairs.Add(new WeightedPair(small, large, rarityWeights[small]/referencePerfectWeight));

				float remainder = rarityWeights[large] - (referencePerfectWeight - rarityWeights[small]);

				rarityWeights[large] = remainder;

				if(Mathf.Abs(remainder - referencePerfectWeight) > floatingPointAccuracy)
				{
					if(remainder > referencePerfectWeight)
					{
						largeIndices.Add(large);
					}
					else if(remainder < referencePerfectWeight)
					{
						smallIndices.Add(large);
					}
				}
				else
				{
					weightedPairs.Add(new WeightedPair(large, large, 1f));
				}
			}
			else
			{
				// Theoretically, the code should never reach this point
				// But just in case, to prevent infinite loops, this is in place
				Debug.LogError("Vose Alias set up failed");
				break;
			}
		}
	}

}
