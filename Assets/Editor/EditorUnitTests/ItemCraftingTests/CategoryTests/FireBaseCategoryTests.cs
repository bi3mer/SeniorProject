using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class FireBaseCategoryTests
{
	private BaseItem createTestItem()
	{
		BaseItem fireItem = new BaseItem("Sample Fire");
		fireItem.FlavorText = "This is a test fire";
		fireItem.InventorySprite = "fire.png";
		fireItem.WorldModel = "fireModel.png";
		fireItem.Types = new List<string>();
		fireItem.Types.Add(ItemTypes.Fuel);

		FireBaseCategory fire = new FireBaseCategory ();
		fire.BurnRateMultiplier = 1f;
		fire.FuelRemaining = 10f;
		fire.StartingFuel = 10f;
	
		fireItem.AddItemCategory(fire);

		return fireItem;
	}

	[Test]
	public void AddFuelTest()
	{
		// Arrange
		BaseItem fireItem = createTestItem();

		FireBaseCategory fireCategory = (FireBaseCategory) fireItem.GetItemCategoryByClass(typeof(FireBaseCategory));

		float fuel = 5f;
		float expectedNewFire = fireCategory.FuelRemaining + fuel;

		// Act
		fireCategory.AddFuel(fuel);
	
		// Assert
		Assert.AreEqual(expectedNewFire, fireCategory.FuelRemaining);
	}
}
