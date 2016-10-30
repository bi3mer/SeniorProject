using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingStat
{
	// the name of the stat to be considered during crafting
	public string statName { get; set; }

	// the items (by tag) that will be considered when finding the result of the stat
	public List<string> affectingItems { get; set; }

	// the values that the item must reach in order to be considered good or excellent
	// failing to reach either of these values will result in a "poor" item
	public List<int> threshold { get; set; }
}
