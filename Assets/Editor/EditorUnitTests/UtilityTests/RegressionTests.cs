using System;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RegressionTests
{
	private readonly float[] windSpeedCoefficents = {0f, -13.641617f, 0.00670152751f};
	private readonly int[,] windSpeedPowers       = {{0}, {1}, {2}};
	private const float windSpeedIntercept        = 6951.17066465f;

	[Test]
	public void RegressionTest()
	{
		float[] inputs = {3};
		float val = Regression.Prediction(windSpeedCoefficents, windSpeedPowers, inputs, windSpeedIntercept);
		Assert.AreEqual(6910.30664f, val);
	}
}

