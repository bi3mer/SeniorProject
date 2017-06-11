using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

[TestFixture]
public class WarmthRateTests
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
	public void PlayerHealthShouldStartAtMaxWarmthTest()
	{
		// Arrange
		Player mockPlayer = createTestPlayer();

		// Assert
		Assert.AreEqual (mockPlayer.MaxWarmth, mockPlayer.Warmth);
	}
}