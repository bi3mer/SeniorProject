using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class ClothCategoryTests
{
	private BaseItem createTestItem()
	{
		BaseItem clothItem = new BaseItem("Sample Cloth");
		clothItem.FlavorText = "This is a test cloth";
		clothItem.InventorySprite = "cloth.png";
		clothItem.WorldModel = "clothModel.png";
		clothItem.Types = new List<string>();
		clothItem.Types.Add(ItemTypes.Cloth);

		ClothCategory cloth = new ClothCategory ();
		cloth.FabricThickness = 0.5f;
		cloth.Impermiability = 1f;
		cloth.ThreadDensity = 0.3f;
		cloth.OnPlayer = 0f;

		clothItem.AddItemCategory(cloth);

		return clothItem;
	}

	[Test]
	public void PutOnTest()
	{
		// Arrange
		BaseItem clothItem = createTestItem();

		ClothCategory clothCategory = (ClothCategory) clothItem.GetItemCategoryByClass(typeof(ClothCategory));

		float expectedClothWarmthRate = clothCategory.FabricThickness + clothCategory.ThreadDensity;
		clothCategory.PutOn();

		// Add assertions here

		clothCategory.TakeOff();

		// Add assertions for cloth taken off here
	}
}
