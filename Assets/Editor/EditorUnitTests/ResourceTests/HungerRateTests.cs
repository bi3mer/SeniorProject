using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[TestFixture]
public class HungerRateTests
{
	private Player createTestPlayer()
	{
		PlayerInventory mockPlayerInventory = new PlayerInventory("player", 20);
		PlayerController controller = new GameObject().AddComponent<PlayerController>();
		controller.PlayerStatManager = new PlayerStatManager ();

		Game.Instance.PlayerInstance = new Player(mockPlayerInventory);
		Game.Instance.PlayerInstance.Controller = controller;

		return Game.Instance.PlayerInstance;
	}

	[Test]
	public void PlayerHungerShouldStartAtMaxHungerTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Assert
		Assert.AreEqual (mockPlayer.MaxHunger, mockPlayer.Hunger);
	}

	[Test]
	public void PlayerHungerUseFoodEnergyBasedOnAmountTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Act
		mockPlayer.Controller.PlayerStatManager.HungerRate.UseFoodEnergy(-1);

		// Assert
		Assert.AreEqual (99, mockPlayer.Hunger);
	}
}