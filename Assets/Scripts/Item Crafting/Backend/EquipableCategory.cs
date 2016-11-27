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

	protected const string equipedAttrName = "equiped";

	/// <summary>
	/// Equip this item.
	/// </summary>
	public void Equip()
	{
		changePlayerEquiped(toolType, 1);
	}

	/// <summary>
	/// Unequip this item.
	/// </summary>
	public void UnEquip()
	{
		changePlayerEquiped("", 0);
	}

	/// <summary>
	/// Changes whether or not the item is the 
	/// </summary>
	/// <param name="equipString">Equip string.</param>
	private void changePlayerEquiped(string equipString, float equipedVal)
	{
		Game.Instance.PlayerInstance.EquippedTool = equipString;
		Equiped = equipedVal;
		GetAttribute(equipedAttrName).Value = Equiped;
		baseItem.DirtyFlag = true;
	}
}
