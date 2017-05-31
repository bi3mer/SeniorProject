using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class PlayerTest 
{
	[Test]
	public void ConstructsPlayer ()
	{
		PlayerInventory inventory = new PlayerInventory("My Ivnentory", 20);
		Player player = new Player(inventory);

		Assert.AreEqual(player.Inventory, inventory);
		Assert.AreEqual(player.MaxHealth, 100);
		Assert.AreEqual(player.Health, 100);
		Assert.AreEqual(player.MaxHunger, 100);
		Assert.AreEqual(player.Hunger, 100);
		Assert.AreEqual(player.MaxWarmth, 100);
		Assert.AreEqual(player.Warmth, 100);
		Assert.AreEqual(player.HealthStatus, PlayerHealthStatus.None);
		Assert.AreEqual(player.WorldPosition, Vector3.zero);
	}

	[Test]
	public void SetsPlayerHealth ()
	{
		PlayerInventory inventory = new PlayerInventory("My Ivnentory", 20);
		Player player = new Player(inventory);

		player.Health = 50;

		Assert.AreEqual(player.Health, 50);

		player.Health = -1;

		Assert.AreEqual(player.Health, 0);

		player.Health = 101;

		Assert.AreEqual(player.Health, 100);
	}

	[Test]
	public void SetsPlayerWarmth ()
	{
		PlayerInventory inventory = new PlayerInventory("My Ivnentory", 20);
		Player player = new Player(inventory);

		player.Warmth = 50;

		Assert.AreEqual(player.Warmth, 50);

		player.Warmth = -1;

		Assert.AreEqual(player.Warmth, 0);

		player.Warmth = 101;

		Assert.AreEqual(player.Warmth, 100);
	}

	[Test]
	public void SetsPlayerHunger ()
	{
		PlayerInventory inventory = new PlayerInventory("My Ivnentory", 20);
		Player player = new Player(inventory);
		
		player.Hunger = 50;

		Assert.AreEqual(player.Hunger, 50);

		player.Hunger = -1;

		Assert.AreEqual(player.Hunger, 0);

		player.Hunger = 101;

		Assert.AreEqual(player.Hunger, 100);
	}
}
