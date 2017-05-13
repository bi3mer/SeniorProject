using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FishingRodCategory : EquipableCategory
{
    /// <summary>
    /// Degenteration threshold that number of uses must reach before falling apart.
    /// </summary>
    public int DegenerationThreshold
    {
        get;
        set;
    }

    private const string degenerationAttributeName = "degenerationThreshold";
       
	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		FishingRodCategory category = new FishingRodCategory();
		category.Equiped = Equiped;
        category.DegenerationThreshold = DegenerationThreshold;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<ItemAttribute>();

		ItemAction equip = new ItemAction (equipActionName, new UnityAction(category.Equip));
		ItemAction unequip = new ItemAction (unequipActionName, new UnityAction(category.UnEquip));

		category.Actions.Add(equip);
		category.Actions.Add(unequip);

		finishDuplication(category);

		return category;
	}

	/// <summary>
	/// Preps the category for use by loading attributes and actions into lists.
	/// </summary>
	public override void ReadyCategory()
	{
		Attributes = new List<ItemAttribute> ();
		Attributes.Add(new ItemAttribute(equipedAttributeName, Equiped));
		Attributes.Add(new ItemAttribute(degenerationAttributeName, DegenerationThreshold));;

        Actions = new List<ItemAction> ();
		ItemAction equip = new ItemAction (equipActionName, new UnityAction(Equip));

		// the equiped attribute acts as a boolean, so the threshold is 1
		equip.Conditions.Add(new ItemCondition(equipedAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.Less)));

		ItemAction unequip = new ItemAction(unequipActionName, new UnityAction(UnEquip));
		unequip.Conditions.Add(new ItemCondition(equipedAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.GreaterOrEqual)));

		Actions.Add(equip);
		Actions.Add(unequip);
	}

    /// <summary>
    /// Breaks the fishing rod and gives back certain percentage of items back.
    /// </summary>
    public void Break()
    {
        // Show notification
        GuiInstanceManager.PlayerNotificationInstance.ShowNotification(NotificationType.BREAK);

        // Unequip rod
        Game.Instance.PlayerInstance.Inventory.EquipedItem = null;
        Equiped = 0f;
        GetAttribute(equipedAttributeName).Value = Equiped;

        // Remove rod from existance
        Game.Instance.PlayerInstance.Inventory.UseItem(baseItem.ItemName, 1);
    }
}
