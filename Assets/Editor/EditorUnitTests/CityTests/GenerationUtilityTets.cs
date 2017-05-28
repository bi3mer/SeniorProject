using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class GenerationUtilityTests
{
	[Test]
	public void ConvertsToAlignedVector3 ()
	{
		Vector3 result = GenerationUtility.ToAlignedVector3 (Vector2.right);

		Assert.AreEqual(result.x, 1f);
		Assert.AreEqual(result.y, 0f);
		Assert.AreEqual(result.z, 0f);
	}

	[Test]
	public void ConvertsToAlignedVector2 ()
	{
		Vector2 result = GenerationUtility.ToAlignedVector2 (Vector3.up);

		Assert.AreEqual(result.x, 0f);
		Assert.AreEqual(result.y, 0f);
	}

	[Test]
	public void AddsNonDuplicateVertex ()
	{
		List<Vector2> verts = new List<Vector2>();

		verts = GenerationUtility.AddNonDuplicateVertex(verts, Vector2.up);

		Assert.AreEqual(verts.Count, 1);

		verts = GenerationUtility.AddNonDuplicateVertex(verts, Vector2.up);

		Assert.AreEqual(verts.Count, 1);
	}

	[Test]
	public void ChecksPointInPolygon ()
	{
		Vector2[] polygon = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

		Assert.IsFalse(GenerationUtility.IsPointInPolygon(Vector2.up * 2, ref polygon));
		Assert.IsTrue(GenerationUtility.IsPointInPolygon(Vector2.zero, ref polygon));
	}

	[Test]
	public void GetsMidpoint ()
	{
		Vector2[] verts = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

		Vector2 result = GenerationUtility.GetMidpoint(new List<Vector2>(verts));

		Assert.AreEqual(result, Vector2.zero);
	}

	[Test]
	public void SortsClockwise ()
	{
		Vector2[] verts = new Vector2[] { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

		List<Vector2> result = GenerationUtility.SortVerticies(new List<Vector2>(verts), Vector2.zero);

		Assert.AreEqual(result[0].x, 0.0);
		Assert.AreEqual(result[0].y, -1.0);
		Assert.AreEqual(result[1].x, 1.0);
		Assert.AreEqual(result[1].y, 0.0);
		Assert.AreEqual(result[2].x, 0.0);
		Assert.AreEqual(result[2].y, 1.0);
		Assert.AreEqual(result[3].x, -1.0);
		Assert.AreEqual(result[3].y, 0.0);
	}

	[Test]
	public void GetsMostCommonVertex ()
	{
		District d = new District (Vector3.zero, new Vector3[] { Vector3.up, Vector3.zero, Vector3.right }, new DistrictConfiguration());
		District e = new District (Vector3.zero, new Vector3[] { Vector3.left, Vector3.right, Vector3.back }, new DistrictConfiguration());

		Assert.AreEqual(Vector3.right, GenerationUtility.GetMostCommonVertex( new District[] { d, e } ));
	}
}