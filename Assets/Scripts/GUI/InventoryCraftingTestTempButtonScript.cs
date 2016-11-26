using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InventoryCraftingTestTempButtonScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject inventoryPanel;
	[SerializeField]
	private GameObject CraftingPanel;

	void Start()
	{
		OnCraftingButtonClick ();
		//OnInventoryButtonClick ();
	}

	public void OnInventoryButtonClick()
	{
		inventoryPanel.SetActive (true);
		CraftingPanel.SetActive (false);
	}

	public void OnCraftingButtonClick()
	{
		inventoryPanel.SetActive (false);
		CraftingPanel.SetActive (true);
	}
}
