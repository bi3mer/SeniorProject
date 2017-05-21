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
		Player mockPlayer = new Player(mockPlayerInventory);
		BaseItem medicineItem = new BaseItem("Sample Medicine", mockPlayer);
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
		medicineItem.GetMockPlayer().Health = 50;

		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));
		medicineCategory.Heal();

		Assert.AreEqual(medicineItem.GetMockPlayer().Health, 55);

		medicineItem.GetMockPlayer().Health = 96;

		medicineCategory.Heal();
		Assert.AreEqual(medicineItem.GetMockPlayer().Health, 100);
	}

	[Test]
	public void CureAllTest()
	{
		BaseItem medicineItem = createTestItem("all");
		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));

		//Act
		medicineCategory.CureAll();
	}

	[Test]
	public void CurePneumoniaTest()
	{
		BaseItem medicineItem = createTestItem("pneumonia");
		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));

		//Act
		medicineCategory.CurePneumonia();
	}

	[Test]
	public void CureFoodPoisoningTest()
	{
		BaseItem medicineItem = createTestItem("food poisoning");
		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));

		//Act
		medicineCategory.CureAll();
	}
}
