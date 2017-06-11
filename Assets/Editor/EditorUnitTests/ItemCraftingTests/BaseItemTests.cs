using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[TestFixture]
public class BaseItemTests
{
	private BaseItem createTestItem()
	{
		PlayerInventory mockPlayerInventory = new PlayerInventory("player", 20);
		PlayerController controller = new GameObject().AddComponent<PlayerController>();

		Game.Instance.PlayerInstance = new Player(mockPlayerInventory);
		Game.Instance.PlayerInstance.Controller = controller;
		BaseItem item = new BaseItem("Sample Item");
		item.FlavorText = "This is a test item";
		item.InventorySprite = "item.png";
		item.WorldModel = "itemWorld.png";
		item.Types = new List<string>();
		item.Types.Add(ItemTypes.BaseSolid);
		item.Types.Add(ItemTypes.Rod);

		SolidCategory solid = new SolidCategory ();
		solid.Durability = 0.1f;
		solid.Elasticity = 0.2f;
		solid.Flexibility = 0.3f;
		solid.Sharpness = 0.4f;
		solid.Stickiness = 0.5f;
		solid.Thickness = 0.6f;

		PlantCategory plant = new PlantCategory ();
		plant.PneumoniaEffect = 0.1f;
		plant.StomachEffect = 0.2f;
		plant.Toughness = 0.3f;
		plant.WaterContent = 0.4f;

		FleshCategory flesh = new FleshCategory ();
		flesh.HealthEffect = 0.1f;
		flesh.HungerGain = 0.2f;

		ContainerCategory container = new ContainerCategory();
		container.Size = 1;

		MedicineCategory medicine = new MedicineCategory ();
		medicine.HealthGain = 5f;
		medicine.Sickness = "all";

		ClothCategory cloth = new ClothCategory ();
		cloth.FabricThickness = 0.5f;
		cloth.Impermiability = 1f;
		cloth.ThreadDensity = 0.3f;
		cloth.OnPlayer = 0f;

		FuelCategory fuel = new FuelCategory();
		fuel.BurnTime = 5f;

		FireBaseCategory fire = new FireBaseCategory ();
		fire.BurnRateMultiplier = 1f;
		fire.FuelRemaining = 10f;
		fire.StartingFuel = 10f;

		ShelterCategory shelter = new ShelterCategory();
		shelter.WarmthRate = 2;

		RaftCategory raft = new RaftCategory();
		raft.Speed = 1f;
		raft.InventorySize = 5;

		WarmthIdolCategory warmthIdol = new WarmthIdolCategory ();
		warmthIdol.Equiped = 0f;
		warmthIdol.WarmthBenefit = 1;

		LightCategory light = new LightCategory ();
		light.Brightness = 2f;
		light.BurnRate = 0.75f;
		light.CurrentFuelLevel = 3f;
		light.MaxFuel = 5f;

		EquipableCategory equipable = new EquipableCategory ();
		equipable.Equiped = 0f;

		item.AddItemCategory(solid);
		item.AddItemCategory(plant);
		item.AddItemCategory(flesh);
		item.AddItemCategory(container);
		item.AddItemCategory(medicine);
		item.AddItemCategory(cloth);
		item.AddItemCategory(fuel);
		item.AddItemCategory(fire);
		item.AddItemCategory(shelter);
		item.AddItemCategory(raft);
		item.AddItemCategory(warmthIdol);
		item.AddItemCategory(light);
		item.AddItemCategory(equipable);

		return item;
	}

	[Test]
	public void AddItemCategoryMethodShouldUpdateGetItemCategoryList()
	{
		//Arrange
		BaseItem item = createTestItem();
		ItemStack itemStack = new ItemStack (item, 4, "");

		Assert.AreEqual (13, itemStack.Item.GetItemCategories().Count);

		PlantCategory plant = new PlantCategory();
		item.AddItemCategory(plant);

		// Act
		itemStack.Item.GetItemCategories();

		// Assert
		Assert.AreEqual (14, itemStack.Item.GetItemCategories().Count);
	}

	[Test]
	public void BaseItemDuplicationTest()
	{
		//Arrange
		BaseItem stick = createTestItem();

		// Act
		BaseItem duplicateStick = new BaseItem(stick);

		//Assert
		Assert.AreEqual(stick.ItemName, duplicateStick.ItemName);
		Assert.AreEqual(stick.FlavorText, duplicateStick.FlavorText);
		Assert.AreEqual(stick.InventorySprite, duplicateStick.InventorySprite); 
		Assert.AreEqual(stick.WorldModel, duplicateStick.WorldModel); 

		Assert.AreEqual(stick.Types.Count, duplicateStick.Types.Count); 

		if(stick.Types.Count == duplicateStick.Types.Count)
		{
			for(int i = 0; i < stick.Types.Count; ++i)
			{
				Assert.AreEqual(stick.Types[i], duplicateStick.Types[i]); 
			}
		}

		Assert.AreEqual(stick.GetItemCategories().Count, duplicateStick.GetItemCategories().Count);

		Assert.AreEqual(stick.GetItemAttributes().Count, duplicateStick.GetItemAttributes().Count);

		if(stick.GetItemAttributes().Count == duplicateStick.GetItemAttributes().Count)
		{
			List<ItemAttribute> attributes = stick.GetItemAttributes();
			List<ItemAttribute> duplicateAttributes = duplicateStick.GetItemAttributes();

			for(int i = 0; i < attributes.Count; ++i)
			{
				Assert.AreEqual(attributes[i].Name, duplicateAttributes[i].Name);
				Assert.AreEqual(attributes[i].Value, duplicateAttributes[i].Value);
			}
		}

		Assert.AreEqual(stick.GetPossibleActions().Count, duplicateStick.GetPossibleActions().Count);

		if(stick.GetPossibleActions().Count == duplicateStick.GetPossibleActions().Count)
		{
			List<ItemAction> actions = stick.GetPossibleActions();
			List<ItemAction> duplicateActions = duplicateStick.GetPossibleActions();

			for(int i = 0; i < actions.Count; ++i)
			{
				Assert.AreEqual(actions[i].ActionName, duplicateActions[i].ActionName);
				Assert.AreEqual(actions[i].ActionComplete, duplicateActions[i].ActionComplete);
				Assert.AreEqual(actions[i].Conditions.Count, duplicateActions[i].Conditions.Count);
			}
		}
	}

	[Test]
	public void ChangeItemNameTest()
	{
		//Arrange
		BaseItem stick = createTestItem();

		// Assert initial name
		Assert.AreEqual(stick.ItemName, "Sample Item");

		// Act
		stick.ChangeName("Changed Item");

		Assert.AreEqual(stick.ItemName, "Changed Item");
	}

	[Test]
	public void RemoveGameCategoriesTest()
	{
		//Arrange
		BaseItem notExcludingTestStick = createTestItem();
		BaseItem excludingTestStick = new BaseItem(notExcludingTestStick);

		//Act
		notExcludingTestStick.RemoveCategoriesSpecified(new List<string> { "SolidCategory" });
		Assert.AreEqual(12, notExcludingTestStick.GetItemCategories().Count);

		for(int i = 0; i < notExcludingTestStick.GetItemCategories().Count; ++i)
		{
			Assert.AreNotEqual("SolidCategory", notExcludingTestStick.GetItemCategories()[i].GetType().Name);
		}

		excludingTestStick.RemoveCategoriesExcluding(new List<string> {"SolidCategory"});

		Assert.AreEqual(1, excludingTestStick.GetItemCategories().Count);
		Assert.AreEqual("SolidCategory", excludingTestStick.GetItemCategories()[0].GetType().Name);
	}
}


