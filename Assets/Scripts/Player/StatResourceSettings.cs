using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatResourceSettings
{
	[Header ("Hunger Rate Settings")]
	public int DefaultHungerDelay;
	public int DefaultHungerDecrease;
	public int FoodPoisonDelay;
	public int FoodPoisonDecrease;

	[Header ("Warmth Rate Settings")]
	public int Tier6Delay;
	public int Tier5Delay;
	public int Tier4Delay;
	public int Tier3Delay;
	public int Tier2Delay;
	public int Tier1Delay;
	public int Tier0Delay;
	public int TierNegativeDelay;
	public float WaterRateMultiplier;
	public float ShelterRateMultiplier;
	public float HeatSourceRateMultiplier;
	public int PneumoniaDelay;
	public int PneumoniaDecrease;

	[Header ("Health Rate Settings")]
	public int DefaultHealthDelay;
	public int ZeroWarmthRateMultiplier;
	public int ZeroHungerRateMultiplier;
}