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
			if (Input.GetKeyDown (controlScheme.Pause)) 
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

			if (Input.GetKeyDown(controlScheme.Inventory)) 
			{
				OnInventoryClick ();
			}

			if (Input.GetKeyDown(controlScheme.Radio)) 
			{
				OnRadioClick ();
			}

			if (Input.GetKeyDown(controlScheme.Crafting)) 
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
		OnInventoryOpen();
		if (GuiInstanceManager.InventoryUiInstance != null) 
		{
			if (GuiInstanceManager.InventoryUiInstance.TargetInventory != null && GuiInstanceManager.InventoryUiInstance.TargetInventory != Game.Instance.PlayerInstance.Inventory) 
			{
				GuiInstanceManager.InventoryUiInstance.LoadNewInventory (Game.Instance.PlayerInstance.Inventory);
			}
			else if(GuiInstanceManager.InventoryUiInstance.TargetInventory != null)
			{
				GuiInstanceManager.InventoryUiInstance.RefreshInventoryPanel();
			}
		}
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
		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);
		radioPanel.SetActive (false);
		craftingPanel.SetActive (false);

		if(GuiInstanceManager.InventoryUiInstance.ItemsToDiscard.Count > 0)
		{
			ItemDiscarder discarder = new ItemDiscarder();
			discarder.DiscardItems(GuiInstanceManager.InventoryUiInstance.ItemsToDiscard);
		}
	}

    /// <summary>
    /// Rusumes the game from pause
    /// </summary>
    public void Resume()
    {
        Game.Instance.PauseInstance.Resume();
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
