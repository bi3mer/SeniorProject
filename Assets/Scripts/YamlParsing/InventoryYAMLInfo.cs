using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryYAMLInfo
{
	public string itemName { get; set; }
	public float itemAmount { get; set; }
	public string unmodifiedItemName { get; set; }
	public List<string> modifications { get; set; }
}
