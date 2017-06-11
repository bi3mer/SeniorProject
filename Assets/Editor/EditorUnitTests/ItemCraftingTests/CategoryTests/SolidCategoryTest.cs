using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class SolidCategoryTest
{
	private BaseItem createTestItem()
	{
		BaseItem solidItem = new BaseItem("Sample Solid");
		solidItem.FlavorText = "This is a test solid";
		solidItem.InventorySprite = "solid.png";
		solidItem.WorldModel = "solid.png";
		solidItem.Types = new List<string>();
		solidItem.Types.Add(ItemTypes.BaseSolid);
		solidItem.Types.Add(ItemTypes.Rod);

		SolidCategory solid = new SolidCategory ();
		solid.Durability = 0.1f;
		solid.Elasticity = 0.2f;
		solid.Flexibility = 0.3f;
		solid.Sharpness = 0.4f;
		solid.Stickiness = 0.5f;
		solid.Thickness = 0.6f;

		solidItem.AddItemCategory(solid);

		solidItem.ActionModifiedModels = new List<string>();
		solidItem.ActionModifiedSprites = new List<string>();
		solidItem.ModifyingActionNames = new List<string>();

		solidItem.ActionModifiedModels.Add("basket.png");
		solidItem.ActionModifiedModels.Add("rope.png");

		solidItem.ActionModifiedSprites.Add("basketSprite.png");
		solidItem.ActionModifiedSprites.Add("ropeSprite.png");

		solidItem.ModifyingActionNames.Add("Weave Basket");
		solidItem.ModifyingActionNames.Add("Weave Rope");

		return solidItem;
	}

	[Test]
	public void WeaveRopeTest()
	{
		// Arrange
		BaseItem thinSolid = createTestItem();
		BaseItem thickSolid = new BaseItem(thinSolid);

		SolidCategory thinSolidCategory = (SolidCategory) thinSolid.GetItemCategoryByClass(typeof(SolidCategory));
		SolidCategory thickSolidCategory = (SolidCategory) thickSolid.GetItemCategoryByClass(typeof(SolidCategory));
		thickSolidCategory.Thickness = 1.5f;

		float expectedDurability = thinSolidCategory.Durability * thinSolidCategory.Elasticity / 2f;
		float expectedThinThickness = thinSolidCategory.Thickness * 4f;
		float expectedThickThickness = thickSolidCategory.Thickness * 4f;

		// Act
		thinSolidCategory.WeaveRope();
		thickSolidCategory.WeaveRope();

		// Check
		Assert.AreEqual(expectedDurability, thinSolidCategory.Durability);
		Assert.AreEqual(expectedThinThickness, thinSolidCategory.Thickness);
		Assert.AreEqual("Sample Solid Thread", thinSolid.ItemName);
		Assert.IsTrue(thinSolid.Types.Contains(ItemTypes.Rope));
		Assert.AreEqual("rope.png", thinSolid.WorldModel);
		Assert.AreEqual("ropeSprite.png", thinSolid.InventorySprite);

		Assert.AreEqual(expectedDurability, thickSolidCategory.Durability);
		Assert.AreEqual(expectedThickThickness, thickSolidCategory.Thickness);
		Assert.AreEqual("Sample Solid Rope", thickSolid.ItemName);
		Assert.IsTrue(thinSolid.Types.Contains(ItemTypes.Rope));
		Assert.AreEqual("rope.png", thinSolid.WorldModel);
		Assert.AreEqual("ropeSprite.png", thinSolid.InventorySprite);
	}

	[Test]
	public void WeaveBasketTest()
	{
		// Arrange
		BaseItem solid = createTestItem();

		SolidCategory solidCategory = (SolidCategory) solid.GetItemCategoryByClass(typeof(SolidCategory));
		float expectedDurability = (solidCategory.Durability + solidCategory.Elasticity) / 2f;
		float expectedThickness = solidCategory.Thickness * 2f;

		// Act
		solidCategory.WeaveBasket();

		// Check
		Assert.AreEqual(expectedDurability, solidCategory.Durability);
		Assert.AreEqual(expectedThickness, solidCategory.Thickness);
		Assert.AreEqual("Sample Solid Basket", solid.ItemName);
		Assert.IsTrue(solid.Types.Contains(ItemTypes.Container));
		Assert.AreEqual("basket.png", solid.WorldModel);
		Assert.AreEqual("basketSprite.png", solid.InventorySprite);
	}

	[Test]
	public void SharpenTest()
	{
		BaseItem dullSolid = createTestItem();
		SolidCategory dullSolidCategory = (SolidCategory) dullSolid.GetItemCategoryByClass(typeof(SolidCategory));

		BaseItem sharpSolid = new BaseItem(dullSolid);
		SolidCategory sharpSolidCategory = (SolidCategory) sharpSolid.GetItemCategoryByClass(typeof(SolidCategory));
		sharpSolidCategory.Sharpness = 3f;

		float expectedDullSharpness = dullSolidCategory.Sharpness * 1.5f + dullSolidCategory.Durability;

		float expectedSharpSharpness = sharpSolidCategory.Sharpness * 1.5f + sharpSolidCategory.Durability;
		//Act
		dullSolidCategory.Sharpen();
		sharpSolidCategory.Sharpen();

		//Check
		Assert.AreEqual(expectedDullSharpness, dullSolidCategory.Sharpness);
		Assert.AreEqual(expectedSharpSharpness, sharpSolidCategory.Sharpness);

		Assert.AreEqual("Filed Sample Solid", dullSolid.ItemName);
		Assert.AreEqual("Sharpened Sample Solid", sharpSolid.ItemName);

		Assert.IsFalse(dullSolid.Types.Contains(ItemTypes.Sharp));
		Assert.IsTrue(sharpSolid.Types.Contains(ItemTypes.Sharp));
	}
}
