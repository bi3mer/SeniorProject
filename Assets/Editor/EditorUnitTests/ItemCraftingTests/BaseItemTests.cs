using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
public class BaseItemTests
{
	[Test]
	public void AddItemCategoryMethodShouldUpdateGetItemCategoryList()
	{
		//Arrange
		BaseItem stick = new BaseItem("Sample Stick");
		SolidCategory solid = new SolidCategory ();
		stick.AddItemCategory (solid);
		Stack stickStack = new Stack (stick, 4, "");

		// Act
		stickStack.Item.GetItemCategories();

		// Assert
		Assert.AreEqual (1, stickStack.Item.GetItemCategories().Count);
	}
}


