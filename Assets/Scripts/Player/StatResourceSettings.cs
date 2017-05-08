using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatResourceSettings
{
	[Header ("Hunger Rate Settings")]
	public int DefaultHungerDelay;

	[Header ("Warmth Rate Settings")]
	public int DefaultWarmthDelay;
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

	[Header ("Health Rate Settings")]
	public int DefaultHealthDelay;
	public int ZeroWarmthRateMultiplier;
	public int ZeroHungerRateMultiplier;
}