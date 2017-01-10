using System.Collections.Generic;

public class ItemRarity
{
	public static string Common = "common";
	public static string Uncommon = "uncommon";
	public static string Rare = "rare";
	public static string Legendary = "legendary";

	private static List<string> rarityNames = new List<string> {Common, Uncommon, Rare, Legendary};
	private static List<float> rarityValues = new List<float> {5f, 2f, 0.5f, 0.05f};

	/// <summary>
	/// Gets the rarity value given a rarity string value.
	/// </summary>
	/// <returns>The rarity.</returns>
	/// <param name="rarity">Rarity.</param>
	public static float GetRarity(string rarity)
	{
		int index = rarityNames.IndexOf(rarity);
		if(index >= 0 && index < rarityValues.Count)
		{
			return rarityValues[index];
		}

		return -1;
	}
}
