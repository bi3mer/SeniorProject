using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class GameLoaderTaskTest 
{
	[Test]
	public void ConstructsGameLoaderTask ()
	{
		GameLoaderTask task = new GameLoaderTask("My Task");

		Assert.AreEqual(task.Name, "My Task");
		Assert.AreEqual(task.PercentageComplete, 0f);
	}

	[Test]
	public void SetsPercentageComplete ()
	{
		GameLoaderTask task = new GameLoaderTask("My Task");

		task.PercentageComplete = 0.5f;

		Assert.AreEqual(task.PercentageComplete, 0.5f);

		task.PercentageComplete = -1f;

		Assert.AreEqual(task.PercentageComplete, 0f);

		task.PercentageComplete = 10f;

		Assert.AreEqual(task.PercentageComplete, 1f);
	}
}
