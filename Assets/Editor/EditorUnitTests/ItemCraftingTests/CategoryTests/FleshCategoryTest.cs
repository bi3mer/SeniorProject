using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class FleshCategoryTest
{
	private BaseItem createTestItem()
	{
		PlayerInventory mockPlayerInventory = new PlayerInventory("player", 20);

		PlayerController controller = new GameObject().AddComponent<PlayerController>();
		controller.PlayerStatManager = new PlayerStatManager();
	
		Game.Instance.PlayerInstance = new Player(mockPlayerInventory);
		Game.Instance.PlayerInstance.Controller = controller;
		GuiInstanceManager.ItemAmountPanelInstance = new GameObject().AddComponent<ChooseItemAmountPanelBehavior>();
		GuiInstanceManager.ItemAmountPanelInstance.CurrentAmount = 1;

		BaseItem fleshItem = new BaseItem("Sample Flesh");
		fleshItem.FlavorText = "This is a test flesh";
		fleshItem.InventorySprite = "flesh.png";
		fleshItem.WorldModel = "fleshModel.png";
		fleshItem.Types = new List<string>();
		fleshItem.Types.Add(ItemTypes.Edible);
		fleshItem.ModifyingActionNames = new List<string>{"Cook"};
		fleshItem.ActionModifiedSprites = new List<string>{"modifiedFlesh.png"};
		fleshItem.ActionModifiedModels = new List<string>{"modifiedFleshModel.png"};

		FleshCategory flesh = new FleshCategory ();
		flesh.HealthEffect = 0.1f;
		flesh.HungerGain = 0.2f;

		fleshItem.AddItemCategory(flesh);

		return fleshItem;
	}

	[Test]
	public void CookTest()
	{
		// Arrange
		BaseItem fleshItem = createTestItem();

		FleshCategory fleshCategory = (FleshCategory) fleshItem.GetItemCategoryByClass(typeof(FleshCategory));

		float expectedHealthEffect1 = fleshCategory.HealthEffect + 0.25f;
		float expectedHungerGain1 = fleshCategory.HungerGain * 1.25f;
		float expectedHealthEffect2 = expectedHealthEffect1 - 0.25f;
		float expectedHungerGain2 = expectedHungerGain1 * 0.5f;

		// Act
		fleshCategory.Cook();

		// Check
		Assert.AreEqual(expectedHealthEffect1, fleshCategory.HealthEffect);
		Assert.AreEqual(expectedHungerGain1, fleshCategory.HungerGain);
		Assert.AreEqual("Cooked Sample Flesh", fleshItem.ItemName);
		Assert.True(fleshItem.Types.Contains(ItemTypes.Edible));
		Assert.AreEqual(1, fleshItem.Types.Count);
		Assert.AreEqual("modifiedFlesh.png", fleshItem.InventorySprite);
		Assert.AreEqual("modifiedFleshModel.png", fleshItem.WorldModel);

		// Check the second time it cooks that it becomes burnt
		fleshCategory.Cook();

		Assert.AreEqual(expectedHealthEffect2, fleshCategory.HealthEffect);
		Assert.AreEqual(expectedHungerGain2, fleshCategory.HungerGain);
		Assert.AreEqual("Burnt Sample Flesh", fleshItem.ItemName);
		Assert.True(fleshItem.Types.Contains(ItemTypes.Fuel));
		Assert.AreEqual(1, fleshItem.Types.Count);
	}

	[Test]
	public void EatTest()
	{
		BaseItem neutralFlesh = createTestItem();
		BaseItem spoiledFlesh = createTestItem();

		FleshCategory neutralFleshCategory = (FleshCategory) neutralFlesh.GetItemCategoryByClass(typeof(FleshCategory));
		FleshCategory spoiledFleshCategory = (FleshCategory) spoiledFlesh.GetItemCategoryByClass(typeof(FleshCategory));

		spoiledFleshCategory.HealthEffect = -0.1f;

		//Act
		neutralFleshCategory.Eat();
		spoiledFleshCategory.Eat();
	}
}
