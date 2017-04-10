using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class PlantCategoryTests
{
	private BaseItem createTestItem()
	{
		BaseItem plantItem = new BaseItem("Sample Plant");
		plantItem.FlavorText = "This is a test plant";
		plantItem.InventorySprite = "plant.png";
		plantItem.WorldModel = "plantModel.png";
		plantItem.Types = new List<string>();
		plantItem.Types.Add(ItemTypes.Edible);

		PlantCategory plant = new PlantCategory ();
		plant.PneumoniaEffect = 0.1f;
		plant.StomachEffect = 0.2f;
		plant.Toughness = 0.3f;
		plant.WaterContent = 0.4f;

		plantItem.AddItemCategory(plant);

		return plantItem;
	}

	[Test]
	public void CookTest()
	{
		// Arrange
		BaseItem plantDry = createTestItem();
		BaseItem plantWater = new BaseItem(plantDry);

		PlantCategory dryPlantCategory = (PlantCategory) plantDry.GetItemCategoryByClass(typeof(PlantCategory));
		PlantCategory waterPlantCategory = (PlantCategory) plantWater.GetItemCategoryByClass(typeof(PlantCategory));
		waterPlantCategory.WaterContent = 5f;

		float expectedToughness = dryPlantCategory.Toughness * 0.5f;
		float expectedDryWaterContent = dryPlantCategory.WaterContent * 1.2f;
		float expectedWaterWaterContent = waterPlantCategory.WaterContent * 1.2f;

		// Act
		dryPlantCategory.Cook();
		waterPlantCategory.Cook();

		// Check
		Assert.AreEqual(expectedToughness, dryPlantCategory.Toughness);
		Assert.AreEqual(expectedDryWaterContent, dryPlantCategory.WaterContent);
		Assert.AreEqual("Cooked Sample Plant", plantDry.ItemName);

		Assert.AreEqual(expectedToughness, waterPlantCategory.Toughness);
		Assert.AreEqual(expectedWaterWaterContent, waterPlantCategory.WaterContent);
		Assert.AreEqual("Sample Plant Soup", plantWater.ItemName);
	}

	[Test]
	public void DryTest()
	{
		// Arrange
		BaseItem plant = createTestItem();
		PlantCategory plantCategory = (PlantCategory) plant.GetItemCategoryByClass(typeof(PlantCategory));

		float expectedWaterContent = plantCategory.WaterContent * 0.25f;
		float expectedToughness = plantCategory.Toughness * 1.5f;

		// Act
		plantCategory.Dry();

		// Check
		Assert.AreEqual(expectedToughness, plantCategory.Toughness);
		Assert.AreEqual(expectedWaterContent, plantCategory.WaterContent);
		Assert.AreEqual("Dried Sample Plant", plant.ItemName);
	}

	//[Test]
	public void EatTest()
	{
		BaseItem neutralPlant = createTestItem();
		BaseItem pneumoniaGivingPlant = createTestItem();
		BaseItem pneumoniaCuringPlant = createTestItem();
		BaseItem foodPoisoningGivingPlant = createTestItem();
		BaseItem foodPoisoningCuringPlant = createTestItem();

		PlantCategory neutralPlantCategory = (PlantCategory) neutralPlant.GetItemCategoryByClass(typeof(PlantCategory));
		PlantCategory pneumoniaGivingPlantCategory = (PlantCategory) pneumoniaGivingPlant.GetItemCategoryByClass(typeof(PlantCategory));
		PlantCategory pneumoniaCuringPlantCategory = (PlantCategory) pneumoniaCuringPlant.GetItemCategoryByClass(typeof(PlantCategory));
		PlantCategory foodPoisoningGivingPlantCategory = (PlantCategory) foodPoisoningGivingPlant.GetItemCategoryByClass(typeof(PlantCategory));
		PlantCategory foodPoisoningCuringPlantCategory = (PlantCategory) foodPoisoningCuringPlant.GetItemCategoryByClass(typeof(PlantCategory));

		neutralPlantCategory.StomachEffect = 0f;
		neutralPlantCategory.PneumoniaEffect = 0f;

		pneumoniaCuringPlantCategory.PneumoniaEffect = 0.1f;
		pneumoniaCuringPlantCategory.StomachEffect = 0f;

		pneumoniaGivingPlantCategory.PneumoniaEffect = -0.1f;
		pneumoniaGivingPlantCategory.StomachEffect = 0f;

		foodPoisoningCuringPlantCategory.PneumoniaEffect = 0f;
		foodPoisoningCuringPlantCategory.StomachEffect = 0.1f;

		foodPoisoningGivingPlantCategory.PneumoniaEffect = 0f;
		foodPoisoningGivingPlantCategory.StomachEffect = 0.1f;

		//Act
		neutralPlantCategory.Eat();
		Assert.AreEqual(PlayerHealthStatus.None, Game.Player.HealthStatus);

		pneumoniaGivingPlantCategory.Eat();
		Assert.AreEqual(PlayerHealthStatus.Pneumonia, Game.Player.HealthStatus);

		pneumoniaCuringPlantCategory.Eat();
		Assert.AreEqual(PlayerHealthStatus.None, Game.Player.HealthStatus);

		foodPoisoningGivingPlantCategory.Eat();
		Assert.AreEqual(PlayerHealthStatus.FoodPoisoning, Game.Player.HealthStatus);

		foodPoisoningCuringPlantCategory.Eat();
		Assert.AreEqual(PlayerHealthStatus.None, Game.Player.HealthStatus);
	}
}
