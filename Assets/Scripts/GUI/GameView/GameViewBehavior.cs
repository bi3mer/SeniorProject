using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameViewBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject inventoryPanel;

	[SerializeField]
	private GameObject inventoryTransferPanel;

	[SerializeField]
	private GameObject pausePanel;

	[SerializeField]
	private GameObject radioCanvas;

    [SerializeField]
    private InteractableRadioModel radioModelAnimation;

	[SerializeField]
	private RecipeBookBehavior craftingPanel;

	[SerializeField]
	private GameObject optionButtonPanel;

	[SerializeField]
	private GameObject settingsPanel;

	private ControlScheme controlScheme;

	/// <summary>
	/// Start the Game View with all other menu panels disabled.
	/// </summary>
	void Start () 
	{
		controlScheme = Game.Instance.Scheme;
		inventoryPanel.SetActive (false);
		inventoryTransferPanel.SetActive(false);

		pausePanel.SetActive (false);

		radioModelAnimation.SetUpRadioCanvas();
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
				if (inventoryPanel.gameObject.activeInHierarchy) 
				{
					OnResumeClick ();
				} 
				else 
				{
					OnInventoryClick ();
				}
			}

			if (Input.GetKeyDown(controlScheme.Radio)) 
			{
                if (radioCanvas.activeInHierarchy)
                {
                    radioModelAnimation.DeactivateRadio();
                }
                else
                {
                    OnRadioClick();
                }
            }

            if (Input.GetKeyDown(controlScheme.Crafting)) 
			{
				if (craftingPanel.gameObject.activeInHierarchy) 
				{
					OnResumeClick ();
				} 
				else 
				{
					OnCraftingClick ();
				}
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
		inventoryTransferPanel.SetActive(false);
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

		Game.Instance.PauseInstance.MenuPause ();

		if (GuiInstanceManager.InventoryUiInstance != null) 
		{
			if(GuiInstanceManager.InventoryUiInstance.AssociatedInventory != null)
			{
				GuiInstanceManager.InventoryUiInstance.RefreshInventoryPanel();
			}
		}
	}

	/// <summary>
	/// Raises the inventory open event.
	/// </summary>
	public void OnInventoryOpen()
	{
		inventoryPanel.SetActive (true);
		pausePanel.SetActive (false);

        // do the deactivate radio menu before deactivating radio Model.
        if (radioCanvas.activeInHierarchy)
        {
            radioModelAnimation.DeactivateRadio();
        }

		inventoryTransferPanel.SetActive(false);
		craftingPanel.gameObject.SetActive (false);
		optionButtonPanel.SetActive(false);
	}

	/// <summary>
	/// Raises the inventory transfer open event.
	/// </summary>
	public void OnInventoryTransferOpen()
	{
		inventoryTransferPanel.SetActive(true);

		inventoryPanel.SetActive (false);
		pausePanel.SetActive (false);

        // do the deactivate radio menu before deactivating radio Model.
        if (radioCanvas.activeInHierarchy)
        {
            radioModelAnimation.DeactivateRadio();
        }
       
		craftingPanel.gameObject.SetActive (false);
		optionButtonPanel.SetActive(false);

		Game.Instance.PauseInstance.MenuPause ();

		if (GuiInstanceManager.InventoryTransferInstance != null) 
		{
			GuiInstanceManager.InventoryTransferInstance.DisplayInventories();
		}
	}

	/// <summary>
	/// Loads the radio panel.
	/// </summary>
	public void OnRadioClick()
	{
		Game.Instance.PauseInstance.MenuPause ();
        radioModelAnimation.ActivateRadio();
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
		if (Game.Instance.PauseInstance.IsPaused) 
		{
			Game.Instance.PauseInstance.Resume ();
		}

		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);
		inventoryTransferPanel.SetActive(false);
        craftingPanel.gameObject.SetActive(false);
        settingsPanel.SetActive(false);
		optionButtonPanel.SetActive(true);


		if(GuiInstanceManager.InventoryUiInstance != null &&GuiInstanceManager.InventoryUiInstance.ItemsToDiscard != null 
		   && GuiInstanceManager.InventoryUiInstance.ItemsToDiscard.Count > 0)
		{
			ItemDiscarder discarder = new ItemDiscarder ();
			discarder.DiscardItems (GuiInstanceManager.InventoryUiInstance.ItemsToDiscard);
		}
	}

	/// <summary>
	/// Loads the crafting panel.
	/// </summary>
	public void OnCraftingClick()
	{
		Game.Instance.PauseInstance.MenuPause ();

        // do the deactivate radio menu if radio is active.
        if (radioCanvas.activeInHierarchy)
        {
            radioModelAnimation.DeactivateRadio();
        }

		inventoryPanel.SetActive (false);
		inventoryTransferPanel.SetActive(false);
		pausePanel.SetActive (false);
		craftingPanel.gameObject.SetActive (true);
		craftingPanel.ResetPanel();
	}

	/// <summary>
	/// Raises the save click event.
	/// </summary>
	public void OnSaveClick()
	{
		Debug.LogError("Not yet implemented");
	}

	/// <summary>
	/// Loads the settings panel.
	/// </summary>
	public void OnSettingsClick ()
	{
		settingsPanel.SetActive (true);
	}
}
