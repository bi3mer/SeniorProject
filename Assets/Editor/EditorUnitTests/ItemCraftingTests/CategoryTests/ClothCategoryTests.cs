using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class ClothCategoryTests
{
	private BaseItem createTestItem()
	{
		PlayerInventory mockPlayerInventory = new PlayerInventory("player", 20);
		Player mockPlayer = new Player(mockPlayerInventory);
		BaseItem clothItem = new BaseItem("Sample Cloth", mockPlayer);
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

		clothCategory.PutOn();
		Assert.AreEqual(clothCategory.OnPlayer, 1f);

		clothCategory.TakeOff();
		Assert.AreEqual(clothCategory.OnPlayer, 0);
	}
}
