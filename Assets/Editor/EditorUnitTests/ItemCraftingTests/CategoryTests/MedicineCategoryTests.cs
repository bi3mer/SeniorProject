using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class MedicineCategoryTests
{
	private BaseItem createTestItem(string illness)
	{
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

		MedicineCategory medicineCategory = (MedicineCategory) medicineItem.GetItemCategoryByClass(typeof(MedicineCategory));
		medicineCategory.Heal();
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
