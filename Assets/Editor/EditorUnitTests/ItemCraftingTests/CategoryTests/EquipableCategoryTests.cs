using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class EquipableCategoryTests
{
	private BaseItem createTestItem()
	{
		PlayerInventory mockPlayerInventory = new PlayerInventory("player", 20);
		Player mockPlayer = new Player(mockPlayerInventory);
		BaseItem equipableItem = new BaseItem("Sample Equipable", mockPlayer);
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

		Assert.AreEqual(equipableItem.GetMockPlayer().Inventory.EquipedItem, equipableItem);
		Assert.AreEqual(equipableItem.GetItemAttribute("equiped").Value, 1f);

		equipableCategory.UnEquip();

		Assert.AreEqual(equipableItem.GetMockPlayer().Inventory.EquipedItem, null);
		Assert.AreEqual(equipableItem.GetItemAttribute("equiped").Value, 0);
	}
}
