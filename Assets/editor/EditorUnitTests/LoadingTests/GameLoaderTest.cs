using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class GameLoaderTest 
{
	[Test]
	public void ConstructsGameLoader ()
	{
		GameLoader loader = new GameLoader();

		Assert.IsFalse(loader.GameLoaded);
		Assert.AreEqual(loader.PercentageComplete, 1f);
	}

	[Test]
	public void CreatesAndAddsTasks ()
	{
		GameLoader loader = new GameLoader();

		loader.CreateGameLoaderTask("My Task");

		Assert.AreEqual(loader.PercentageComplete, 0f);
		Assert.AreEqual(loader.CurrentTask, "My Task...");
	}

	[Test]
	public void TracksPercentageComplete ()
	{
		GameLoader loader = new GameLoader();

		GameLoaderTask task1 = loader.CreateGameLoaderTask("My Task");
		GameLoaderTask task2 = loader.CreateGameLoaderTask("My Other Task");

		Assert.AreEqual(loader.PercentageComplete, 0f);
		Assert.AreEqual(loader.CurrentTask, "My Task...");

		task1.PercentageComplete = 0.5f;

		Assert.AreEqual(loader.PercentageComplete, 0.25f);
		Assert.AreEqual(loader.CurrentTask, "My Task...");

		task1.PercentageComplete = 1f;

		Assert.AreEqual(loader.PercentageComplete, 0.5f);
		Assert.AreEqual(loader.CurrentTask, "My Other Task...");

		task2.PercentageComplete = 0.5f;

		Assert.AreEqual(loader.PercentageComplete, 0.75f);
		Assert.AreEqual(loader.CurrentTask, "My Other Task...");
	}

	[Test]
	public void TriggersGameLoadedEvent ()
	{
		GameLoader loader = new GameLoader();
		bool wasTriggered = false;
		loader.GameLoadedEvent += () => {
			wasTriggered = true;
		};

		GameLoaderTask task1 = loader.CreateGameLoaderTask("My Task");
		GameLoaderTask task2 = loader.CreateGameLoaderTask("My Other Task");

		task1.PercentageComplete = 1f;
		task2.PercentageComplete = 1f;

		Assert.AreEqual(loader.PercentageComplete, 1f);
		Assert.IsTrue(loader.GameLoaded);
		Assert.IsTrue(wasTriggered);
	}

	[Test]
	public void Resets ()
	{
		GameLoader loader = new GameLoader();

		GameLoaderTask task1 = loader.CreateGameLoaderTask("My Task");
		GameLoaderTask task2 = loader.CreateGameLoaderTask("My Other Task");

		task1.PercentageComplete = 1f;
		task2.PercentageComplete = 1f;
		loader.Reset();

		loader.CreateGameLoaderTask("My Brand New Task");

		Assert.AreEqual(loader.PercentageComplete, 0f);
		Assert.IsFalse(loader.GameLoaded);
		Assert.AreEqual(loader.CurrentTask, "My Brand New Task...");
	}
}