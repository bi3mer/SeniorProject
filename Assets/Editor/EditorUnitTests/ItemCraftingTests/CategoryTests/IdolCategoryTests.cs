using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class IdolCategoryTests
{
	private BaseItem createWarmthIdol()
	{
		BaseItem warmthIdolItem = new BaseItem("Sample Warmth Idol");
		warmthIdolItem.FlavorText = "This is a test warmth idol";
		warmthIdolItem.InventorySprite = "warmthIdol.png";
		warmthIdolItem.WorldModel = "warmthIdolModel.png";
		warmthIdolItem.Types = new List<string>();
		warmthIdolItem.Types.Add(ItemTypes.Equipable);

		WarmthIdolCategory warmthIdol = new WarmthIdolCategory ();
		warmthIdol.Equiped = 0f;
		warmthIdol.WarmthBenefit = 1;

		warmthIdolItem.AddItemCategory(warmthIdol);

		return warmthIdolItem;
	}

	[Test]
	public void EquipTest()
	{
		// Arrange
		BaseItem warmthIdolItem = createWarmthIdol();

		WarmthIdolCategory warmthIdolCategory = (WarmthIdolCategory) warmthIdolItem.GetItemCategoryByClass(typeof(WarmthIdolCategory));

		warmthIdolCategory.Equip();

		// Add assertions here

		warmthIdolCategory.UnEquip();

		// Add assertions for cloth taken off here
	}
}
