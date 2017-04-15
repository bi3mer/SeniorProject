using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class FireInteractable : InteractableObject, HasSelectionInterface
{
	/// <summary>
	/// Gets or sets the fire base category object.
	/// </summary>
	/// <value>The fire base.</value>
	public FireBaseCategory FireBase
	{
		get;
		set;
	}

	/// <summary>
	/// It's lit. Or is it?
	/// </summary>
	private bool lit;

	private ParticleSystem fireParticles;
	private GameObject optionsCanvas;
	private GameObject itemOptionButton;

	private const float maxLitTime = 60f;
	private const int maxParticlesAmount = 100;

	private const string lightFirePrompt = "Light Fire";
	private const string fuelFirePrompt = "Add Fuel";

	private const string burnTimeAttrName = "burnTime"; 

	private const string playerTag = "Player";

	private const float fireUpdateWaitTime = 0.2f;

	/// <summary>
	/// Sets openInventory as an action that should fire off when PerformAction is called.
	/// </summary>
	public override void SetUp()
	{
		base.SetUp();
		lit = false;
		fireParticles = GetComponentInChildren<ParticleSystem>();
		fireParticles.gameObject.SetActive(false);
		Text = lightFirePrompt;

		SetAction
		(
			delegate 
			{ 
				UseFirePit(); 
			}
		);
	}

	/// <summary>
	/// Interact with the fire pit.
	/// </summary>
	public void UseFirePit()
	{
		if(lit)
		{
			GuiInstanceManager.WorldSelectionGuiInstance.DisplayItemOptions(ItemTypes.Fuel, this);
		}
		else
		{
			GuiInstanceManager.WorldSelectionGuiInstance.DisplayItemOptions(ItemTypes.Igniter, this);
		}
	}

	/// <summary>
	/// Updates the fire.
	/// </summary>
	/// <returns>The fire.</returns>
	public IEnumerator UpdateFire()
	{
		float fireTimeLeft = 0f;

		while(lit)
		{
			fireTimeLeft = FireBase.CalculateRemainingFuel();

			if(fireTimeLeft > 0)
			{
				fireParticles.maxParticles = Mathf.Clamp(Mathf.CeilToInt(maxParticlesAmount * fireTimeLeft/maxLitTime), 0, maxParticlesAmount);
			}
			else
			{
				fireParticles.gameObject.SetActive(false);
				lit = false;
				Game.Instance.PlayerInstance.Controller.IsByFire = false;
				Text = lightFirePrompt;
			}

			yield return new WaitForSeconds(fireUpdateWaitTime);
		}
	}

	/// <summary>
	/// Add fuel to the fire.
	/// </summary>
	/// <param name="fuelName">Fuel name.</param>
	public void AddFuel(string fuelName)
	{
		BaseItem item = Game.Instance.PlayerInstance.Inventory.GetInventoryBaseItem(fuelName);
		FireBase.AddFuel(item.GetItemAttribute(burnTimeAttrName).Value);

		// For now, you can only add fuel one at a time
		Game.Instance.PlayerInstance.Inventory.UseItem(item.ItemName, 1);
	}

	/// <summary>
	/// Ignites the fire.
	/// </summary>
	/// <param name="igniterName">Igniter name.</param>
	public void IgniteFire(string igniterName)
	{
		lit = true;
		Text = fuelFirePrompt;

		// TODO: In the future, certain fire bases may need more ignition to ignite
		Game.Instance.PlayerInstance.Inventory.UseItem(igniterName, 1);
		fireParticles.gameObject.SetActive(true);

		Game.Instance.PlayerInstance.Controller.IsByFire = true;
		StartCoroutine(UpdateFire());
	}

	/// <summary>
	/// Callback action that will be fired off when an item has been selected.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public void ItemSelectedCallbackAction(string itemName)
	{
		if(lit)
		{
			AddFuel(itemName);
		}
		else
		{
			IgniteFire(itemName);
		}
	}

	/// <summary>
    /// When next to fire set IsByFire bool to true.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (lit && other.CompareTag(playerTag))
        {
            Game.Instance.PlayerInstance.Controller.IsByFire = true;
            // TO DO: Add variable for player to know fire is close
        }
    }

    /// <summary>
    /// When not next to fire set IsByFire bool to false.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (lit && other.CompareTag(playerTag))
        {
            Game.Instance.PlayerInstance.Controller.IsByFire = false;
            // TO DO: Change variable for player to know what fire is not close
        }
    }
}
