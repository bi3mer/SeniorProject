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
	private GameObject radioCanvas;

    [SerializeField]
    private InteractableRadioModel radioModel;

	[SerializeField]
	private RecipeBookBehavior craftingPanel;

	[SerializeField]
	private GameObject optionButtonPanel;

	private ControlScheme controlScheme;

	/// <summary>
	/// Start the Game View with all other menu panels disabled.
	/// </summary>
	void Start () 
	{
		controlScheme = Game.Instance.Scheme;
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		radioCanvas.SetActive (false);
		craftingPanel.gameObject.SetActive (false);
		optionButtonPanel.SetActive(true);

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
		radioCanvas.SetActive (false);
		craftingPanel.gameObject.SetActive (false);
		optionButtonPanel.SetActive(false);
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

        // do the deactivate radio menu before deactivating radio canvas.
        if (radioCanvas.activeInHierarchy)
        {
            radioModel.DeactivateRadio();
        }
		radioCanvas.SetActive (false);
		craftingPanel.gameObject.SetActive (false);
		optionButtonPanel.SetActive(false);
	}

	/// <summary>
	/// Loads the radio panel.
	/// </summary>
	public void OnRadioClick()
	{
        radioCanvas.SetActive(true);
        radioModel.ActivateRadio();
        inventoryPanel.SetActive(false);
        pausePanel.SetActive(false);
        craftingPanel.gameObject.SetActive(false);
        optionButtonPanel.SetActive(false);
	}

	/// <summary>
	/// Returns to the game scene.
	/// </summary>
	public void OnResumeClick()
	{
		Game.Instance.PauseInstance.Resume ();
		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);

        // do the deactivate radio menu before deactivating radio canvas.
        if (radioCanvas.activeInHierarchy)
        {
            radioModel.DeactivateRadio();
        }
        radioCanvas.SetActive (false);
		craftingPanel.gameObject.SetActive (false);
		optionButtonPanel.SetActive(true);

		if(GuiInstanceManager.InventoryUiInstance != null && GuiInstanceManager.InventoryUiInstance.ItemsToDiscard.Count > 0)
		{
			ItemDiscarder discarder = new ItemDiscarder();
			discarder.DiscardItems(GuiInstanceManager.InventoryUiInstance.ItemsToDiscard);
		}
	}

	/// <summary>
	/// Loads the crafting panel.
	/// </summary>
	public void OnCraftingClick()
	{
        // do the deactivate radio menu before deactivating radio canvas.
        if (radioCanvas.activeInHierarchy)
        {
            radioModel.DeactivateRadio();
        }
        radioCanvas.SetActive (false); 
		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);
		craftingPanel.gameObject.SetActive (true);
		craftingPanel.ResetPanel();
	}
}
