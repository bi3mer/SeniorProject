using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class PressureSystemTest
{
    // [Test]
    // public void UpdateHighPressureOverWater()
    // {
        // TODO: I haven't been able to figure out how to get the raycast to work
        //       properly here. 
        //PressureSystem ps = new PressureSystem();
        //ps.Position       = new Vector2(0, 0);
        //ps.Pressure       = 0;
        //ps.IsHighPressure = true;

        //GameObject go         = new GameObject();
        //go.transform.position = Vector3.zero;
        //BoxCollider bc        = go.AddComponent<BoxCollider>();
        //bc.center             = go.transform.position;
        //bc.size               = new Vector3(1, 1, 1);
        //bc.tag                = "Water";
        //FloodWater fw         = go.AddComponent<FloodWater>();

        //ps.UpdatePressure();
        //Assert.AreEqual(-0.5f, ps.Pressure);
        //UnityEngine.Object.Destroy(go);
    // }

    // [Test]
    // public void UpdateLowPressureOverWater()
    // {
        // // TODO: I haven't been able to figure out how to get the raycast to work
        //       properly here. 
        //PressureSystem ps = new PressureSystem();
        //ps.Position       = new Vector2(0, 0);
        //ps.Pressure       = 0;
        //ps.IsHighPressure = false;

        //GameObject go         = new GameObject();
        //go.transform.position = Vector3.zero;
        //BoxCollider bc        = go.AddComponent<BoxCollider>();
        //bc.center             = go.transform.position;
        //bc.size               = new Vector3(1, 1, 1);
        //bc.tag                = "Water";
        //FloodWater fw         = go.AddComponent<FloodWater>();

        //ps.UpdatePressure();
        //Assert.AreEqual(-1.0f, ps.Pressure);
        //UnityEngine.Object.Destroy(go);
    // }

    [Test]
    public void UpdateLowPressureOverLand()
    {
        PressureSystem ps = new PressureSystem();
        ps.Position       = new Vector2(0, 0);
        ps.Pressure       = 0;
        ps.IsHighPressure = false;

        ps.UpdatePressure();
        Assert.AreEqual(0.5f, ps.Pressure);
    }

    [Test]
    public void UpdateHighPressureOverLand()
    {
        PressureSystem ps = new PressureSystem();
        ps.Position       = new Vector2(0, 0);
        ps.Pressure       = 0;
        ps.IsHighPressure = true;

        ps.UpdatePressure();
        Assert.AreEqual(1.0f, ps.Pressure);
    }
}
