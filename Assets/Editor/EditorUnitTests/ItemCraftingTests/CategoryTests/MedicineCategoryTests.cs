using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class MedicineCategoryTests
{
	private BaseItem createTestItem(string illness)
	{
		PlayerInventory mockPlayerInventory = new PlayerInventory("player", 20);
		PlayerController controller = new GameObject().AddComponent<PlayerController>();
		controller.PlayerStatManager = new PlayerStatManager();

		Game.Instance.PlayerInstance = new Player(mockPlayerInventory);
		Game.Instance.PlayerInstance.Controller = controller;

		BaseItem medicineItem = new BaseItem("Sample Medicine");
		medicineItem.FlavorText = "This is a test medicine";
		medicineItem.InventorySprite = "medicine.png";
		medicineItem.WorldModel = "medicineModel.png";
		medicineItem.Types = new List<string>();
		medicineItem.Types.Add(ItemTypes.Medicinal);

		MedicineCategory medicine = new MedicineCategory ();
		medicine.HealthGain = 5f;
		medicine.Sickness = illness;

		medicineItem.AddItemCategory(medicine);

		return medicineItem;
	}

	[Test]
	public void HealTest()
	{
		// Arrange
		BaseItem medicineItem = createTestItem("");
		Game.Instance.PlayerInstance.Health = 50;

		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));
		medicineCategory.Heal();

		Assert.AreEqual(Game.Instance.PlayerInstance.Health, 55);

		Game.Instance.PlayerInstance.Health = 96;

		medicineCategory.Heal();
		Assert.AreEqual(Game.Instance.PlayerInstance.Health, 100);
	}

	[Test]
	public void CureAllTest()
	{
		BaseItem medicineItem = createTestItem("all");
		Game.Instance.PlayerInstance.HealthStatus = PlayerHealthStatus.FoodPoisoning;
		Game.Instance.PlayerInstance.Health = 50;

		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));

		//Act
		medicineCategory.CureAll();

		Assert.AreEqual(Game.Instance.PlayerInstance.Health, 100);
		Assert.AreEqual(Game.Instance.PlayerInstance.HealthStatus, PlayerHealthStatus.None);
	}

	[Test]
	public void CurePneumoniaTest()
	{
		BaseItem medicineItem = createTestItem("pneumonia");
		Game.Instance.PlayerInstance.HealthStatus = PlayerHealthStatus.Pneumonia;
		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));

		//Act
		medicineCategory.CurePneumonia();

		Assert.AreEqual(Game.Instance.PlayerInstance.HealthStatus, PlayerHealthStatus.None);
	}

	[Test]
	public void CureFoodPoisoningTest()
	{
		BaseItem medicineItem = createTestItem("food poisoning");
		Game.Instance.PlayerInstance.HealthStatus = PlayerHealthStatus.FoodPoisoning;
		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));

		//Act
		medicineCategory.CureFoodPoisoning();
		Assert.AreEqual(Game.Instance.PlayerInstance.HealthStatus, PlayerHealthStatus.None);
	}
}
