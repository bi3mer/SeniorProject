using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RandomUtilityTests
{
	[Test]
	public void RandomBinomialTest ()
	{
        for (int i = 0; i < 100; ++i)
        {
            float res = RandomUtility.RandomBinomial;
            Assert.IsTrue(res >= -1 && res <= 1);
        }
    }

    [Test]
    public void RandomPercentTest()
    {
        for (int i = 0; i < 100; ++i)
        {
            float val = RandomUtility.RandomPercent;
            Assert.IsTrue(val >= 0 && val <= 1);
        }
    }

    [Test]
    public void RandomHundredPercentTest()
    {
        for (int i = 0; i < 100; ++i)
        {
            float val = RandomUtility.RandomHundredPercent;
            Assert.IsTrue(val >= 0 && val <= 100);
        }
    }

    [Test]
    public void RandomVector2dTest()
    {
        Vector2 min = new Vector2(0, 0);
        Vector2 max = new Vector2(10, 10);
        Vector2 res = RandomUtility.RandomVector2d(min, max);

        Assert.IsTrue(res.x >= min.x && res.x <= max.x);
        Assert.IsTrue(res.y >= min.y && res.y <= max.y);
    }

    [Test]
    public void RandomVector3dTest()
    {
        Vector3 min = new Vector3(0, 0, 0);
        Vector3 max = new Vector3(10, 10, 10);
        Vector3 res = RandomUtility.RandomVector3d(min, max);

        Assert.IsTrue(res.x >= min.x && res.x <= max.x);
        Assert.IsTrue(res.y >= min.y && res.y <= max.y);
        Assert.IsTrue(res.z >= min.z && res.z <= max.z);
    }

    [Test]
    public void RandomVector2dMaxMagnitudeTest()
    {
        const float maxMagnitude = 10;

        for (int i = 0; i < 100; ++i)
        {
            Vector2 res = RandomUtility.RandomVector2d(maxMagnitude);

            // Floor is used to avoid floating point errors that can
            // cause this test to fail when it shouldn't
            Assert.IsTrue(Math.Floor(res.magnitude) <= maxMagnitude);
        }
    }

    [Test]
    public void RandomBoolTest()
    {
        const int maxIterations = 1000;
        bool trueFound  = false;
        bool falseFound = false;

        for (int i = 0; i < maxIterations && !(trueFound && falseFound); ++i)
        {
            bool rand = RandomUtility.RandomBool;

            if (rand == true) trueFound  = true;
            else              falseFound = true;
        }

        Assert.IsTrue(trueFound);
        Assert.IsTrue(falseFound);
    }
}

