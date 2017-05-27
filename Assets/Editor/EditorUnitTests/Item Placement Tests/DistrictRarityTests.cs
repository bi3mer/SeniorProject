using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[TestFixture]
public class DistrictRarityTests
{
	private float errorMargin = 0.2f;
	private int timeToRun = 10000;

	public DistrictItemRarityConfiguration CreateTestDistrict()
	{
		//Arrange
		DistrictItemRarityConfiguration rarity = new DistrictItemRarityConfiguration();
		List<float> rarityValues = new List<float>();

		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Common));
		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Uncommon));
		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Rare));
		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Legendary));

		rarity.SetUpVoseAlias(rarityValues);

		return rarity;
	}

	[Test] 
	public void RarityWeightTest()
	{
		int[] returnedRarityCounts = new int[4];
		DistrictItemRarityConfiguration config = CreateTestDistrict();

		List<float> rarityValues = new List<float>();

		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Common));
		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Uncommon));
		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Rare));
		rarityValues.Add(ItemRarity.GetRarity(ItemRarity.Legendary));
		float rarityTotal = 0f;

		for(int i = 0; i < rarityValues.Count; ++i)
		{
			rarityTotal += rarityValues[i];
		}

		for(int i = 0; i < timeToRun; ++i)
		{
			returnedRarityCounts[config.GetWeightedRandomItemIndex()] ++;
		}

		for(int i = 0; i < returnedRarityCounts.Length; ++i)
		{
			Assert.LessOrEqual(Mathf.Abs((float)returnedRarityCounts[i]/timeToRun - rarityValues[i]/rarityTotal), errorMargin);
		}
	}
}


