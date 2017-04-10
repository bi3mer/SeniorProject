using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class EquipableCategoryTests
{
	private BaseItem createTestItem()
	{
		BaseItem equipableItem = new BaseItem("Sample Equipable");
		equipableItem.FlavorText = "This is a test equipable";
		equipableItem.InventorySprite = "equipable.png";
		equipableItem.WorldModel = "equipableModel.png";
		equipableItem.Types = new List<string>();
		equipableItem.Types.Add(ItemTypes.Equipable);

		EquipableCategory equipable = new EquipableCategory ();
		equipable.Equiped = 0f;

		equipableItem.AddItemCategory(equipable);

		return equipableItem;
	}

	[Test]
	public void EquipTest()
	{
		// Arrange
		BaseItem equipableItem = createTestItem();

		EquipableCategory equipableCategory = (EquipableCategory) equipableItem.GetItemCategoryByClass(typeof(EquipableCategory));

		equipableCategory.Equip();

		// Add assertions here

		equipableCategory.UnEquip();

		// Add assertions for cloth taken off here
	}
}
