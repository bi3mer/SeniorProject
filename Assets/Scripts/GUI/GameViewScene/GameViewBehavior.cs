using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameViewBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject inventoryPanel;
	[SerializeField]
	private GameObject pausePanel;
	[SerializeField]
	private GameObject radioPanel;
	[SerializeField]
	private GameObject craftingPanel;
	private ControlScheme controlScheme;

	/// <summary>
	/// Start the Game View with all other menu panels disabled.
	/// </summary>
	void Start () 
	{
		controlScheme = Game.Instance.Scheme;
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		radioPanel.SetActive (false);
		craftingPanel.SetActive (false);

		Game.Instance.GameViewInstance = this;
	}

	/// <summary>
	/// Get player input and update accordingly.
	/// </summary>
	void Update()
	{
		if (controlScheme != null) 
		{
			if (Input.GetKey (controlScheme.Pause)) 
			{
				if (Game.Instance.PauseInstance.IsPaused) 
				{
					OnResumeClick ();
				} 
				else 
				{
					OnPauseClick ();
				}
			}

			if (Input.GetKey (controlScheme.Inventory)) 
			{
				OnInventoryClick ();
			}

			if (Input.GetKey (controlScheme.Radio)) 
			{
				OnRadioClick ();
			}

			if (Input.GetKey (controlScheme.Crafting)) 
			{
				OnCraftingClick ();
			}
		}
	}
		
	/// <summary>
	/// Loads the pause panel.
	/// </summary>
	public void OnPauseClick() 
	{
		Game.Instance.PauseInstance.Pause ();
		pausePanel.SetActive (true);
		inventoryPanel.SetActive (false);
		radioPanel.SetActive (false);
		craftingPanel.SetActive (false);
	}

	/// <summary>
	/// Loads the inventory panel.
	/// </summary>
	public void OnInventoryClick()
	{
		if(InventoryUI.Instance.TargetInventory != Game.Instance.PlayerInstance.Inventory)
		{
			InventoryUI.Instance.LoadNewInventory(Game.Instance.PlayerInstance.Inventory);
		}

		OnInventoryOpen();
	}

	public void OnInventoryOpen()
	{
		inventoryPanel.SetActive (true);
		pausePanel.SetActive (false);
		radioPanel.SetActive (false);
		craftingPanel.SetActive (false);
	}

	/// <summary>
	/// Loads the radio panel.
	/// </summary>
	public void OnRadioClick()
	{
		radioPanel.SetActive (true);
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		craftingPanel.SetActive (false);
	}

	/// <summary>
	/// Returns to the game scene.
	/// </summary>
	public void OnResumeClick()
	{
		Game.Instance.PauseInstance.Resume ();
		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);
		radioPanel.SetActive (false);
		craftingPanel.SetActive (false);

		if(InventoryUI.Instance.ItemsToDiscard.Count > 0)
		{
			ItemDiscarder discarder = new ItemDiscarder();
			discarder.DiscardItems(InventoryUI.Instance.ItemsToDiscard);
		}
	}

	/// <summary>
	/// Loads the crafting panel.
	/// </summary>
	public void OnCraftingClick()
	{
		radioPanel.SetActive (false);
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		craftingPanel.SetActive (true);
	}
}
