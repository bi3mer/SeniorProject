public abstract class EquipableCategory : ItemCategory 
{
	/// <summary>
	/// Gets or sets the equiped state. 1 is equiped, 0 is unequiped.
	/// TODO: When there are multiple equipables, when a new item is equiped, the pre-equiped item must have this set to 0f
	/// </summary>
	/// <value>The equiped.</value>
	public float Equiped
	{
		get;
		set;
	}

	protected string toolType;
	protected const string equipActionName = "Equip";
	protected const string unequipActionName = "Unequip";
    protected const string equipedAttributeName = "equiped";

	/// <summary>
	/// Equip this item.
	/// </summary>
	public void Equip()
	{
        Game.Instance.PlayerInstance.Inventory.EquipedItem = baseItem;
        Equiped = 1f;
		GetAttribute(equipedAttributeName).Value = Equiped;
		baseItem.UpdateExistingFlag = true;
	}

	/// <summary>
	/// Unequip this item.
	/// </summary>
	public void UnEquip()
	{
        if (Game.Instance.PlayerInstance.Inventory.EquipedItem.ItemName.Equals(baseItem.ItemName))
        {
            Game.Instance.PlayerInstance.Inventory.EquipedItem = null;
            Equiped = 0f;
			GetAttribute(equipedAttributeName).Value = Equiped;
			baseItem.UpdateExistingFlag = true;
        }
	}
}
