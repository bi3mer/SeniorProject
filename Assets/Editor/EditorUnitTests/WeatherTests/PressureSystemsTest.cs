using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class PressureSystemsTest
{
    [Test]
    public void Initialized()
    {
        PressureSystems ps = new PressureSystems();
        Assert.AreEqual(false, ps.Initialized);

        CityBoundaries cb = new CityBoundaries();
        cb.CityBounds = new Bounds(Vector3.zero, new Vector3(2, 2, 2));
        ps.Initialize(cb);
        Assert.AreEqual(true, ps.Initialized);
    }

    [Test]
    public void PressureSystemsSize()
    {
        PressureSystems ps = new PressureSystems();
        CityBoundaries cb = new CityBoundaries();
        cb.CityBounds = new Bounds(Vector3.zero, new Vector3(2, 2, 2));
        ps.Initialize(cb);

        Assert.AreEqual(10, ps.LocalPressureSystems.Count);
    }

    [Test]
    public void ClosestPressureSystem()
    {
        PressureSystems ps = new PressureSystems();
        CityBoundaries cb = new CityBoundaries();
        cb.CityBounds = new Bounds(Vector3.zero, new Vector3(2, 2, 2));
        ps.Initialize(cb);

        // get closest position near 0, 0
        PressureSystem closest = ps.GetClosestPressureSystem(Vector2.zero);
        float dist = Vector2.Distance(closest.Position, Vector2.zero);

        // check each position to make sure distnace <= than the closest pressure
        // system's distance.
        for (int i = 0; i < ps.LocalPressureSystems.Count; ++i)
        {
            Assert.IsTrue(dist <= Vector2.Distance(ps.LocalPressureSystems[i].Position, Vector2.zero));
        }
    }

    [Test]
    public void GetLowPressureSystems()
    {
        PressureSystems ps = new PressureSystems();
        CityBoundaries cb = new CityBoundaries();
        cb.CityBounds = new Bounds(Vector3.zero, new Vector3(2, 2, 2));
        ps.Initialize(cb);

        List<PressureSystem> lows = ps.LowPressureSystems();

        for (int i = 0; i < lows.Count; ++i)
        {
            Assert.IsTrue(lows[i].IsHighPressure == false);
        }
    }

    // NOTE: UpdatePressureSystem is not tested since it is far to random and variable to properly
    //       test. 
}
