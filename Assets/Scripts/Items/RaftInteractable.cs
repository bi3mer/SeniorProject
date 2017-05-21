using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class RaftInteractable : InteractableObject {
    [SerializeField]
    private RaftMovement raftMovement;

    /// <summary>
    /// Gets or sets the raft category object.
    /// </summary>
    /// <value>The raft.</value>
    public RaftCategory Raft
    {
        get;
        set;
    }

	/// <summary>
	/// Gets or sets the attached inventory.
	/// </summary>
	/// <value>The attached inventory.</value>
	public Inventory AttachedInventory
	{
		get;
		set;
	}

	private bool raftBoarded = false;

	private const string useRaftText = "Use Raft (F)";

	private const string disembarkRaftText = "Disembark Raft";
	private const string boardRaftText = "Board Raft";
	private const string checkInventoryText = "Check Inventory";
	private const string moorRaftText = "Moor Raft";
	private const string unmoorRaftText = "Unmoor Raft";

	private OverworldItemOptionSelection itemSelectionHandler;

	private List<string> moorRaftItemTypes;

	private WindMovement wind;

    /// <summary>
    /// Sets action for raft as board raft
    /// </summary>
    public override void SetUp()
    {
    	// do not run set up unless Raft is set
    	if(Raft != null)
    	{
	        base.SetUp();

	        raftMovement = GetComponentInChildren<RaftMovement>();
	        raftMovement.SetMaxSpeed(Raft.Speed);
	        Text = useRaftText;

			moorRaftItemTypes = new List<string> {ItemTypes.Rope};
			wind = GetComponent<WindMovement>();

			if(AttachedInventory != null)
			{
				itemSelectionHandler = new OverworldItemOptionSelection(true);
			}

	        SetAction
	        (
	            delegate
	            {
	               HandleAction();
	            }
	        );
	    }
    }

    public void HandleAction()
    {
    	if(AttachedInventory == null)
    	{
    		HandleBoarding();
    	}
    	else
    	{
			itemSelectionHandler.Reset();

    		if(raftBoarded)
    		{
				itemSelectionHandler.AddPossibleAction(new ItemAction(disembarkRaftText, DisembarkRaft));
    		}
    		else
    		{
				itemSelectionHandler.AddPossibleAction(new ItemAction(boardRaftText, BoardRaft));
    		}

			itemSelectionHandler.AddPossibleAction(new ItemAction(checkInventoryText, openInventoryTransferPanel));

			if(raftMovement.RaftMoored)
			{
				itemSelectionHandler.AddPossibleAction(new ItemAction(unmoorRaftText, UnmoorRaft));
			}
			else
			{

				ItemAction action = new ItemAction(moorRaftText, MoorRaft);
				action.TypeUsed = moorRaftItemTypes;

				itemSelectionHandler.AddPossibleAction(action);
			}

			itemSelectionHandler.ShowPossibleActions();
    	}
    }

    /// <summary>
    /// Handles whether the player is boarding or disembarking the raft..
    /// </summary>
    public void HandleBoarding()
    {
		if(raftBoarded)
		{
			DisembarkRaft();
		}
		else
		{
			BoardRaft();
		}
    }

    /// <summary>
    /// Player boards the raft and assumes raft controls until the player dismounts.
    /// </summary>
    public void BoardRaft()
    {
        Game.Instance.PlayerInstance.Controller.BoardRaft(raftMovement);

        // Give the raft the player's animator to control.
        raftMovement.PlayerAnimator = Game.Instance.PlayerInstance.Controller.PlayerAnimator;
		raftBoarded = true;

        // Notify subscribers
        Game.Instance.EventManager.RaftBoarded();
    }

    /// <summary>
    /// Player disembarks the raft and resumes player movement.
    /// </summary>
    /// <param name="raftMovement"></param>
    public void DisembarkRaft()
    {
        Game.Instance.PlayerInstance.Controller.DisembarkRaft(raftMovement);
        raftBoarded = false;
    }

    /// <summary>
    /// Moors the raft. Wind and player inputs no longer move the raft.
    /// </summary>
    public void MoorRaft()
    {
		BaseItem item = Game.Instance.PlayerInstance.Inventory.GetInventoryBaseItem(itemSelectionHandler.SelectedItem);
		wind.enabled = false;
		raftMovement.RaftMoored = true;

		Game.Instance.PlayerInstance.Inventory.UseItem(item.ItemName, 1);
    }

    /// <summary>
    /// Unmoors the raft, allowing wind and player inputs to control the raft.
    /// </summary>
    public void UnmoorRaft()
    {
    	wind.enabled = true;
    	raftMovement.RaftMoored = false;
    }

	/// <summary>
	/// Picks up the item and adds it to the inventory. The Item is then removed from the world.
	/// </summary>
	private void openInventoryTransferPanel()
	{
		Game.Instance.GameViewInstance.OnInventoryTransferOpen();
		GuiInstanceManager.InventoryTransferInstance.LoadContainerInventory(AttachedInventory);
	}
}
