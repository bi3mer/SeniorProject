using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class WorldItemFactoryTests 
{
	[Test]
	public void TestGetAllInteractables()
	{
		WorldItemFactory factory = new WorldItemFactory();
		Dictionary<string, List<GameObject>> allLandItems = factory.GetAllInteractableItemsByDistrict(false, false);
		Dictionary<string, List<GameObject>> allWaterItems = factory.GetAllInteractableItemsByDistrict(false, true);

		List<string> districts = new List<string>{"business", "residential", "shopping"};
		List<int> landItemCounts = new List<int> {13, 13, 15};
		List<int> waterItemCounts = new List<int> {10, 9, 10};

		int current = 0;

		foreach(string district in districts)
		{
			Assert.IsTrue(districts.Contains(district));
			Assert.AreEqual(allLandItems[district].Count, landItemCounts[current]);
			Assert.AreEqual(allWaterItems[district].Count, waterItemCounts[current]);
			++current;
		}
	}

	[Test]
	public void CreatePickUpInteractableItemTest()
	{
		WorldItemFactory factory = new WorldItemFactory();
		ItemFactory itemFactory = new ItemFactory();

		GameObject item = factory.CreatePickUpInteractableItem(itemFactory.GetBaseItem("River Reed"), 1);
		PickUpItem pickupItem = item.GetComponent<PickUpItem>();

		Assert.AreNotEqual(pickupItem, null);
		Assert.AreEqual(pickupItem.name, "River Reed");
		Assert.AreEqual(pickupItem.Amount, 1);
	}
}
