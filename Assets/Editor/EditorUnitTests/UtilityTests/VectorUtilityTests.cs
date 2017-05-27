using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class VectorUtilityTests
{
	[Test]
	public void GetSlopeTest()
	{
		// test for zero slope
		Vector2 v1 = new Vector3(1,0);
		Vector2 v2 = new Vector2(5,0);
		Assert.AreEqual(0, VectorUtility.GetSlope(v1, v2));

		// test for infinite slope
		Vector2 v3 = new Vector2(0,5);
		Vector2 v4 = new Vector2(0,3);
		Assert.AreEqual(Mathf.Infinity, VectorUtility.GetSlope(v3, v4));

		// test for regular occasion
		Vector2 v5 = new Vector2(1,2);
		Vector2 v6 = new Vector2(3,4);
		Assert.AreEqual((4f-2f)/(3f-1f), VectorUtility.GetSlope(v5, v6));
	}

	[Test]
	public void GetPerpindicularSlopeTest()
	{
		// check for 0 slope
		Vector2 v1 = new Vector2(0,5);
		Vector2 v2 = new Vector2(0,3);
		Assert.AreEqual(0, VectorUtility.GetPerpindicularSlope(v1,v2));

		// check for infinite slope
		Vector2 v3 = new Vector2(1,0);
		Vector2 v4 = new Vector2(5,0);
		Assert.AreEqual(Mathf.Infinity, VectorUtility.GetPerpindicularSlope(v3, v4));

		// check for regular slope
		Vector2 v5 = new Vector2(1,2);
		Vector2 v6 = new Vector2(3,4);
		Assert.AreEqual(1.0f, VectorUtility.GetSlope(v5, v6));
	}

	[Test]
	public void GetAngleTest()
	{
		// check for 0 angle
		Vector2 v1 = new Vector2(0,2);
		Vector2 v2 = new Vector2(0,5);
		Assert.AreEqual(0, VectorUtility.GetAngle(v1,v2));

		// check for regular angle
		Vector2 v3 = new Vector2(1,2);
		Vector2 v4 = new Vector2(3,4);
		Assert.AreEqual(0.785398185f, VectorUtility.GetAngle(v3, v4));
	}

	[Test]
	public void XZTest()
	{
		Vector3 v1       = new Vector3(3,4,5);
		Vector2 expected = new Vector2(3,5);
		Assert.AreEqual(expected, VectorUtility.XZ(v1));
	}

	[Test]
	public void TwoDimensional3dTest() 
	{
		// test if no value given for y works
		Vector3 v1 = new Vector2(2,3);
		Vector3 v2 = VectorUtility.twoDimensional3d(v1);
		Assert.AreEqual(0, v2.y);

		// test if setting y works
		Vector2 v3 = VectorUtility.twoDimensional3d(v1, 3);
		Assert.AreEqual(3, v3.y);
	}

	[Test]
	public void HeightVector3dTest()
	{
		Vector3 v1 = VectorUtility.HeightVector3d(3);
		Vector3 v2 = new Vector3(0,3,0);
		Assert.AreEqual(v2, v1);
	}

	[Test]
	public void HeightVector2dTest()
	{
		Vector2 v1 = VectorUtility.HeightVector2d(3);
		Vector2 v2 = new Vector2(0,3);
		Assert.AreEqual(v2, v1);
	}
}

