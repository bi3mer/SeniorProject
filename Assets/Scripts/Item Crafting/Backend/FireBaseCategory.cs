using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class FireBaseCategory : ItemCategory
{
    /// <summary>
    /// Gets and sets the life the fire gains.
    /// </summary>
    public float BurnRateMultiplier
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the starting fuel.
    /// </summary>
    /// <value>The starting fuel.</value>
    public float StartingFuel
    {
    	get;
    	set;
    }

    /// <summary>
    /// Gets or sets the fuel remaining.
    /// </summary>
    /// <value>The fuel remaining.</value>
    public float FuelRemaining
    {
    	get;
    	set;
    }

    private const string burnRateMultiplierAttrName = "burnRateMultiplier";

    private const string setDownActName = "Set Down";

    private const string startingFuelAttrName = "startingFuel";

    private const string fuelRemainingAttrName = "fuelRemaining";

    /// <summary>
    /// Creates a copy of this fuel category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        FireBaseCategory category = new FireBaseCategory();

        category.BurnRateMultiplier = BurnRateMultiplier;
		category.Attributes = new List<ItemAttribute>();
        category.Actions = new List<ItemAction>();

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(category.SetDown));

        category.Actions.Add(setDown);

        category.StartingFuel = StartingFuel;
        category.FuelRemaining = FuelRemaining;

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<ItemAttribute>();
        Attributes.Add(new ItemAttribute(burnRateMultiplierAttrName, BurnRateMultiplier));
        Attributes.Add(new ItemAttribute(startingFuelAttrName, StartingFuel));
        Attributes.Add(new ItemAttribute(fuelRemainingAttrName, FuelRemaining));

        Actions = new List<ItemAction>();
        Actions.Add(new ItemAction(setDownActName, new UnityAction(SetDown)));

        FuelRemaining = StartingFuel;
    }

    /// <summary>
    /// Calculates remaining fuel int the fire. This is not an action that can be done in the inventory! This is only used in the world.
    /// </summary>
    /// <returns>The fire.</returns>
    public float CalculateRemainingFuel()
    {
		FuelRemaining -= Time.deltaTime * BurnRateMultiplier;

       	return FuelRemaining;
    }

    /// <summary>
    /// Adds fuel to the fire. Not an action that can be done in the inventory! This is only used in the world.
    /// </summary>
    /// <param name="burnTime">Burn time.</param>
    public void AddFuel(float burnTime)
    {
    	FuelRemaining += burnTime;
    }

	/// <summary>
	/// Sets down the fire pit in the world. Drops it where the player stands.
	/// </summary>
	public void SetDown()
	{
		// create the object with the model
		GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem);
		FireInteractable fireBase = item.AddComponent<FireInteractable>();

		fireBase.SetUp();
		fireBase.FireBase = this;

		item.name = baseItem.ItemName;
		item.transform.position = Game.Instance.PlayerInstance.WorldTransform.position;
		Game.Instance.ItemPoolInstance.AddItemFromWorld(item);

		SetActionComplete(setDownActName);

		baseItem.RemovalFlag = true;
	}
}
