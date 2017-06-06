using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[TestFixture]
public class HealthRateTests
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
	public void PlayerHealthShouldStartAtMaxHealthTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Assert
		Assert.AreEqual (mockPlayer.MaxHealth, mockPlayer.Health);
	}

	[Test]
	public void PlayerHealthCannotExceedMaxHealthTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Act
		mockPlayer.Controller.PlayerStatManager.HealthRate.AffectHealthByGivenAmount(1);

		// Assert
		Assert.AreEqual (mockPlayer.MaxHealth, mockPlayer.Health);
	}

	[Test]
	public void PlayerHealthIncreasesBasedOnGivenAmountTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Act
		mockPlayer.Controller.PlayerStatManager.HealthRate.AffectHealthByGivenAmount(-10);
		mockPlayer.Controller.PlayerStatManager.HealthRate.AffectHealthByGivenAmount (9);

		// Assert
		Assert.AreEqual(99, mockPlayer.Health);
	}

	[Test]
	public void PlayerHealthDecreasesBasedOnGivenAmountTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Act
		mockPlayer.Controller.PlayerStatManager.HealthRate.AffectHealthByGivenAmount(-10);


		// Assert
		Assert.AreEqual(90, mockPlayer.Health);
	}

	[Test]
	public void PlayerHealthDecreasesBasedOnFallDamageAmountTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Act
		mockPlayer.Controller.PlayerStatManager.HealthRate.TakeFallDamage(10);


		// Assert
		Assert.AreEqual(90, mockPlayer.Health);
	}
}