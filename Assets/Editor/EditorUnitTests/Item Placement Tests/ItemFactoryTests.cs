using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]

public class ItemFactoryTests : MonoBehaviour 
{
	public ItemFactory createItemFactory()
	{
		return new ItemFactory();
	}

	public Inventory createInventory()
	{
		return new Inventory("Test Inventory", 5);
	}

	public Recipe TieredRecipe()
	{
		Recipe fishingRodRecipe = new Recipe();
		Requirement hook = new Requirement();
		hook.AmountRequired = 1;
		hook.ItemType = ItemTypes.Hook;

		Requirement rope = new Requirement();
		rope.AmountRequired = 1;
		rope.ItemType = ItemTypes.Rope;

		Requirement rod = new Requirement();
		rod.AmountRequired = 1;
		rod.ItemType = ItemTypes.Rod;

		fishingRodRecipe.ResourceRequirements = new List<Requirement>{hook, rod, rope};

		fishingRodRecipe.Tiered = true;
		fishingRodRecipe.ToolRequirements = null;
		fishingRodRecipe.RecipeName = "Fishing Rod";
		fishingRodRecipe.StatsToCheck = new List<CraftingStat>();

		CraftingStat durability = new CraftingStat();
		durability.StatName = "durability";
		durability.QualityThreshold = new List<float> {9, 13};
		durability.StatAffectingItems = new List<string>{ItemTypes.Hook, ItemTypes.Rod, ItemTypes.Rope};

		CraftingStat flexibility = new CraftingStat();
		flexibility.StatName = "flexibility";
		flexibility.QualityThreshold = new List<float> {0.5f, 1};
		flexibility.StatAffectingItems = new List<string>{ItemTypes.Rod, ItemTypes.Rope};

		fishingRodRecipe.StatsToCheck.Add(durability);
		fishingRodRecipe.StatsToCheck.Add(flexibility);

		return fishingRodRecipe;
	}

	public Recipe TieredDecreasingRecipeWithTool()
	{
		Recipe cloth = new Recipe();

		Requirement threadReq = new Requirement();
		threadReq.AmountRequired = 1;
		threadReq.ItemType = ItemTypes.Rope;

		Requirement knifeReq = new Requirement();
		knifeReq.AmountRequired = 1;
		knifeReq.ItemType = ItemTypes.Blade;

		cloth.ResourceRequirements = new List<Requirement>{threadReq};

		cloth.Tiered = true;
		cloth.ToolRequirements = new List<Requirement>{knifeReq};
		cloth.RecipeName = "Woven Blanket";
		cloth.StatsToCheck = new List<CraftingStat>();

		CraftingStat thickness = new CraftingStat();
		thickness.StatName = "thickness";
		thickness.QualityThreshold = new List<float> {0.8f, 0.2f};
		thickness.StatAffectingItems = new List<string>{ItemTypes.Rope};

		CraftingStat flexibility = new CraftingStat();
		flexibility.StatName = "flexibility";
		flexibility.QualityThreshold = new List<float> {3f, 4f};
		flexibility.StatAffectingItems = new List<string>{ItemTypes.Rope};

		cloth.StatsToCheck.Add(thickness);
		cloth.StatsToCheck.Add(flexibility);

		return cloth;
	}

	public Recipe UntieredTool()
	{
		Recipe knifeRecipe = new Recipe();

		Requirement threadReq = new Requirement();
		threadReq.AmountRequired = 1;
		threadReq.ItemType = ItemTypes.Rope;

		Requirement sharpReq = new Requirement();
		sharpReq.AmountRequired = 1;
		sharpReq.ItemType = ItemTypes.Sharp;

		knifeRecipe.ResourceRequirements = new List<Requirement>{threadReq, sharpReq};

		knifeRecipe.Tiered = false;
		knifeRecipe.ToolRequirements = null;
		knifeRecipe.RecipeName = "Rope Bound Knife";
		knifeRecipe.StatsToCheck = null;

		return knifeRecipe;
	}

	[Test]
	public void TestTieredCrafting()
	{
		Inventory targetInventory = createInventory();
		ItemFactory factory = new ItemFactory();

		BaseItem riverReedThread = factory.GetBaseItem("River Reed");
		((SolidCategory)riverReedThread.GetItemCategoryByClass(typeof(SolidCategory))).WeaveRope();

		BaseItem can = factory.GetBaseItem("Empty Can");
		BaseItem stick = factory.GetBaseItem("Stick");

		targetInventory.AddItem(can, 1);
		targetInventory.AddItem(stick, 1);
		targetInventory.AddItem(riverReedThread, 1);

		Recipe recipe = TieredRecipe();

		Ingredient threadIng = new Ingredient();
		threadIng.Amount = 1;
		threadIng.UseType = ItemTypes.Rope;
		threadIng.IngredientName = "River Reed Thread";
		threadIng.AssociatedStackId = targetInventory.GetStacks("River Reed Thread", 1)[0].Id;

		Ingredient canIng = new Ingredient();
		canIng.Amount = 1;
		canIng.UseType = ItemTypes.Hook;
		canIng.IngredientName = "Empty Can";
		canIng.AssociatedStackId = targetInventory.GetStacks("Empty Can", 1)[0].Id;

		Ingredient stickIng = new Ingredient();
		stickIng.Amount = 1;
		stickIng.UseType = ItemTypes.Rod;
		stickIng.IngredientName = "Stick";
		stickIng.AssociatedStackId = targetInventory.GetStacks("Stick", 1)[0].Id;

		List<Ingredient> ingredients = new List<Ingredient>{threadIng, canIng, stickIng};

		factory.Craft(recipe, ingredients, targetInventory);

		Assert.AreEqual(targetInventory.GetStacks("River Reed Thread", 1).Count, 0);
		Assert.AreEqual(targetInventory.GetStacks("Empty Can", 1).Count, 0);
		Assert.AreEqual(targetInventory.GetStacks("Stick", 1).Count, 0);

		int result = targetInventory.GetStacks("Standard Fishing Rod", 1).Count;
		Assert.AreEqual(result, 1);
	}

	[Test]
	public void TestTieredDecreaseCrafting()
	{
		Inventory targetInventory = createInventory();
		ItemFactory factory = new ItemFactory();

		BaseItem riverReedThread = factory.GetBaseItem("River Reed");
		((SolidCategory)riverReedThread.GetItemCategoryByClass(typeof(SolidCategory))).WeaveRope();

		BaseItem knife = factory.GetBaseItem("Rope Bound Knife");

		targetInventory.AddItem(riverReedThread, 1);
		targetInventory.AddItem(knife, 1);

		Recipe recipe = TieredDecreasingRecipeWithTool();

		Ingredient threadIng = new Ingredient();
		threadIng.Amount = 1;
		threadIng.UseType = ItemTypes.Rope;
		threadIng.IngredientName = "River Reed Thread";
		threadIng.AssociatedStackId = targetInventory.GetStacks("River Reed Thread", 1)[0].Id;

		Ingredient knifeIng = new Ingredient();
		knifeIng.Amount = 1;
		knifeIng.UseType = ItemTypes.Blade;
		knifeIng.IngredientName = "Rope Bound Knife";
		knifeIng.AssociatedStackId = targetInventory.GetStacks("Rope Bound Knife", 1)[0].Id;


		List<Ingredient> ingredients = new List<Ingredient>{threadIng};

		factory.Craft(recipe, ingredients, targetInventory);

		Assert.AreEqual(targetInventory.GetStacks("River Reed Thread", 1).Count, 0);
		Assert.AreEqual(targetInventory.GetStacks("Rope Bound Knife", 1).Count, 1);

		int result = targetInventory.GetStacks("Excellent Woven Blanket", 1).Count;
		Assert.AreEqual(result, 1);
	}

	[Test]
	public void unTieredTest()
	{
		Inventory targetInventory = createInventory();
		ItemFactory factory = new ItemFactory();

		BaseItem riverReedThread = factory.GetBaseItem("River Reed");
		((SolidCategory)riverReedThread.GetItemCategoryByClass(typeof(SolidCategory))).WeaveRope();

		BaseItem sharpObject = factory.GetBaseItem("Scrap Metal");
		((SolidCategory)sharpObject.GetItemCategoryByClass(typeof(SolidCategory))).Sharpen();

		targetInventory.AddItem(riverReedThread, 1);
		targetInventory.AddItem(sharpObject, 1);

		Recipe recipe = UntieredTool();

		Ingredient threadIng = new Ingredient();
		threadIng.Amount = 1;
		threadIng.UseType = ItemTypes.Rope;
		threadIng.IngredientName = "River Reed Thread";
		threadIng.AssociatedStackId = targetInventory.GetStacks("River Reed Thread", 1)[0].Id;

		Ingredient sharpIng = new Ingredient();
		sharpIng.Amount = 1;
		sharpIng.UseType = ItemTypes.Blade;
		sharpIng.IngredientName = "Sharpened Scrap Metal";
		sharpIng.AssociatedStackId = targetInventory.GetStacks("Sharpened Scrap Metal", 1)[0].Id;


		List<Ingredient> ingredients = new List<Ingredient>{threadIng, sharpIng};

		factory.Craft(recipe, ingredients, targetInventory);

		Assert.AreEqual(targetInventory.GetStacks("River Reed Thread", 1).Count, 0);
		Assert.AreEqual(targetInventory.GetStacks("Sharped Scrap Metal", 1).Count, 0);

		int result = targetInventory.GetStacks("Rope Bound Knife", 1).Count;
		Assert.AreEqual(result, 1);
	}
}
