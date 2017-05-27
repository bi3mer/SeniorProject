using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BlockTest 
{
	[Test]
	public void ConstructsBlock ()
	{
		Vector3[] verts = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

		Block block = new Block (Vector3.left, verts);

		Assert.AreEqual(block.Center, Vector3.left);
		Assert.AreEqual(block.Verticies, verts);
		Assert.NotNull(block.Buildings);
	}

	[Test]
	public void CalculatesBounds ()
	{
		Vector3[] verts = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

		Block block = new Block (Vector3.left, verts);

		Assert.AreNotEqual(block.BoundingBox, new Bounds()); 
	}

	[Test]
	public void CheckContainsPoint ()
	{
		Vector3[] verts = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

		Block block = new Block (Vector3.left, verts);

		Assert.IsTrue(block.ContainsPoint(Vector3.zero)); 
		Assert.IsFalse(block.ContainsPoint(Vector3.right * 2)); 
	}
}
