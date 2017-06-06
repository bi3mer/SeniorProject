using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class LightCategoryTests
{
	private BaseItem createTestItem()
	{
		BaseItem lightItem = new BaseItem("Sample Light");
		lightItem.FlavorText = "This is a test light";
		lightItem.InventorySprite = "light.png";
		lightItem.WorldModel = "lightModel.png";
		lightItem.Types = new List<string>();
		lightItem.Types.Add(ItemTypes.Fuel);

		LightCategory light = new LightCategory ();
		light.Brightness = 2f;
		light.BurnRate = 0.75f;
		light.CurrentFuelLevel = 3f;
		light.MaxFuel = 5f;
	
		lightItem.AddItemCategory(light);

		return lightItem;
	}

	[Test]
	public void AddFuelTest()
	{
		// Arrange
		BaseItem lightItem = createTestItem();

		LightCategory lightCategory = (LightCategory) lightItem.GetItemCategoryByClass(typeof(LightCategory));

		float fuel = 1.5f;
		float expectedNewFuel = lightCategory.CurrentFuelLevel + fuel;
		float expectedNewFuel2 = lightCategory.MaxFuel;

		// Act
		lightCategory.AddFuel(fuel);
	
		// Assert
		Assert.AreEqual(expectedNewFuel, lightCategory.CurrentFuelLevel);

		lightCategory.AddFuel(fuel);

		Assert.AreEqual(expectedNewFuel2, lightCategory.CurrentFuelLevel);
	}
}
