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
	public float DefaultWarmthDelay;
	public int DefaultWarmthDecrease; 
	public float Tier6Delay;
	public float Tier5Delay;
	public float Tier4Delay;
	public float Tier3Delay;
	public float Tier2Delay;
	public float Tier1Delay;
	public float Tier0Delay;
	public float TierNegativeDelay;
	public int HeatSourceRateMultiplier;
	public int ShelterRateMultiplier;
	public int WaterRateMultiplier;
	public int PneumoniaRateMultiplier;

	[Header ("Health Rate Settings")]
	public int DefaultHealthDecrease;
	public int DefaultHealthDelay;
	public int ZeroWarmthDelay;
	public int ZeroHungerDelay;
}