using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class DistrictTest
{

	private DistrictConfiguration makeDistrictConfig ()
	{
		DistrictConfiguration config = new DistrictConfiguration();

		return config;
	}

	[Test]
	public void ConstructsDistrict ()
	{
		DistrictConfiguration config = 	makeDistrictConfig();
		Vector3[] verts = 				new Vector3[] { Vector3.up };

		District district = new District(Vector3.zero, verts, config);

		Assert.AreEqual(district.SeedPoint, Vector3.zero);
		Assert.AreEqual(district.EdgeVerticies, verts);
		Assert.AreEqual(district.Configuration, config);
		Assert.IsNotNull(district.Blocks);
	}

	[Test]
	public void GetsDistrictName ()
	{
		DistrictConfiguration config = 	makeDistrictConfig();
		Vector3[] verts = 				new Vector3[] { Vector3.forward };

		District district = new District(Vector3.zero, verts, config);

		Assert.AreEqual(district.Name, config.Name);
	}

	[Test]
	public void CalculateBounds ()
	{
		DistrictConfiguration config = 	makeDistrictConfig();
		Vector3[] verts = 				new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

		District district = new District(Vector3.zero, verts, config);

		Assert.AreNotEqual(district.BoundingBox, new Bounds());
	}

	[Test]
	public void ChecksPointContained ()
	{
		DistrictConfiguration config = 	makeDistrictConfig();
		Vector3[] verts = 				new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

		District district = new District(Vector3.zero, verts, config);

		Assert.IsFalse(district.ContainsPoint(2*Vector3.left));
		Assert.IsTrue(district.ContainsPoint(Vector3.zero));
	}
}
