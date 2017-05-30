using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections;

public class CityChunkManagerTest
{
	[Test]
	public void ConstructsCityChunkManager () 
	{
		Bounds bounds = new Bounds();
		bounds.Encapsulate(Vector3.right);

		CityChunk chunk = new CityChunk(1, 2, bounds);

		Assert.AreEqual(chunk.Location.X, 1);
		Assert.AreEqual(chunk.Location.Y, 2);
		Assert.AreEqual(chunk.BoundingBox, bounds);
		Assert.AreEqual(chunk.IsLoaded, true);
		Assert.NotNull(chunk.Buildings);
	}
}
